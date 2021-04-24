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
    private static int scenesLoaded = 0; 

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
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
            ammo = gunScript.ammo;
            ammoRemainder = gunScript.ammoRemainder;
        }
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
        }
    }

    void PersistInventory()
    {
        GameManager.instance.inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        GameManager.instance.ReloadInventory();
    }

    void PersistAmmo()
    {
        if (scenesLoaded == 0) return;
        
        gunScript.ammo = ammo;
        gunScript.ammoRemainder = ammoRemainder;
    }
}
