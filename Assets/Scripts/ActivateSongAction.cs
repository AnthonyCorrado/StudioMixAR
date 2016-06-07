using UnityEngine;
using System.Collections;

public class ActivateSongAction : MonoBehaviour {

    GameObject mixer;

	void Start () {
        mixer = GameObject.Find("Mixer");
	}
	
	void Update () {
	    
	}

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
    }
}
