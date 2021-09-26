using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class GunScript : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    public int gunIndex;
    public Rigidbody2D playerRb2d;
    private Transform shotPoint;
    public float smoothSpeed;
    private float switchGunTimer;
    
    public SpriteRenderer gunSr;
    private Transform playerTransform;
    private float shotTime;
    private Camera mainCam;
    private bool flipped = false;
    [HideInInspector] public float angle;

    [Header("UI VARIABLES")] 
    private GameObject canvas;
    public GameObject flatBullets;
    public Image weaponImage;
    
    private bool reloading;
    private Color32 darkBulletColor;
    public TextMeshProUGUI ammoText;
    
    public List<Gun> selectedGuns = new List<Gun>();

    private void Awake()
    {
        playerRb2d = GetComponentInParent<Rigidbody2D>();
        playerTransform = transform.parent;
        mainCam = Camera.main;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        darkBulletColor = new Color32(80,80,80,255);
        
        shotPoint = transform.Find(currentGun.shotPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedGuns = GameManager.guns;
        currentGun = selectedGuns[gunIndex];
        gunSr.sprite = currentGun.gunSprite;
        UpdateUI();
        UpdateAmmo();
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
                    if (currentGun.ammo > 0)
                    {
                        Shoot();
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) && !reloading)
            {
                if (currentGun.ammo > 0) return;
                AudioManager.instance.Play(currentGun.emptyMagazineSound);
            }

            if (Input.GetKeyDown(KeyCode.R) && !reloading)
            {
                if (currentGun.ammoRemainder > 0 && currentGun.ammo != currentGun.maxAmmo)
                {
                    //do reload animation
                    StartCoroutine(Reload(currentGun.reloadTime));
                }
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                ChangeWeapon();
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
                if (child.name.Equals("HealthBar")) continue;
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
        //AudioManager.instance.PlayRandomBetweenSounds(currentGun.shotSounds);
        AudioManager.instance.PlayOneShotRandomBetweenSounds(currentGun.shotSounds);
        //ProCamera2DShake.Instance.Shake("PistolCamShake");
        Instantiate(currentGun.projectile, shotPoint.position, transform.rotation);
        currentGun.ammo--;
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
        if (currentGun.ammo > currentGun.maxAmmo)
        {
            currentGun.ammoRemainder += (currentGun.ammo - currentGun.maxAmmo);
            currentGun.ammo = currentGun.maxAmmo;
        }
        
        ammoText.SetText(currentGun.ammo + " | " + currentGun.ammoRemainder);
        
        DarkenUsedAmmo();
        LightenUnusedAmmo();
    }

    public void UpdateUI()
    {
        weaponImage.sprite = currentGun.flatImage;
        flatBullets.GetComponent<HorizontalLayoutGroup>().spacing = currentGun.spacingBtwFlats;
        
        foreach (Transform child in flatBullets.transform) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentGun.maxAmmo; i++)
        {
            Instantiate(currentGun.flatProjectile, flatBullets.transform);
        }
    }

    private void DarkenUsedAmmo()
    {
        int count = currentGun.maxAmmo - currentGun.ammo;

        Image[] bulletImages = flatBullets.GetComponentsInChildren<Image>();
        
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
        Image[] bulletImages = flatBullets.GetComponentsInChildren<Image>();
        
        foreach (var bullet in bulletImages)
        {
            bullet.color = darkBulletColor;
        }
        
        int count = currentGun.ammo;
        
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
        
        int ammoToBeReloaed = currentGun.ammoRemainder;
        currentGun.ammoRemainder -= ammoToBeReloaed;
        currentGun.ammo += ammoToBeReloaed;
        UpdateAmmo();

        reloading = false;
    }

    public void ChangeWeapon()
    {
        if (Time.time < switchGunTimer) return;

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (gunIndex < GameManager.guns.Count - 1)
                gunIndex++;
            else
                gunIndex = 0;

            switchGunTimer = Time.time + 0.2f;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (gunIndex > 0)
                gunIndex--;
            else
                gunIndex = GameManager.guns.Count - 1;

            switchGunTimer = Time.time + 0.2f;
        }
        
        currentGun = GameManager.guns[gunIndex];
        gunSr.sprite = currentGun.gunSprite;
        shotPoint = transform.Find(currentGun.shotPoint);
        AudioManager.instance.Play(currentGun.equipSound);

        if (selectedGuns.Any(gun => currentGun.name.Equals(gun.name)))
        {
            currentGun = selectedGuns[gunIndex]; 
            UpdateUI();
            UpdateAmmo();
        }
    }
}
