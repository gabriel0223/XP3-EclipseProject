using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SearchableItem
{
    public InventoryItem item;
    public int quantity;
}
public class Searchable : MonoBehaviour
{
    public enum SearchableState
    {
        Open, Closed
    }

    public enum SearchableType
    {
        Cabinet, Chest
    }

    [HideInInspector] public bool isClose;
    [HideInInspector] public PlayerInteraction playerInteraction;
    private Animator anim;
    public SearchableType searchableType;
    public SearchInventory searchInventory;
    public string name;
    [HideInInspector] public string id; 
    public bool locked;
    public SearchableState searchableState;
    public List<SearchableItem> items = new List<SearchableItem>();

    private void Awake()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        searchableState = SearchableState.Closed;
        anim = GetComponent<Animator>();
        
        id = SceneManager.GetActiveScene().name + gameObject.name + Math.Sqrt(transform.position.magnitude) + transform.rotation;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.saveFile.KeyExists(id))
        {
            items = LevelManager.saveFile.Load<List<SearchableItem>>(id);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose && !GameManager.instance.interactingUI && Input.GetKeyDown(KeyCode.F))
        {
            if (searchableState == SearchableState.Open) return;
            Search();
        }
    }

    void Search()
    {
        searchInventory.currentSearchable = this;
        searchInventory.OpenInventory();
        playerInteraction.keyPressIcon.SetActive(false);
        anim.SetTrigger("Open");

        switch (searchableType)
        {
            case SearchableType.Cabinet:
                AudioManager.instance.Play("ArmarioAbrir");
                break;
            case SearchableType.Chest:
                AudioManager.instance.Play("BauAbrir");
                break;
        }
        
        searchableState = SearchableState.Open;
    }

    public void Close()
    {
        anim.SetTrigger("Close");
        
        switch (searchableType)
        {
            case SearchableType.Cabinet:
                AudioManager.instance.Play("ArmarioFechar");
                break;
             case SearchableType.Chest:
                 AudioManager.instance.Play("BauFechar");
                 break;
        }
        
        searchableState = SearchableState.Closed;
        LevelManager.saveFile.Save(id, items);
        LevelManager.saveFile.Sync();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (locked) return;
            
            isClose = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
        }
    }
}
