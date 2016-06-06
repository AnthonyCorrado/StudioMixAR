using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class materialDatabase : MonoBehaviour {
	
	public List<myMaterial> colorSchemaAOff = new List<myMaterial>();
	public List<myMaterial> colorSchemaAOn = new List<myMaterial>();
	public List<myMaterial> colorSchemaBOff = new List<myMaterial>();
	public List<myMaterial> colorSchemaBOn = new List<myMaterial>();

	public Material getMaterial(int schema, int pos, bool isOn) {
		if (schema == 0 && isOn)
				return colorSchemaAOn [pos].material;
		if (schema == 0 && !isOn)
			return colorSchemaAOff [pos].material;
		if (schema == 1 && isOn)
			return colorSchemaBOn [pos].material;
		if (schema == 1 && !isOn)
			return colorSchemaBOff [pos].material;

		return null;
	}
}
