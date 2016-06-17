using UnityEngine;
using System.Collections;

public class ActivateSongAction : MonoBehaviour {

    GameObject mixer;
    MixerManager mixerManager;

	void Start () {
        mixer = GameObject.Find("Mixer");
        mixerManager = transform.parent.GetComponent<MixerManager>();
	}
	
	void Update () {
	    
	}

    // sets current song title and calls method to set selected song's tracks to active
    void activateSong(string name)
    {
        int index = name.IndexOf("_Button");
        if (index != -1)
        {
            name = name.Remove(index);
        }

        foreach (Transform child in mixer.transform)
        {
            if (child.gameObject.name == name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        mixerManager.activeSong = name;
        SendMessageUpwards("setActiveSong");
    }
}
