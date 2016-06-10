using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsActiveEffect : MonoBehaviour {

    Material defaultMaterial;
    Material activeMaterial;
    Material inactiveMaterial;
    Renderer rend;
    List<Renderer> defaultMaterials = new List<Renderer>();
    List<Renderer> rends = new List<Renderer>();
    List<Renderer> rendChildren = new List<Renderer>();

	// Use this for initialization
	void Start () {

        // if instrument has only one element
        if (GetComponent<Renderer>())
        {
            rends.Add(GetComponent<Renderer>());
        }

        // if instrument is multi element - Example: violin with a bow. Or GO has children with renderer
        if (!GetComponent<Renderer>() && (gameObject.tag == "Track" || gameObject.name == "VolumePanel"))
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    rends.Add(child.GetComponent<Renderer>());
                }
            }
        }
        
        // stores initial material. Multi element instruments still contain only one shared material
        for (int i = 0; i < rends.Count; i++)
        {
            defaultMaterial = rends[0].material;
        }

        // stores material to apply for effects 
        activeMaterial = Resources.Load<Material>("Materials/Active");
        inactiveMaterial = Resources.Load<Material>("Materials/InactiveInstrument");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddActivePanelState()
    {
        // applies renderer to the proper hierarchy 
        foreach (Renderer rend in rends)
        {
            if (rend.GetComponent<Renderer>() && (rend.tag == "Track" || rend.transform.parent.tag == "Track" || rend.transform.parent.name == "VolumePanel"))
            {
                rend.material = defaultMaterial;
            }
            else
            {
                rend.material = activeMaterial;
            }
        }
    }

    public void RemoveActivePanelState()
    {
        // applies renderer to the proper hierarchy 
        foreach (Renderer rend in rends)
        {
            if (rend.GetComponent<Renderer>() && (rend.tag == "Track" || rend.transform.parent.tag == "Track"))
            {
                rend.material = inactiveMaterial;
            }
            else
            {
                rend.material = defaultMaterial;
            }
        }
    }
}
