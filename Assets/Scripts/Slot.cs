using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDeselectHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;

    public RawImage itemIcon;
    private Animator anim;
    private InventoryNavigation inventoryNavigation;
    private ItemPanel itemPanel;
    private bool selected;
    public int slotIndex;
    public GameObject buttomPrompt;
    private TextMeshProUGUI promptText;
    public int itemQuantity;
    private TextMeshProUGUI itemQuantityText;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inventoryNavigation = transform.parent.GetComponent<InventoryNavigation>();
        promptText = buttomPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>(true);
        itemPanel = inventoryNavigation.itemPanel;
        itemIcon = GetComponentInChildren<RawImage>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == EventSystem.current.currentSelectedGameObject)
        {
            anim.SetBool("Selected", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowItemQuantity();
        
        if (item != null)
        {
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false); 
        }
    }

    private void ShowItemQuantity()
    {
        if (itemQuantity > 1)
        {
            itemQuantityText.gameObject.SetActive(true);
            itemQuantityText.SetText(itemQuantity.ToString());
        }
        else
        {
            itemQuantityText.gameObject.SetActive(false);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        anim.SetBool("Selected", false);
        selected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        anim.SetBool("Selected", true);
        AudioManager.instance.Play("UINavigation");

        if (item == null || GameManager.instance.isSelectingItem)
        {
            buttomPrompt.SetActive(false);
            return;
        }

        switch (item.itemType)
        {
            case InventoryItem.ItemType.Examinable:
                buttomPrompt.SetActive(true);
                promptText.SetText("Examinar");
                break;
            case InventoryItem.ItemType.Consumable:
                buttomPrompt.SetActive(true);
                promptText.SetText("Usar");
                break;
            case InventoryItem.ItemType.Resource:
                buttomPrompt.SetActive(false);
                break;
        }

        selected = true;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (item == null || GameManager.instance.isSelectingItem) return;

        InteractWithItem();
    }

    public void InteractWithItem()
    {
        switch (item.itemType)
        {
            case InventoryItem.ItemType.Examinable:
                itemPanel.OpenItemPanel(item, slotIndex);
                break;
            case InventoryItem.ItemType.Consumable:
                item.consumableAction.Invoke();

                if (itemQuantity == 1)
                {
                    GameManager.instance.RemoveItemFromInventory(item); 
                    buttomPrompt.SetActive(false);
                }
                else
                {
                    itemQuantity--;
                }
                
                //inventoryNavigation.inventory.CloseInventory(false);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if (selected)
        // {
        //     InteractWithItem();
        // }
    }
}
