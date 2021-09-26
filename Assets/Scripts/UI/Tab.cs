using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour, ISubmitHandler, IPointerClickHandler
{
    public TerminalTabs terminalTabs;
    // Start is called before the first frame update
    void Start()
    {
        terminalTabs = FindObjectOfType<TerminalTabs>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SetSelectedTab();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedTab();
    }

    private void SetSelectedTab()
    {
        // terminalTabs.currentlySelectedTab = gameObject.GetComponent<Button>();
    }
}
