using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private SpriteRenderer sr;
    public float blinkingRate = 20f;
    public float brightnessScale = 0.5f;
    public LightSprite2D healthBarLight;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameManager.playerHealth);

        ChangeBarColor();

        // if (GameManager.playerHealth >= 75)
        // {
        //     blinkingRate = 1.5f;
        // }
        // else if (GameManager.playerHealth >= 25 && GameManager.playerHealth <= 75)
        // {
        //     blinkingRate = 4f;
        // }
        // else
        // {
        //     blinkingRate = 12.5f;
        // }

        blinkingRate = 200 / GameManager.playerHealth;

    }

    void ChangeBarColor()
    {
        sr.color = Color.HSVToRGB(GameManager.playerHealth / 312, 0.7f, 0.85f + Mathf.Sin(Time.time * blinkingRate) * brightnessScale);
        var lightColor = Color.HSVToRGB(GameManager.playerHealth / 312, 0.7f, 0.1f + Mathf.Sin(Time.time * blinkingRate) * brightnessScale);
        
        
        healthBarLight.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.75f);
    }
}
