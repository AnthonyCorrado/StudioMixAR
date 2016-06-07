using UnityEngine;
using System.Collections;

public class MuteSoloAction : MonoBehaviour {

    string buttonName;

	void Start () {

	}
	
	void Update () {

	}

    void MuteOrSolo()
    {
        buttonName = gameObject.name;
        int index = name.IndexOf("Button");
        if (index != -1)
        {
           buttonName = buttonName.Remove(index);
        }

        
    }
}
