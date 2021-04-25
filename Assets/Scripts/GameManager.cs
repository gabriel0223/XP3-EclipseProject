using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject pauseMenu;
    public bool blockPlayerMovement;
    public bool isSelectingItem;
    public bool interactingUI;
    public Inventory inventory;
    private VolumeProfile volume;

    public static List<InventoryItem> currentItems = new List<InventoryItem>();
    public static Dictionary<InventoryItem, string> itemsCollected = new Dictionary<InventoryItem, string>();
    public static List<string> tutorialsLearnt = new List<string>();
    
    public static List<Gun> guns = new List<Gun>();
    public Gun[] firstGuns;

    //public int PistolAmmo;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (guns.Count != 0) return;
        foreach (var gun in firstGuns) guns.Add(gun);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        volume = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Volume>().profile;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !blockPlayerMovement)
        {
            Pause();
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false)
            {
                inventory.slots[i].GetComponent<Slot>().item = item;
                inventory.slots[i].transform.GetChild(0).gameObject.SetActive(true); //enable icon
                inventory.isFull[i] = true;
                itemsCollected.Add(item, item.name);
                currentItems.Add(item);
                break;
            }
        }  
    }
    
    public void RemoveRequestedItemFromInventory(InventoryItem requestedItem)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].GetComponent<Slot>().item == requestedItem)
            {
                inventory.slots[i].GetComponent<Slot>().item = null;
                inventory.slots[i].transform.GetChild(0).gameObject.SetActive(false); //disable icon
                inventory.isFull[i] = false;
                currentItems.Remove(requestedItem);
                break;
            }
        }
    }

    public void ReloadInventory()
    {
        for (int i = 0; i < currentItems.Count; i++)
        {
            inventory.slots[i].GetComponent<Slot>().item = currentItems[i];
            inventory.slots[i].transform.GetChild(0).gameObject.SetActive(true); //enable icon
            inventory.isFull[i] = true;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        blockPlayerMovement = true;
        pauseMenu.SetActive(true);
        ActivateBlur();
    }
    
    public void ActivateBlur () {
        DepthOfField dof;
        if(!volume.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
        dof.active = true;
    }
    
    public void DisableBlur () {
        DepthOfField dof;
        if(!volume.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
        dof.active = false;
    }
}
