using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchSlot : MonoBehaviour, IDeselectHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;

    public RawImage itemIcon;
    private Animator anim;
    private ItemPanel itemPanel;
    private bool selected;
    private SearchInventory searchInventory;
    public int slotIndex;
    public TextMeshProUGUI itemName;
    public GameObject buttomPrompt;
    private TextMeshProUGUI promptText;
    public int itemQuantity;
    private TextMeshProUGUI itemQuantityText;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        promptText = buttomPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>(true);
        itemIcon = GetComponentInChildren<RawImage>(true);
        searchInventory = transform.parent.GetComponent<SearchInventory>();
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

        ShowItemQuantity();
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
            itemName.SetText("");
            return;
        }
        
        itemName.SetText(item.itemName);
        buttomPrompt.SetActive(true);
        selected = true;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (item == null || GameManager.instance.isSelectingItem) return;

        TakeItem();
    }

    public void TakeItem()
    {
        var itemToBeTaken = searchInventory.currentSearchable.items.Find(i => i.item == item);

        buttomPrompt.SetActive(false);
        SetIcon(false);
        itemName.SetText("");

        var timesToAdd = itemQuantity;
        for (int i = 0; i < timesToAdd; i++)
        {
            GameManager.instance.AddItemToInventory(item);
            itemToBeTaken.quantity--;
            itemQuantity--;
        }

        if (itemQuantity <= 0)
        {
            itemQuantityText.gameObject.SetActive(false);
            item = null;
            searchInventory.currentSearchable.items.Remove(itemToBeTaken);
            searchInventory.currentSearchable.items.TrimExcess();
        }
    }

    public void SetIcon(bool value)
    {
        itemIcon.gameObject.SetActive(value);
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

