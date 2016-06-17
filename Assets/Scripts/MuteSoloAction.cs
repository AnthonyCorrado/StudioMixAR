using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuteSoloAction : MonoBehaviour {

    public GameObject transport;
    public MixerManager mixer;

    string instrumentName;
    string buttonName;

    void Start () {
        transport = GameObject.Find("Transport");
        mixer = transport.GetComponent<MixerManager>() ;
        // gets the name of the prefab'ed instrument
        instrumentName = transform.parent.parent.parent.name;
	}

    void MuteSolo (string name)
    {
        if (name == "MuteButton")
        {
            mixer.SendMessageUpwards("MuteTrack", instrumentName);
        }
        else if (name == "SoloButton")
        {
            mixer.SendMessageUpwards("SoloTrack", instrumentName);   
        }
    }
}
