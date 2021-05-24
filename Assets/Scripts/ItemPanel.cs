using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    private PlayerInteraction playerInteractionScript;
    private bool examining;
    public GameObject inventoryGameObject;
    public Inventory inventory;
    private int itemSlotIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerInteractionScript = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            CloseItemPanel();
        }
    }
    
    public void OpenItemPanel(InventoryItem item, int slotIndex)
    {
        if (inventoryGameObject.activeSelf)
        {
            examining = true;
            inventory.CloseInventory(false);
            itemSlotIndex = slotIndex;
        }
        
        Time.timeScale = 0;
        GameManager.instance.blockPlayerMovement = true;
        GameManager.instance.interactingUI = true;
        gameObject.SetActive(true);
        gameObject.GetComponentInChildren<RawImage>().texture = item.itemImage;
        GameManager.instance.ActivateBlur();
    }

    private void CloseItemPanel()
    {
        if (GetComponentInChildren<TutorialTrigger>() != null)
        {
            GetComponentInChildren<TutorialTrigger>().ActivateTutorial();    
        }

        Time.timeScale = 1;
        GameManager.instance.DisableBlur();
        playerInteractionScript.interacting = false;
        GameManager.instance.blockPlayerMovement = false;
        GameManager.instance.interactingUI = false;
        
        if (examining)
        {
            examining = false;
            inventory.OpenInventory(false);
            
            foreach (var slot in inventory.slots)
            {
                if (slot.GetComponent<Slot>().slotIndex == itemSlotIndex)
                {
                    EventSystem.current.SetSelectedGameObject(slot);
                }
            }
        }
        
        gameObject.SetActive(false); 
    }
}
