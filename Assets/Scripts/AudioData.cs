using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    public bool hasAudio;
    public AudioSource curAudio;
    private ControlData controlData;

    void Start()
    {
        hasAudio = false;
        curAudio = GetComponent<AudioSource>();
        controlData = GetComponent<ControlData>();
        if (curAudio != null)
        {
            hasAudio = true;
        }
        if (hasAudio && controlData.isActive)
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        if (curAudio != null)
        {
            Debug.Log("Is Playing!");
            curAudio.Play();
        }
    }

    public void StopAudio()
    {
        if (curAudio != null)
        {
            Debug.Log("Is Stopping!");
            curAudio.Stop();
        }
    }
}
