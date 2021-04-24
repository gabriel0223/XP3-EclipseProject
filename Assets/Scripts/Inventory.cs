using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject weaponUI;
    public GameObject selectItemText;

    public bool[] isFull;
    public GameObject[] slots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeSelf)
            {
                CloseInventory();
            }
            else
            {
                if (!GameManager.instance.blockPlayerMovement)
                    OpenInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventory.activeSelf)
            {
                CloseInventory();
            }
        }
    }

    public void OpenInventory()
    {
        if (!GameManager.instance.interactingUI)
        {
            if (GameManager.instance.isSelectingItem)
                selectItemText.SetActive(true);
                
            //Time.timeScale = 0f;
            GameManager.instance.blockPlayerMovement = true;
            GameManager.instance.interactingUI = true;
            AudioManager.instance.Play("OpenInventory");
            weaponUI.SetActive(false);
            inventory.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(inventory.transform.GetChild(0).gameObject);
        }
        
    }
    
    public void CloseInventory()
    {
        //Time.timeScale = 1f;
        foreach (var slot in slots)
        {
            slot.transform.localScale = Vector3.one;
        }
        
        if (GameManager.instance.isSelectingItem)
            selectItemText.SetActive(false);
        
        GameManager.instance.blockPlayerMovement = false;
        GameManager.instance.isSelectingItem = false;
        GameManager.instance.interactingUI = false;
        AudioManager.instance.Play("CloseInventory");
        weaponUI.SetActive(true);
        inventory.SetActive(false);
    }
}