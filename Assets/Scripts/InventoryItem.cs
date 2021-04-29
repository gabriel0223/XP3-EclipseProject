using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItem : ScriptableObject
{
    public enum ItemType
    {
        Examinable, Consumable 
    }

    public ItemType itemType;
    public UnityEvent consumableAction;
    public Texture itemImage;
    public Texture itemIcon;
    public string itemName;
    
    [TextArea(7,7)]
    public string text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
