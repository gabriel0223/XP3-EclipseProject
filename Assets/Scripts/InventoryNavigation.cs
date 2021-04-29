using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryNavigation : MonoBehaviour
{
    public PlayerInteraction playerInteraction;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemText;
    public ItemPanel itemPanel;
    private InventoryItem item;

    public Inventory inventory;

    [HideInInspector] public InventoryItem requestedItem;
    [HideInInspector] public Interactive interactedObject;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        item = EventSystem.current.currentSelectedGameObject.GetComponent<Slot>().item;

        if (item != null)
        {
            itemName.SetText(item.itemName);
            itemText.SetText(item.text);
        }
        else
        {
            itemName.SetText("");
            itemText.SetText("");
        }
    }

    public void SelectRequestedItem()
    {
        if (GameManager.instance.isSelectingItem)
        {
            if (item == interactedObject.requestedItem)
            {
                inventory.CloseInventory(true);
                interactedObject.interactiveMethod.Invoke();
                interactedObject.tag = "Untagged";
                playerInteraction.canInteract = false;
                
                if (interactedObject.destroyInteractiveAfterUsage)
                    Destroy(interactedObject.gameObject);
                
                if (interactedObject.destroyItemAfterUsage)
                    GameManager.instance.RemoveRequestedItemFromInventory(interactedObject.requestedItem);
                
                //interactedObject.RemoveRequestedItemFromInventory();
                
                GameManager.instance.isSelectingItem = false;
            }
            else
            {
                AudioManager.instance.Play("WrongItem");
            }
        }
    }
}
