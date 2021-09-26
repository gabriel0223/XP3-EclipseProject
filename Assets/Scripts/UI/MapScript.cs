using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject mapPanel;

    public GameObject noMapPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapPanel == null) //se não tiver o mapa da área, mostre o panel de mapa indisponível
            {
                if (noMapPanel.activeSelf)
                {
                    noMapPanel.SetActive(false);
                    GameManager.instance.blockPlayerMovement = false;
                }
                else
                {
                    if (!GameManager.instance.blockPlayerMovement)
                    {
                        GameManager.instance.blockPlayerMovement = true;
                        noMapPanel.SetActive(true); 
                    }
                }
            }
            else
            {
                if (mapPanel.activeSelf)
                {
                    mapPanel.SetActive(false);
                    GameManager.instance.blockPlayerMovement = false;
                }
                else
                {
                    if (!GameManager.instance.blockPlayerMovement)
                    {
                        GameManager.instance.blockPlayerMovement = true;
                        mapPanel.SetActive(true); 
                    }
                } 
            }
        }  
        
        
    }
}
