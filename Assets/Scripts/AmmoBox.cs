using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private PlayerInteraction playerInteraction;
    public GunScript gunScript;
    public GameObject ammoBoxUI;
    public int ammo;
    private bool isClose;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
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
        AudioManager.instance.PlayRandomBetweenSounds(new []{"AmmoPickup1", "AmmoPickup2", "AmmoPickup3"});
        gunScript.ammoRemainder += ammo;
        gunScript.UpdateAmmo();
        ammoBoxUI.SetActive(false);
        Destroy(gameObject);
    }
}
