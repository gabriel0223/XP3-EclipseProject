using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalFiles : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;
    public Text outputText;
    public Image outputImage;
    public GameObject outputAudio;
    public TerminalButton selectedFile;

    public string lastAudioPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            CloseTerminal();
        }
    }

    public void SelectFile(TerminalButton file)
    {
        AudioManager.instance.Play("TerminalPassagem");
        selectedFile = file;
        selectedFile.feedbackRect.SetActive(true);
        selectedFile.fileText.color = new Color32(19,19,19,255);

        if (file.GetComponent<TextDoc>() != null)
        {
            DisplayText();
            return;
        }

        if (file.GetComponent<ImageDoc>() != null)
        {
            DisplayImage();
            return;
        }

        if (file.GetComponent<AudioDoc>())
        {
            PlayAudio();
        }
        
    }
    public void DisplayText()
    {
        LeaveOneDoc(outputText.gameObject);
        scrollRect.content = outputText.GetComponent<RectTransform>();
        ResetScrollbar();
        outputText.text = selectedFile.GetComponent<TextDoc>().textToDisplay;
    }
    
    public void DisplayImage()
    {
        LeaveOneDoc(outputImage.gameObject);
        scrollRect.content = outputImage.GetComponent<RectTransform>();
        ResetScrollbar();
        outputImage.sprite = selectedFile.GetComponent<ImageDoc>().imageToDisplay;
        outputImage.SetNativeSize();
    }

    public void PlayAudio()
    {
        LeaveOneDoc(outputAudio.gameObject);
        ActivateSoundWave();
        CancelInvoke();
        
        if (lastAudioPlayed != "")
        {
            if (AudioManager.instance.IfSoundIsPlaying(lastAudioPlayed))
            {
                AudioManager.instance.Stop(lastAudioPlayed);
            }
        }

        lastAudioPlayed = selectedFile.GetComponent<AudioDoc>().soundName;
        AudioManager.instance.Play(lastAudioPlayed);
        Invoke(nameof(MuteSoundWave), AudioManager.instance.GetAudioLength(lastAudioPlayed));
    }

    private void ActivateSoundWave()
    {
        AudioManager.instance.SetSoundVolume(AudioManager.instance.musicPlaying, 0.1f);
        outputAudio.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }
    private void MuteSoundWave()
    {
        AudioManager.instance.SetSoundVolume(AudioManager.instance.musicPlaying, 0.5f);
        outputAudio.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    private void LeaveOneDoc(GameObject remainingDoc)
    {
        outputImage.gameObject.SetActive(false);
        outputText.gameObject.SetActive(false);
        outputAudio.gameObject.SetActive(false);
        
        remainingDoc.SetActive(true);
    }

    private void ResetScrollbar()
    {
        scrollbar = scrollRect.verticalScrollbar;
        scrollbar.value = 1;
    }

    private void CloseTerminal()
    {
        AudioManager.instance.Stop(lastAudioPlayed);
        AudioManager.instance.Play("TerminalFechar");
        GameManager.instance.blockPlayerMovement = false;
        Destroy(transform.parent.gameObject);
    }
}
