using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorial;
    public bool interactive;
    private bool isClose;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.tutorialsLearnt.Contains(tutorial.name))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && interactive && isClose)
        {
            ActivateTutorial();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !interactive)
        {
            ActivateTutorial();
        }
        isClose = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
        }
    }

    public void ActivateTutorial()
    {
        FindObjectOfType<PlayerInteraction>().LearningSomething = true;
        tutorial.SetActive(true);
        GameManager.tutorialsLearnt.Add(tutorial.name);
        Destroy(gameObject);
    }
}
