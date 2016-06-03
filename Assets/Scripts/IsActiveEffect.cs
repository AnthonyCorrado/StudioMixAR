using UnityEngine;
using System.Collections;

public class IsActiveEffect : MonoBehaviour {

    Material defaultMaterial;
    Material activeMaterial;
    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        defaultMaterial = rend.material;
        activeMaterial = Resources.Load<Material>("Materials/Active");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddActivePanelState()
    {
        rend.material = activeMaterial;
    }

    void RemoveActivePanelState()
    {
        rend.material = defaultMaterial;
    }
}
