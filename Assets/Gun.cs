using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    [Header("STATS")]
    public GameObject projectile;
    public string shotPoint;
    public float timeBtwShots;
    public float gunKnockback;
    public Sprite gunSprite;
    public int maxAmmo;
    public float reloadTime;
    public float damage;
    [Header("AUDIO")]
    public string[] shotSounds;
    public string clipOutSound, clipInSound;
    public string emptyMagazineSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
