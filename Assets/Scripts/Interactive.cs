using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{

    [Header("NPC VARIABLES")]
    public GameObject dialoguePlayer;
    private DialoguePlayer dialoguePlayerScript;
    public Dialogue dialogue;
    private GameObject canvas;
    
    [Header("DOCUMENT VARIABLES")]
    public GameObject documentPanel;
    public Document document;

    [Header("ITEM VARIABLES")] 
    private Inventory inventory;
    public GameObject itemPanel;
    public InventoryItem item;

    [Header("ITEM REQUESTER VARIABLES")] 
    public InventoryNavigation inventoryNavigation;
    public InventoryItem requestedItem;
    public UnityEvent interactiveMethod;

    public bool destroyInteractiveAfterUsage;
    public bool destroyItemAfterUsage;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
        if (dialoguePlayer != null)
            dialoguePlayerScript = dialoguePlayer.GetComponent<DialoguePlayer>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            if (GameManager.itemsCollected.ContainsKey(item))
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {

    }

    public void Interact()
    {
        if (dialogue != null)
        {
            GameManager.instance.blockPlayerMovement = true;
            var newDialoguePlayer = Instantiate(dialoguePlayer, canvas.transform).GetComponent<DialoguePlayer>();

            newDialoguePlayer.dialogue = dialogue;
        }

        if (document != null)
        {
            GameManager.instance.blockPlayerMovement = true;
            documentPanel.SetActive(true);

            documentPanel.GetComponentInChildren<RawImage>().texture = document.documentImage;
            documentPanel.GetComponent<DocumentPanel>().documentText = document.documentText;
            GameManager.instance.ActivateBlur();
        }

        if (item != null)
        {
            Time.timeScale = 0;
            GameManager.instance.blockPlayerMovement = true;
            itemPanel.SetActive(true);
        
            itemPanel.GetComponentInChildren<RawImage>().texture = item.itemImage;
            //itemPanel.GetComponentInChildren<RawImage>().SetNativeSize();
            GameManager.instance.ActivateBlur();
            GameManager.instance.AddItemToInventory(item);
        
            Destroy(gameObject);
        }

        if (requestedItem != null)
        {
            GameManager.instance.blockPlayerMovement = true;
            GameManager.instance.isSelectingItem = true;
            inventoryNavigation.interactedObject = this;
            inventory.OpenInventory();
        }
    }
}
