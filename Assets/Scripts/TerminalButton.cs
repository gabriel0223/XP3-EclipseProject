using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public Button button;
    public Image line;
    public TerminalTabs terminalTabs;
    public GameObject tabContent;
    public GameObject feedbackRect;
    public TextMeshProUGUI fileText;
    public TerminalFiles terminalFiles;

    private void Awake()
    {
        fileText = GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
        terminalTabs = FindObjectOfType<TerminalTabs>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (gameObject.CompareTag("Tab"))
        {
            terminalTabs.selectedTab.GetComponent<Image>().color = Color.white;
            terminalTabs.SelectTab(gameObject);
        }
        else
        {
            if (terminalFiles.selectedFile != null)
            {
                terminalFiles.selectedFile.feedbackRect.SetActive(false);
                terminalFiles.selectedFile.fileText.color = new Color32(212,79,78,255);
            }

            terminalFiles.SelectFile(this);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
    }
}
