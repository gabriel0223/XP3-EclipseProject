using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Gun currentGun;
    public Rigidbody2D playerRb2d;
    private Transform shotPoint;
    public float smoothSpeed;
    
    public SpriteRenderer gunSr;
    private Transform playerTransform;
    private float shotTime;
    private Camera mainCam;
    private bool flipped = false;
    [HideInInspector] public float angle;

    [Header("AMMO VARIABLES")]
    public Image[] bulletImages;
    
    public int ammo;
    [HideInInspector] public int ammoRemainder;
    private bool reloading;
    private Color32 darkBulletColor;

    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        playerRb2d = GetComponentInParent<Rigidbody2D>();
        playerTransform = transform.parent;
        mainCam = Camera.main;
        gunSr.sprite = currentGun.gunSprite;
        shotPoint = transform.Find(currentGun.shotPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        darkBulletColor = new Color32(80,80,80,255);
        UpdateAmmo();
        DarkenUsedAmmo();
        LightenUnusedAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.blockPlayerMovement)
        {
            if (transform.localRotation.eulerAngles.z.Between(270, 360) ||
                transform.localRotation.eulerAngles.z.Between(0, 90))
            {
                if (flipped)
                {
                    Flip(1);
                }
            }
            else
            {
                if (!flipped)
                {
                    Flip(-1);
                }
            }

            var dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

            if (Input.GetMouseButton(0) && !reloading)
            {
                if (Time.time >= shotTime)
                {
                    if (ammo > 0)
                    {
                        Shoot();
                    }
                    else
                    {
                        if (!AudioManager.instance.IfSoundIsPlaying(currentGun.emptyMagazineSound))
                            AudioManager.instance.Play(currentGun.emptyMagazineSound);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (ammoRemainder > 0 && ammo != currentGun.maxAmmo)
                {
                    //do reload animation
                    StartCoroutine(Reload(currentGun.reloadTime));
                }
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                Debug.Log("go up in weapon selection");
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                Debug.Log("go down in weapon selection");
            }

        }
    }
    
    private void Flip(int side)
    {
        //unparent player's children
        List<Transform> childrenRemoved = new List<Transform>();
        
        foreach (Transform child in playerTransform.GetComponentsInChildren(typeof(Transform), true))
        {
            if (child.parent == playerTransform)
            {
                child.parent = null;
                childrenRemoved.Add(child);
            }
        }
        
        playerTransform.localScale = new Vector3(side, 1, 1);
        
        foreach (Transform child in childrenRemoved) child.parent = playerTransform;
        childrenRemoved.Clear();
        transform.localScale = new Vector3(side, side, 1);

        flipped = side == -1;
    }

    public void Shoot()
    {
        AudioManager.instance.PlayRandomBetweenSounds(currentGun.shotSounds);
        Instantiate(currentGun.projectile, shotPoint.position, transform.rotation);
        ammo--;
        UpdateAmmo();
        DarkenUsedAmmo();
        AddForceAtAngle(-currentGun.gunKnockback, angle);
        shotTime = Time.time + currentGun.timeBtwShots;
    }
    
    public void AddForceAtAngle(float force, float angle)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;
 
        playerRb2d.AddForce(new Vector2(xcomponent, ycomponent), ForceMode2D.Impulse);
    }

    public void UpdateAmmo()
    {
        if (ammo > currentGun.maxAmmo)
        {
            ammoRemainder += (ammo - currentGun.maxAmmo);
            ammo = currentGun.maxAmmo;
        }
        
        ammoText.SetText(ammo + " | " + ammoRemainder);
    }

    private void DarkenUsedAmmo()
    {
        int count = currentGun.maxAmmo - ammo;

        foreach (var bullet in bulletImages)
        {
            bullet.color = Color.white;
        }
        
        foreach (var bullet in bulletImages)
        {
            if (bullet.color.Equals(Color.white))
            {
                bullet.color = darkBulletColor;
                count--;
            }

            if (count == 0)
                break;
        }
    }

    private void LightenUnusedAmmo()
    {
        foreach (var bullet in bulletImages)
        {
            bullet.color = darkBulletColor;
        }
        
        int count = ammo;
        
        if (count == 0)
            return;
        
        foreach (var bullet in bulletImages.Reverse())
        {
            if (bullet.color.Equals(darkBulletColor))
            {
                bullet.color = Color.white;
                count--;
            }
            
            if (count == 0)
                break;
        }
    }

    IEnumerator Reload(float reloadTime)
    {
        reloading = true;
        AudioManager.instance.PlaySoundAfterAnother(currentGun.clipOutSound, currentGun.clipInSound, reloadTime);
        yield return new WaitForSeconds(currentGun.reloadTime + 0.25f);
        
        int ammoToBeReloaed = ammoRemainder;
        ammoRemainder -= ammoToBeReloaed;
        ammo += ammoToBeReloaed;
        UpdateAmmo();
        LightenUnusedAmmo();
        
        reloading = false;
    }

    public void ChangeWeapon()
    {
        
    }
}
