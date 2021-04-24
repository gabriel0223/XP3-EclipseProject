using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ZDoor", menuName = "ZDoor")]
public class ZDoor : ScriptableObject
{
    public string doorID;
    public string nextScene;
    public ZDoor nextDoor;
    public int orientation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
