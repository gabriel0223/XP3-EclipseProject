using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    static Animator _fadePanel;
    public static ZNavigation.ZDoor currentZDoor;

    static Vector3 playerPosition;
    private static Quaternion playerRotation;
    private static Vector2 playerVelocity;
    [HideInInspector] public GunScript gunScript;
    private static int ammo;
    private static int ammoRemainder;
    private static int currentGunIndex;
    private static int scenesLoaded = 0;
    public static ES3File saveFile;
    public Slot[] inventory;

    private void Awake()
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

        if (saveFile == null)
        {
            saveFile = new ES3File("SaveFile.save");
            saveFile.Clear();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            if (SceneManager.GetActiveScene().name.Equals("Intro")) return; 
        
            _fadePanel = GameObject.FindGameObjectWithTag("Canvas").transform.Find("FadePanel").GetComponent<Animator>();

            if (SceneManager.GetActiveScene().buildIndex < 2) return;
        
            inventory = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Inventory").GetComponentsInChildren<Slot>(true);
            gunScript = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPivotPoint")
                .GetComponent<GunScript>();
        
            LoadData();
            scenesLoaded++;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("É O LEVEL MANAGER QUE TÁ ZOADO NO CÓDIGO ONSCENELOADED");
            Debug.Log(e.Message);
            //Debug.Log(e.Source);
            Debug.Log(e.TargetSite);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(saveFile.GetKeys()[0]);
    }

    public void StartGame()
    {
        StartCoroutine(GoToScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator EnterDoor(ZNavigation.ZDoor door)
    {
        _fadePanel.SetTrigger("Fade");
        //yield return new WaitForSeconds(_fadePanel.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1);
        GetData();
        
        currentZDoor = new ZNavigation.ZDoor
        {
            doorID = door.doorID,
            nextDoorID = door.nextDoorID,
            nextScene = door.nextScene
        };
        
        Debug.Log(currentZDoor.nextDoorID);
        SceneManager.LoadScene(currentZDoor.nextScene);
    }
    
    public IEnumerator GoToScene(int sceneName)
    {
        _fadePanel.SetTrigger("Fade");
        yield return new WaitForSeconds(_fadePanel.GetCurrentAnimatorStateInfo(0).length);
        GetData();
        SceneManager.LoadScene(sceneName);
    }

    private void GetData()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            playerRotation = player.transform.rotation;
            currentGunIndex = gunScript.gunIndex;
        }

        saveFile.Save("Inventory", inventory);
    }
    
    private void LoadData()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Camera cam = Camera.main.transform.parent.GetComponent<Camera>();

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (currentZDoor != null)
            {
                var zDoorsInScene = GameObject.FindGameObjectsWithTag("ZDoor");
                Debug.Log($"The current door ID is: {currentZDoor.doorID}");
                foreach (var door in zDoorsInScene)
                {
                    if (door.GetComponent<ZNavigation>() == null)
                    {
                        Debug.Log("NÃO TEM ZNAVIGATION");
                        continue;
                    }
                    else
                    {
                        var nextDoorID = door.GetComponent<ZNavigation>().zDoor.doorID;
                        Debug.Log("TEM ZNAVIGATION");
                        Debug.Log(nextDoorID);
                    }
                    
                    if (currentZDoor.nextDoorID.Equals(door.GetComponent<ZNavigation>().zDoor.doorID)) //se a porta final tiver o ID procurado pela porta inicial
                    {
                        Debug.Log("TELEPORTED TO THE DOOR");
                        player.transform.position = door.transform.position;
                        //cam.transform.position = door.transform.position;
                        break;
                    }
                }
                // currentZDoor = null;
            }
            else
            {
                Debug.Log("current ZDoor is null /:");
            }

            player.transform.rotation = playerRotation;
            cam.transform.rotation = playerRotation;
            PersistInventory();
            PersistAmmo();
            PersistDoors();
        }
    }

    void PersistInventory()
    {

        //var inventory = ES3.Load<Slot[]>("Inventory");
        if (saveFile.KeyExists("Inventory"))
        {
            inventory = null;

            while (inventory == null)
            {
                inventory = saveFile.Load<Slot[]>("Inventory");
            }
            
            GameManager.instance.ReloadInventory(inventory); 
        }
        else
        {
            Debug.Log("não achei save nenhum D:");
        }
    }

    void PersistDoors()
    {
        foreach (var door in FindObjectsOfType<Door>())
        {
            if (saveFile.KeyExists(door.id))
            {
                door.locked = saveFile.Load<bool>(door.id); 
            }
        }
    }

    void PersistAmmo()
    {
        gunScript.gunIndex = currentGunIndex;
    }
}
