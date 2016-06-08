using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour {

    MixerManager mixerManager;
    string activeSong;

    Transform currentSongDir;
    Transform currentTrackDir;

    bool isMuted;

    void Start ()
    {
        mixerManager = GameObject.Find("Transport").GetComponent<MixerManager>();
        isMuted = false;
    }

    void MuteTrack(string name)
    {
        activeSong = mixerManager.activeSong;
        isMuted = !isMuted;

        currentSongDir = transform.Find(activeSong);
        currentTrackDir = currentSongDir.transform.Find(name);

        // targets the object containing the audio source
        foreach (Transform child in currentTrackDir)
        {
            Debug.Log(child.name);
            if (child.GetComponent<AudioSource>())
            {
                child.GetComponent<AudioSource>().mute = isMuted;
            }
        }
        Debug.Log(name + " has been muted");
    }

    void SoloTrack(string name)
    {
        Debug.Log(name + " has been soloed");
    }
}