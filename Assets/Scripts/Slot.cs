using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public InventoryItem item;

    public RawImage itemIcon;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == EventSystem.current.currentSelectedGameObject)
        {
            anim.SetBool("Selected", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (item != null)
        {
            itemIcon.texture = item.itemIcon;
        }
        else
        {
            itemIcon.texture = null;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        anim.SetBool("Selected", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        anim.SetBool("Selected", true);
        AudioManager.instance.Play("UINavigation");
    }
    
}
