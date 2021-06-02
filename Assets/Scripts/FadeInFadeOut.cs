using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] tutorialKeys;

    public float timeBeforeFadeBegins;
    public float fadeInDuration;
    public float timeFullAlpha;
    public float fadeOutDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialKeys = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(FadeIn(fadeInDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn(float fadeDuration)
    {
        yield return new WaitForSeconds(timeBeforeFadeBegins);
        
        while (tutorialKeys[0].color.a < 1)
        {
            foreach (var sr in tutorialKeys)
            {
                sr.color += new Color(0,0,0,1 * Time.deltaTime);
            }
            yield return new WaitForSeconds(fadeDuration / 255);
        }
        yield return new WaitForSeconds(timeFullAlpha);
        StartCoroutine(FadeOut(fadeOutDuration));
    }
    
    public IEnumerator FadeOut(float fadeDuration)
    {
        transform.parent.parent.GetComponent<PlayerInteraction>().learningSomething = true;
        
        while (tutorialKeys[0].color.a > 0)
        {
            foreach (var sr in tutorialKeys)
            {
                sr.color -= new Color(0,0,0,1 * Time.deltaTime);
            }
            yield return new WaitForSeconds(fadeDuration / 255);
        }
        
        transform.parent.parent.GetComponent<PlayerInteraction>().learningSomething = false;
        
        if (transform.parent.parent.GetComponent<PlayerInteraction>().canInteract)
        {
            transform.parent.parent.GetComponent<PlayerInteraction>().keyPressIcon.SetActive(true);
        }
        
        Destroy(gameObject);
    }
}
