using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffects : MonoBehaviour
{
    public PostProcessVolume volume;

    [HideInInspector] public static DepthOfField depthOfField;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
