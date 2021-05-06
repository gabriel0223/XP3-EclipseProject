using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SearchableItem
{
    public InventoryItem item;
    public int quantity;
}
public class Searchable : MonoBehaviour
{
    [HideInInspector] public bool isClose;
    [HideInInspector] public PlayerInteraction playerInteraction;
    public SearchInventory searchInventory;
    public string name;
    //public List<InventoryItem> items = new List<InventoryItem>();
    public List<SearchableItem> items = new List<SearchableItem>();

    private void Awake()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose && Input.GetKeyDown(KeyCode.F))
        {
            searchInventory.currentSearchable = this;
            searchInventory.OpenInventory();
            playerInteraction.keyPressIcon.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = true;
            playerInteraction.keyPressIcon.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
            playerInteraction.keyPressIcon.SetActive(false);
        }
    }
}
