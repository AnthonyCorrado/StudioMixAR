using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour {

    MixerManager mixerManager;
    string activeSong;

    Transform currentSongDir;
    Transform currentTrackDir;
    Transform instrument;

    List<GameObject> mixingBoard;
    public List<AudioSource> soloedTracks;

    void Start ()
    {
        mixerManager = GameObject.Find("Transport").GetComponent<MixerManager>();
        mixingBoard = new List<GameObject>();
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
               // clip.mute = !clip.mute;
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
        updateMixerStatus(instrument, "solo");
    }

    void updateMixerStatus(Transform instrument, string action)
    {
        Debug.Log("update mixer called!!!");
        GameObject[] taggedObjects;
        taggedObjects = GameObject.FindGameObjectsWithTag("Track");

        foreach(GameObject obj in taggedObjects)
        {
            mixingBoard.Add(obj);
        }

        for (int z = 0; z < mixingBoard.Count; z++)
        {
            AudioSource thisTrack = mixingBoard[z].GetComponent<AudioSource>();
            if (action == "mute")
            {
                if (mixingBoard[z].name == instrument.name)
                {
                    thisTrack.mute = !thisTrack.mute;
                }
            }
            else if (action == "solo")
            {
                if (!soloedTracks.Contains(thisTrack) && thisTrack.name == instrument.name)
                {
                    soloedTracks.Add(thisTrack);
                    Debug.Log(thisTrack.name + " has been added");
                }
                else if (soloedTracks.Contains(thisTrack) && thisTrack.name == instrument.name)
                {
                    soloedTracks.Remove(thisTrack);
                    Debug.Log(thisTrack.name + " has been removed");
                }

                for (int m = 0; m < mixingBoard.Count; m++)
                {
                    if (soloedTracks.Contains(thisTrack))
                    {
                        thisTrack.mute = false;
                    }
                    else
                    {
                        thisTrack.mute = true;
                    }

                }
                //for (int s = 0; s < soloedTracks.Count; s++)
                //{
                //    if (!soloedTracks.
                //}
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
        }
    }
}