using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vuMeter : MonoBehaviour {

	public AudioSource audioSource;

	public enum ledShape {Quad, Cube, Capsule, Bag, Sphere, Cylinder};
	public ledShape shape;	//Shape of the led segments. Square and cube is less render cost

	public enum numChannels {Mono, Stereo};
	public numChannels monoStereo; //Number on leds columns

	public enum colorSchema {Green_Yellow_Red, Blue_Orange_Red};
	public colorSchema ledColors; //Color schema for leds
	public bool hideWhenOff = false;

	[Range(7,25)]
	public int numSegments=15; //Number of segments int he led

	public float ledPadding=0.2f; //Separation between leds
	public float ledSeparation=0.5f; //Separation left and richt channels
	public float ledDeltaZ=0.1f; //Separation y z axis from meter prefab

	private List<GameObject> ledsChannel1 = new List<GameObject>(); 
	private List<GameObject> ledsChannel2 = new List<GameObject>(); 
	private materialDatabase allMaterials;

	//The percentages are 50% for low color - 30% for medium color 20% for top color
	//You can change here there figures. Ba crefull and get sure all together sum 1.0
	private float segmentsInLow = 0.5f;
	private float segmentsInMedium = 0.3f;
	private float segmentsInHigh = 0.2f;
	private int colorSchemaIndex=0;

	private Vector3 scaleMultiplier = new Vector3(1,1,1); //depends on object selected scale must be resized

	private int numSamples=1024;		//Num samples to precess MUST be a power of 2
	private float[] frames0;
	private float[] frames1;
	private float refValue = 0.1f; // RMS value for 0 dB
	private float minDb = -10f;
	private float maxDb = 18f;

	void Start() {
		allMaterials = gameObject.GetComponent<materialDatabase> ();

		//Allocates udio data
		frames0 = new float[numSamples];
		frames1 = new float[numSamples];

		//Hide vuMeter base quad
		gameObject.GetComponent<Renderer>().enabled = false;
		setUpSegments ();

        // get and assign AudioSource from parent-sibling gameObject
        foreach (Transform child in transform.parent.parent)
        {
            if (child.name != "VolumePanel")
            {
                audioSource = child.GetComponent<AudioSource>();
            }
        }
    }

    // Update is called once per frame
    void Update () {
		audioSource.GetOutputData(frames0, 0);
		audioSource.GetOutputData(frames1, 1);

		if (monoStereo == numChannels.Mono) {
			for(int i=0; i<frames0.Length; i++)
				frames0[i]+= frames1[i];
			int volume = getVolume(frames0);
			setSegmentVolume(0,volume);
		}
		else {
			int volume0 = getVolume(frames0);
			setSegmentVolume(0,volume0);
			int volume1 = getVolume(frames1);
			setSegmentVolume(1,volume1);
		}
	}

	void setSegmentVolume(int column, int segment) {
		if(column==0) {
			for(int i=0; i<ledsChannel1.Count; i++) {
				if(i<=segment) {
					ledsChannel1[i].GetComponent<Renderer>().enabled=true;
					ledsChannel1[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), true);
				}
				else {
					if(hideWhenOff)
						ledsChannel1[i].GetComponent<Renderer>().enabled=false;
					else {
						ledsChannel1[i].GetComponent<Renderer>().enabled=true;
						ledsChannel1[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), false);
					}
				}
			}
		}
		else {
			for(int i=0; i<ledsChannel2.Count; i++) {
				if(i<=segment) {
					ledsChannel2[i].GetComponent<Renderer>().enabled=true;
					ledsChannel2[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), true);
				}
				else {
					if(hideWhenOff)
						ledsChannel2[i].GetComponent<Renderer>().enabled=false;
					else {
						ledsChannel2[i].GetComponent<Renderer>().enabled=true;
						ledsChannel2[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), false);
					}
				}
			}
		}
	}

	int getVolume(float [] data) {
		float sum = 0f;
		for(int i=0; i<data.Length; i++)
			sum += (data[i]*data[i])/2f;
		float rmsValue = Mathf.Sqrt(sum/data.Length);
		float volume = 20f*Mathf.Log10(rmsValue/refValue);

		//Limit volume to minDb <-> maxDb
		if(volume<minDb)
			volume=minDb;
		if(volume>maxDb)
			volume=maxDb;
		volume -= minDb;
		float segment = ((float) numSegments * volume)/(maxDb - minDb);
		int volSegment= (int)Mathf.Round (segment) - 1;
		return volSegment;
	}

	//Creates instances for all segments
	void setUpSegments () {
		//Removel all previous segments if exists
		destroyPreviousMeter ();

		//Determine current color schema
		if(ledColors==colorSchema.Green_Yellow_Red)
			colorSchemaIndex=0;
		else
			colorSchemaIndex=1;

		//Calculate segment size for current conf
		Vector3 size=segmentSize();

		//Instantiate all segments
		for (int i=0; i<numSegments; i++) {
			Vector3 position;
			position = segmentPosition(i,0);
			int color=segmentColor(i);

			ledsChannel1.Add(instantiateSegment(position, size, color));
			if(monoStereo==numChannels.Stereo) {
				position = segmentPosition(i,1);
				ledsChannel2.Add(instantiateSegment(position, size, color));
			}

		}
	}

	GameObject instantiateSegment(Vector3 position, Vector3 size, int color) {
		GameObject segment=null;
		//Normalize position to center of the object
		position.x+=size.x/2f;
		position.y+=size.y/2f;
		position.y-=transform.localScale.y/2f;
		position.x /= transform.localScale.x;
		position.y /= transform.localScale.y;

		switch (shape) {
		case ledShape.Quad:
			segment=GameObject.CreatePrimitive(PrimitiveType.Quad);
			scaleMultiplier=new Vector3(0.1f,0.1f,0.1f);
			break;
		case ledShape.Cube:
			segment=GameObject.CreatePrimitive(PrimitiveType.Cube);
			scaleMultiplier=new Vector3(0.05f, 0.05f, 0.01f);
			break;
		case ledShape.Capsule:
			segment=GameObject.CreatePrimitive(PrimitiveType.Capsule);
			scaleMultiplier=new Vector3(1,0.5f,1);
			break;
		case ledShape.Bag:
			segment=GameObject.CreatePrimitive(PrimitiveType.Capsule);
			scaleMultiplier=new Vector3(1,1,1);
			break;
		case ledShape.Sphere:
			segment=GameObject.CreatePrimitive(PrimitiveType.Sphere);
			scaleMultiplier=new Vector3(1,1,1);
			break;
		case ledShape.Cylinder:
			segment=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			scaleMultiplier=new Vector3(1,0.5f,1);
			break;
		}
		segment.transform.localScale = size;
		segment.transform.localScale = Vector3.Scale (segment.transform.localScale, scaleMultiplier);
		segment.GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, color, false);
		segment.transform.position = transform.TransformPoint(position);
		segment.transform.rotation *= transform.rotation;
		segment.transform.parent = gameObject.transform;
		return segment;
	}

	int segmentColor (int segmentNumber) {
		float relPos = (float)segmentNumber / (float)numSegments;
		if(relPos<segmentsInLow)
			return 0;
		if(relPos<segmentsInMedium+segmentsInLow)
			return 1;
		return 2;
	}

	Vector3 segmentSize () {
		Vector3 size=new Vector3();

		if (monoStereo == numChannels.Mono)
			size.x = transform.localScale.x-(2f*ledPadding);
		else
			size.x = ((transform.localScale.x-ledSeparation)/2f) - ledPadding;
	
		size.y=(transform.localScale.y / (float)numSegments) - ledPadding;

		size.z = size.y;
		return size;
	}

	//Returns the left-down position of the led
	Vector3 segmentPosition(int segmentNumber, int channel) {
		Vector3 position=new Vector3();

		if (monoStereo == numChannels.Mono || channel==0)
			position.x=ledPadding-(transform.localScale.x/2f);
		else
			position.x = ledSeparation/2f;;
			
		position.y = ((float)segmentNumber * transform.localScale.y / (float)numSegments);
		position.z = ledDeltaZ;
		return position;
	}

	void destroyPreviousMeter () {
		for(int i=0; i<ledsChannel1.Count; i++)
			Destroy(ledsChannel1[i]);
		ledsChannel1.Clear ();

		for(int i=0; i<ledsChannel2.Count; i++)
			Destroy(ledsChannel2[i]);
		ledsChannel2.Clear ();
	}
}
