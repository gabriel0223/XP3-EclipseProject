using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    static Animator _fadePanel;
    public static ZDoor currentZDoor;

    static Vector3 playerPosition;
    private static Quaternion playerRotation;
    private static Vector2 playerVelocity;
    private GunScript gunScript;
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
        inventory = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Inventory").GetComponentsInChildren<Slot>(true);
        _fadePanel = GameObject.FindGameObjectWithTag("Canvas").transform.Find("FadePanel").GetComponent<Animator>();
        gunScript = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPivotPoint")
            .GetComponent<GunScript>();
        
        LoadData();
        scenesLoaded++;
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

    public IEnumerator EnterDoor(ZDoor door)
    {
        _fadePanel.SetTrigger("Fade");
        yield return new WaitForSeconds(_fadePanel.GetCurrentAnimatorStateInfo(0).length);
        GetData();
        currentZDoor = door;
        Debug.Log(currentZDoor.nextDoor.doorID);
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

                foreach (var door in zDoorsInScene)
                {
                    if (currentZDoor.nextDoor.doorID.Equals(door.GetComponent<ZNavigation>().zDoor.doorID)) //se a porta final tiver o ID procurado pela porta inicial
                    {
                        Debug.Log("LAST STEP");
                        player.transform.position = door.transform.position;
                        cam.transform.position = door.transform.position;
                        break;
                    }
                }
                currentZDoor = null;
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
            inventory = saveFile.Load<Slot[]>("Inventory");
            GameManager.instance.ReloadInventory(inventory); 
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
