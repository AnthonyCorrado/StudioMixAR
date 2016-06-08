using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuteSoloAction : MonoBehaviour {

    public MixerController mixer;
    List<string> details = new List<string>();

    string instrumentName;
    string buttonName;

    void Start () {
        instrumentName = GetComponent<Collider>().transform.root.name;
	}
	
	void Update () {

	}

    void MuteSolo (GameObject actionObject)
    {
        buttonName = actionObject.name;
        int index = buttonName.IndexOf("Button");
        if (index != -1)
        {
            buttonName = buttonName.Remove(index);
        }

        details.Add(instrumentName);
        details.Add(buttonName);

        mixer.SendMessageUpwards("MuteOrSoloTrack", details);
    }
}
