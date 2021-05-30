using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixerSnapshot noSong;
    public Transform selectionSquare;
    public GameObject[] menuSections;

    public GameObject firstButton;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        AudioManager.instance.Play("MenuSong");
    }

    // Update is called once per frame
    void Update()
    {
        selectionSquare.position = new Vector3(selectionSquare.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y, 0);
        selectionSquare.localPosition = new Vector3(selectionSquare.localPosition.x,selectionSquare.localPosition.y, 0);
    }

    public void ChangeMenuSection(GameObject newMenuSection)
    {
        foreach (var menuSection in menuSections)
        {
            menuSection.SetActive(false);            
        }
        newMenuSection.SetActive(true);
    }

    public void SetNewButton(GameObject newButton)
    {
        EventSystem.current.SetSelectedGameObject(newButton);
    }

    public void StartGame()
    {
        Debug.Log("CHAMANDO O MÉTODO CERTO");
        noSong.TransitionTo(0.5f);
        LevelManager.instance.StartGame();
    }
}
