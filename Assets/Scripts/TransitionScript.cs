using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    private Image transitionImage;
    
    private void Awake()
    {
        transitionImage = GetComponent<Image>();
        transitionImage.color = new Color(0,0,0, 1);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
