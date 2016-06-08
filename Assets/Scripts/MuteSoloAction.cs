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
	
	void Update () {

	}

    void MuteSolo (GameObject actionObject)
    {
        Dictionary<string, string> details = new Dictionary<string, string>();
        buttonName = actionObject.name;
        int index = buttonName.IndexOf("Button");
        if (index != -1)
        {
            buttonName = buttonName.Remove(index);
        }

        details.Add("name", instrumentName);
        details.Add("action", buttonName);

        mixer.SendMessageUpwards("MuteOrSoloTrack", details);
    }
}
