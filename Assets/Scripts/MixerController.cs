using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour {

    SongManager songManager;
    List<SongManager.Track> song;

    Object trackPrefab;
    Vector3 cameraPos;

	void Start () {
        cameraPos = Camera.main.gameObject.transform.position + new Vector3(0, 0, 1.25f);
        songManager = GetComponent<SongManager>();
        song = songManager.getSong("Not Recall");

        buildSong(song);
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void buildSong (List<SongManager.Track> tracks)
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            // plots prefab tracks equally spaced around the user
            int angle = i * (360 / tracks.Count);
            Vector3 plotPos = Circle(cameraPos, 2.2f, angle);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, cameraPos - plotPos);

            // creates prefab based on instrument/track type or family
            string prefabType = tracks[i].type;
            trackPrefab = Resources.Load("Prefabs/" + prefabType);

            // instantiates a track prefab
            var track = Instantiate(trackPrefab, plotPos, rotation);
            track.name = tracks[i].name;

            GameObject currentTrack = GameObject.Find(tracks[i].name);
            Debug.Log(currentTrack);
            //currentTrack.gameObject.tag = tracks[i].songName;
            AudioClip currentClip;

            // dynamically sets current tracks audio clip
            currentClip = Resources.Load<AudioClip>("Audio/" + tracks[i].clipName);
            Debug.Log(currentClip);
            initTrackAudio(tracks[i].name, currentClip, tracks[i].volume);
        }
    }

    private void initTrackAudio(string name, AudioClip audioClip, float volume)
    {
        AudioSource audioSource = GameObject.Find(name).transform.GetChild(0).GetComponent<AudioSource>();

        // initializes audioClip of instantiated prefab
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    Vector3 Circle(Vector3 center, float radius, int ang)
    {
        float angle = ang;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }
}
