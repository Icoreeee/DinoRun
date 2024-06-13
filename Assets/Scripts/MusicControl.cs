using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SocialPlatforms.Impl;

public class MusicControl : MonoBehaviour
{
    public StudioEventEmitter Music;
    public bool isPlaying;

    private void MuteControl()
    {
        if (Input.GetKeyDown(KeyCode.M) && isPlaying)
        {
            Music.Stop();
            isPlaying = !isPlaying;
        }
        else if (Input.GetKeyDown(KeyCode.M) && !isPlaying)
        {
            Music.Play();
            isPlaying = !isPlaying;
        }
    }

    public void MusicSwitch(int switchVar)
    {
        Music.SetParameter("Switch Control", switchVar);
    }

    public void Drum1Control(int controlVar)
    {
        Music.SetParameter("Drums1 Control", controlVar);
    }

    public IEnumerator DrumDelay()
    {
        yield return new WaitForSeconds(60);
        
        Drum1Control(1);
        Debug.Log("Drums1 Control set to 1");
    }

    public IEnumerator MusicJump()
    {
        MusicSwitch(1);
        
        yield return new WaitForSeconds(90);
        
        MusicSwitch(2);

        yield return new WaitForSeconds(120);
        
        MusicSwitch(3);

        yield return new WaitForSeconds(180);
        
        MusicSwitch(4);

        yield return new WaitForSeconds(240);
        
        MusicSwitch(5);
    }

    void Start()
    {
        MusicSwitch(0);
        Music.Play();
        StartCoroutine(DrumDelay());
        isPlaying = true;
    }
}
