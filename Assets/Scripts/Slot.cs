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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inventoryNavigation = transform.parent.GetComponent<InventoryNavigation>();
        itemPanel = inventoryNavigation.itemPanel;
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
        if (item != null)
        {
            itemIcon.texture = item.itemIcon;
        }
        else
        {
            itemIcon.texture = null;
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

        selected = true;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (item == null) return;

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
                inventoryNavigation.inventory.CloseInventory(false);
                GameManager.instance.RemoveItemFromInventory(item);
                break;
        }
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     anim.SetBool("Selected", true);
    //     AudioManager.instance.Play("UINavigation");
    //
    //     if (selected)
    //     {
    //         InteractWithItem();
    //     }
    //     
    //     selected = true;
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     anim.SetBool("Selected", false);
    //     selected = false;
    // }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (selected)
        {
            InteractWithItem();
        }
    }
}
