using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [HideInInspector] public Image barImage;
    public float fillSpeed;
    public float decreaseSpeed;

    private void Awake()
    {
        barImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void FillBar()
    {
        barImage.fillAmount += fillSpeed * Time.deltaTime;
    }
}
