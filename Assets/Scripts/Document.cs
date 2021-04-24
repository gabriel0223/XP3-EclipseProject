using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Document", menuName = "Document")]
public class Document : ScriptableObject
{
    public Texture documentImage;
    [TextArea(20,10)]
    public string documentText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
