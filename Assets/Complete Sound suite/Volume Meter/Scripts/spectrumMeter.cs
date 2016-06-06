using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spectrumMeter : MonoBehaviour {

	public AudioSource audioSource;
	
	public enum ledShape {Quad, Cube, Capsule, Bag, Sphere, Cylinder};
	public ledShape shape;	//Shape of the led segments. Square and cube is less render cost
	
	public enum colorSchema {Green_Yellow_Red, Blu_Orange_Red};
	public colorSchema ledColors; //Color schema for leds
	public bool hideWhenOff = false;

	[Range(3,12)]
	public int numColums=7; //Number of columns

	public bool limitHighFreqs=true;

	[Range(7,25)]
	public int numSegments=15; //Number of segments int he led
	
	public float ledPadding=0.2f; //Separation between leds
	public float ledDeltaZ=0.1f; //Separation y z axis from meter prefab
	
	private List<List<GameObject>> ledsColumns; 
	private materialDatabase allMaterials;
	
	//The percentages are 50% for low color - 30% for medium color 20% for top color
	//You can change here there figures. Ba crefull and get sure all together sum 1.0
	private float segmentsInLow = 0.5f;
	private float segmentsInMedium = 0.3f;
	private float segmentsInHigh = 0.2f;
	private int colorSchemaIndex=0;
	
	private Vector3 scaleMultiplier = new Vector3(1,1,1); //depends on object selected scale must be resized
	
	private int numSamples=1024;		//Num samples to precess MUST be a power of 2
	private float[] spectrum;
	private int sampleRate;
	private float maxPower = 0.17f;

	// Use this for initialization
	void Start () {
		allMaterials = gameObject.GetComponent<materialDatabase> ();

		//Instantiats the columns
		ledsColumns = new List<List<GameObject>> ();
		for (int i=0; i<12; i++) {
			List<GameObject> column = new List<GameObject>();
			ledsColumns.Add(column);
		}

		//Allocates audio data
		spectrum = new float[numSamples];
		sampleRate = AudioSettings.outputSampleRate;
		
		//Hide vuMeter base quad
		gameObject.GetComponent<Renderer>().enabled = false;
		setUpSegments ();
	}
	
	// Update is called once per frame
	void Update () {
		audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		int cols = numColums;
		if(limitHighFreqs) {
			if(numColums>11)
				cols=numColums;
			else if(numColums>=10)
				cols=5*numColums/4;
			else if(numColums>=7)
				cols=3*numColums/2;
			else if(numColums>=5)
				cols=5*numColums/3;
			else
				cols=2*numColums;
		}
		for (int i = 0; i < cols; i++) {
			if(i>=numColums)
				continue;
			float lowFreq, hiFreq, freqStep;
			if (i == 0)
				lowFreq = 0f;
			else
				lowFreq = (float)(sampleRate / 2) / (float)Mathf.Pow(2, cols - i);
			hiFreq = (float)(sampleRate / 2) / (float) Mathf.Pow(2, cols - i - 1);
			float cl=calcAvg(lowFreq, hiFreq, spectrum);
			int segment=normalizePower(cl);
			setSegmentPower(i,segment);
		}
	}

	void setSegmentPower(int column, int segment) {
		List <GameObject> col = ledsColumns [column];
		for(int i=0; i<col.Count; i++) {
			if(i<=segment) {
				col[i].GetComponent<Renderer>().enabled=true;
				col[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), true);
			}
			else {
				if(hideWhenOff)
					col[i].GetComponent<Renderer>().enabled=false;
				else {
					col[i].GetComponent<Renderer>().enabled=true;
					col[i].GetComponent<Renderer>().material=allMaterials.getMaterial(colorSchemaIndex, segmentColor(i), false);
				}
			}
		}
	}

	int normalizePower(float power) {
		if(power<0)
			power=0;
		if(power>maxPower)
			power=maxPower;
		float segment = ((float) numSegments * power)/maxPower;
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
		for (int i=0; i<numColums; i++) {
			for(int j=0; j<numSegments; j++) {
				Vector3 position;
				position = segmentPosition(i,j);
				int color=segmentColor(j);
				ledsColumns[i].Add(instantiateSegment(position, size, color));
			}
		}
	}

	GameObject instantiateSegment(Vector3 position, Vector3 size, int color) {
		GameObject segment=null;
		//Normalize position to center of the object
		position.x+=size.x/2f;
		position.y+=size.y/2f;
		position.x-=transform.localScale.x/2f;
		position.y-=transform.localScale.y/2f;
		position.x /= transform.localScale.x;
		position.y /= transform.localScale.y;
		
		switch (shape) {
		case ledShape.Quad:
			segment=GameObject.CreatePrimitive(PrimitiveType.Quad);
			scaleMultiplier=new Vector3(1,1,1);
			break;
		case ledShape.Cube:
			segment=GameObject.CreatePrimitive(PrimitiveType.Cube);
			scaleMultiplier=new Vector3(1,1,1);
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

		size.x = (transform.localScale.x/(float)numColums) - ledPadding;
		size.y=(transform.localScale.y / (float)numSegments) - ledPadding;
		size.z = size.y;
		return size;
	}

	//Returns the left-down position of the led
	Vector3 segmentPosition(int column, int segmentNumber) {
		Vector3 position=new Vector3();
		position.x=((float)column * transform.localScale.x / (float)numColums);
		position.y = ((float)segmentNumber * transform.localScale.y / (float)numSegments);
		position.z = ledDeltaZ;
		return position;
	}

	void destroyPreviousMeter () {
		for(int i=0; i<ledsColumns.Count; i++) {
			List<GameObject> column=ledsColumns[i];
			for(int j=0; j<column.Count; j++)
				Destroy(column[j]);
			column.Clear();
		}

	}

	float calcAvg(float lowFreq, float hiFreq, float[] spectrum) {
		int lowBound = freqToIndex(lowFreq);
		int hiBound = freqToIndex(hiFreq);
		float avg = 0f;
		for (int i = lowBound; i <= hiBound; i++)
			avg += spectrum[i];
		avg /= (hiBound - lowBound + 1);
		return avg;
	}

	int freqToIndex(float freq) {
		float bandwidth=(float)sampleRate/(float)numSamples;
		// special case: freq is lower than the bandwidth of spectrum[0]
		if (freq < bandwidth / 2) return 0;
		// special case: freq is within the bandwidth of spectrum[spectrum.length - 1]
		if (freq > sampleRate / 2 - bandwidth / 2) return (numSamples/2) - 1;
		// all other cases
		float fraction = freq / (float) sampleRate;
		int i = (int)(numSamples * fraction);
		return i;
	}
}
