using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DocumentPanel : MonoBehaviour
{
    private PlayerInteraction playerInteractionScript;
    public GameObject readPanel;
    public string documentText;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInteractionScript = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>();

        readPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(documentText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.DisableBlur();
            playerInteractionScript.interacting = false;
            GameManager.instance.blockPlayerMovement = false;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (readPanel.activeSelf)
            {
                readPanel.SetActive(false);
            }
            else
            {
                readPanel.SetActive(true);
            }
        }
    }
}
