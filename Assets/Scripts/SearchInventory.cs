using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SearchInventory : MonoBehaviour
{
    public TextMeshProUGUI inventoryTitle;
    public Searchable currentSearchable;
    private SearchSlot[] slots;
    private InventoryItem selectedItem;

    private void Awake()
    {
        slots = GetComponentsInChildren<SearchSlot>();  
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        if (GameManager.instance.interactingUI) return;

        GameManager.instance.blockPlayerMovement = true;
        GameManager.instance.interactingUI = true;
        inventoryTitle.SetText(currentSearchable.name);
        gameObject.SetActive(true);
        AddItemsToSearch();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
    }

    public void CloseInventory()
    {
        foreach (var slot in slots)
        {
            slot.transform.localScale = Vector3.one;
        }
        
        GameManager.instance.blockPlayerMovement = false;
        GameManager.instance.interactingUI = false;
        
        if (currentSearchable.isClose)
            currentSearchable.playerInteraction.keyPressIcon.SetActive(true);
        
        ClearSearchItems();
        currentSearchable.Close();
        gameObject.SetActive(false);
    }

    public void AddItemsToSearch()
    {
        for (int i = 0; i < currentSearchable.items.Count; i++)
        {
            //slots[i].GetComponent<SearchSlot>().item = currentSearchable.items[i];
            slots[i].GetComponent<SearchSlot>().item = currentSearchable.items[i].item;
            slots[i].GetComponent<SearchSlot>().itemQuantity = currentSearchable.items[i].quantity;
            slots[i].GetComponent<SearchSlot>().SetIcon(true);
        }
    }

    public void ClearSearchItems()
    {
        foreach (var slot in slots)
        {
            slot.item = null;
            slot.SetIcon(false);
        }
    }
}
