using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour {

    MixerManager mixerManager;
    string activeSong;

    Transform currentSongDir;
    Transform currentTrackDir;
    Transform instrument;

    void Start ()
    {
        mixerManager = GameObject.Find("Transport").GetComponent<MixerManager>();
    }

    void MuteTrack(string name)
    {
        activeSong = mixerManager.activeSong;

        currentSongDir = transform.Find(activeSong);
        currentTrackDir = currentSongDir.transform.Find(name);
        instrument = currentTrackDir.transform.GetChild(0);
        Transform muteButton;
        muteButton = currentTrackDir.transform.Find("InstrumentPanel/InterfacePanel/MuteSoloPanel/MuteButton/MuteButtonOutline");

        //muteUI(muteButton, instrument, isMuted);

        // targets the object containing the audio source
        foreach (Transform child in currentTrackDir)
        {           
            if (child.GetComponent<AudioSource>())
            {
                AudioSource clip = child.GetComponent<AudioSource>();
                clip.mute = !clip.mute;
                muteUI(muteButton, instrument, clip.mute);
                //child.GetComponent<AudioSource>().mute = isMuted;
            }
        }
        Debug.Log(name + " has been muted");
    }

    void SoloTrack(string name)
    {
        Debug.Log(name + " has been soloed");
    }

    void muteUI(Transform target, Transform instrument, bool isMuted)
    {
        IsActiveEffect activeEffect;
        IsActiveEffect instrumentEffect;

        activeEffect = target.GetComponent<IsActiveEffect>();
        instrumentEffect = instrument.GetComponent<IsActiveEffect>();
        if (isMuted)
        {
            activeEffect.AddActivePanelState();
            instrumentEffect.RemoveActivePanelState();
        }
        else
        {
            activeEffect.RemoveActivePanelState();
            instrumentEffect.AddActivePanelState();
        }
    }
}