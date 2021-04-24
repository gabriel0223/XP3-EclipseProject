using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    private PlayerInteraction playerInteractionScript;

    // Start is called before the first frame update
    void Start()
    {
        playerInteractionScript = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
        {
            CloseItemPanel();
        }
    }

    private void CloseItemPanel()
    {
        if (GetComponentInChildren<TutorialTrigger>() != null)
        {
            GetComponentInChildren<TutorialTrigger>().ActivateTutorial();    
        }
        
        Time.timeScale = 1;
        GameManager.instance.DisableBlur();
        playerInteractionScript.interacting = false;
        GameManager.instance.blockPlayerMovement = false;
        gameObject.SetActive(false); 
    }
}
