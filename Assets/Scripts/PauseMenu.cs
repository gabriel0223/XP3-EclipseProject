using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Transform selectionSquare;
    public GameObject firstButton;
    
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Update is called once per frame
    void Update()
    {
        selectionSquare.position = new Vector3(selectionSquare.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y, 0);
        selectionSquare.localPosition = new Vector3(selectionSquare.localPosition.x,selectionSquare.localPosition.y, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Continue()
    {
        Time.timeScale = 1;
        GameManager.instance.blockPlayerMovement = false;
        GameManager.instance.DisableBlur();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
