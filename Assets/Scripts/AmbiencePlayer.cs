using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    public string ambienceSound;
    public float minDistance;
    public float maxDistance;
    public bool looped;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayClipAtGameObject(ambienceSound, gameObject, looped, minDistance, maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
