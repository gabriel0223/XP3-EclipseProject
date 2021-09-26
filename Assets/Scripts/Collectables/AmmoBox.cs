using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AmmoBox : MonoBehaviour
{
    private PlayerInteraction playerInteraction;
    [HideInInspector] public string id;
    public Gun gun;
    public GunScript gunScript;
    public GameObject ammoBoxUI;
    public SpriteRenderer weaponFlat;
    public int ammo;
    private bool isClose;

    private void Awake()
    {
        id = SceneManager.GetActiveScene().name + gameObject.name + Math.Sqrt(transform.position.magnitude) + transform.rotation;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.saveFile.KeyExists(id))
        {
            Destroy(gameObject);
        }
        
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        weaponFlat = ammoBoxUI.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerInteraction.keyPressIcon.SetActive(false);
                Invoke(nameof(TakeAmmo), 0.1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = true;
            ammoBoxUI.GetComponentInChildren<TextMeshProUGUI>().SetText(ammo.ToString());
            ammoBoxUI.SetActive(true);
            weaponFlat.sprite = gun.flatImage;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
            ammoBoxUI.SetActive(false);
        }
    }

    public void TakeAmmo()
    {
        foreach (var g in GameManager.guns)
        {
            if (gun.name.Equals(g.name))
            {
                AudioManager.instance.PlayRandomBetweenSounds(new []{"AmmoPickup1", "AmmoPickup2", "AmmoPickup3"});
                g.ammoRemainder += ammo;
                gunScript.UpdateAmmo();
                ammoBoxUI.SetActive(false);
                LevelManager.saveFile.Save(id, "taken");
                LevelManager.saveFile.Sync();
                Destroy(gameObject);
            }
        }
    }
}
