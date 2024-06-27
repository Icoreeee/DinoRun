using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicControl : MonoBehaviour
{
    public StudioEventEmitter Music;
    public EventInstance MusicInst;
    public bool isPlaying;

    private static MusicControl instance = null;

    public static MusicControl Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void MuteControl()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPlaying)
            {
                Music.SetParameter("VolumeControl", 0);
                isPlaying = !isPlaying;
            }
            else if (!isPlaying)
            {
                Music.SetParameter("VolumeControl", 1);
                isPlaying = !isPlaying;
            }
        }
    }

        public void MusicSwitch(int switchVar)
    {
        Music.SetParameter("Switch Control", switchVar);
        Debug.Log($"Part {switchVar + 1} active");
    }

    public void Drum1Control(int controlVar)
    {
        Music.SetParameter("Drums1 Control", controlVar);
        Debug.Log($"Drums1 Control set to {controlVar}");
    }

    public IEnumerator DrumDelay()
    {
        Drum1Control(0);
        Debug.Log($"Delay till drum is set to 1 is 60 sec");
        yield return new WaitForSecondsRealtime(60);
        Drum1Control(1);
    }

    public void MusicJumpStart()
    {
        StartCoroutine(MusicDelayJump());
    }

    public void MusicJumpEnd()
    {
        StopCoroutine(MusicDelayJump());
    }

    public void DrumDelayStart()
    {
        StartCoroutine(MusicDelayJump());
    }

    public IEnumerator MusicDelayJump()
    {
        StopCoroutine(DrumDelay());
        Drum1Control(1);
        MusicSwitch(1);
        Debug.Log("Delay till next part: 30 sec \n It will be activated after the current one is over");
        yield return new WaitForSecondsRealtime(30);
        MusicSwitch(2);
        Debug.Log("Delay till next part: 30 sec \n It will be activated after the current one is over");
        yield return new WaitForSecondsRealtime(30);
        MusicSwitch(3);
        Debug.Log("Delay till next part: 30 sec \n It will be activated after the current one is over");
        yield return new WaitForSecondsRealtime(30);
        MusicSwitch(4);
        Debug.Log("Delay till next part: 30 sec \n It will be activated after the current one is over");
        yield return new WaitForSecondsRealtime(30);
        MusicSwitch(5);
        Debug.Log("Delay till next part: 30 sec \n It will be activated after the current one is over");
    }

    private void Start()
    {
        Music = FindObjectOfType<StudioEventEmitter>();
        Music.Play();
        isPlaying = true;        
        MusicSwitch(0);
        Music.SetParameter("VolumeControl", 1);     
        StartCoroutine(DrumDelay());
    }

    private void Update()
    {
        MuteControl();
    }
}
