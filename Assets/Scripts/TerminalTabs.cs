using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalTabs : MonoBehaviour
{
    public GameObject textTab;

    public GameObject selectedTab;
    public RectTransform feedbackRect;
    public float moveTabSpeed;

    private int tabSelections = 0;

    // Start is called before the first frame update
    void Start()
    {
        selectedTab = textTab;
        EventSystem.current.SetSelectedGameObject(selectedTab);
        SelectTab(textTab);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTabFeedback();

        selectedTab.GetComponent<Image>().color = new Color32(19,19,19, 255);
    }

    public void SelectTab(GameObject tab)
    {
        if (tabSelections > 1)
            AudioManager.instance.Play("TerminalAba");
        
        
        selectedTab.GetComponent<TerminalButton>().tabContent.SetActive(false);
        
        tab.GetComponent<TerminalButton>().tabContent.SetActive(true);
        selectedTab = EventSystem.current.currentSelectedGameObject;
        tabSelections++;
    }

    public void MoveTabFeedback()
    {
        Vector3 tabPos = new Vector3(selectedTab.GetComponent<RectTransform>().position.x, selectedTab.GetComponent<RectTransform>().position.y, 1);
        feedbackRect.position = Vector2.Lerp(feedbackRect.position, tabPos, moveTabSpeed * Time.deltaTime);
        feedbackRect.localPosition = new Vector3(feedbackRect.localPosition.x,feedbackRect.localPosition.y, 0);
    }
}
