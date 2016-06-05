using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransportController : MonoBehaviour {

    List<SongManager.Song> songList;
    SongManager songManager;

    Object songPrefab;

	void Start () {
        songManager = GetComponent<SongManager>();
        
        getSongs();
	}

	void Update () {
	
	}

    void getSongs()
    {
        songList = songManager.getAllSongs();

        for (int i = 0; i < songList.Count; i++)
        {
            // remove mock vector
            Vector3 mockPos = new Vector3(0.275f, (i * 0.1f) - 0.05f, 0.995f);

            string buttonFolder = songList[i].name + "_Button";
            songPrefab = Resources.Load("Prefabs/SongButton");

            // instantiates song prefab
            Object song = Instantiate(songPrefab, mockPos, Quaternion.identity);
            song.name = buttonFolder;

            // sets this song prefab as child of Transport
            GameObject currentSong = GameObject.Find(buttonFolder);
            currentSong.transform.parent = transform;

            currentSong.tag = "Song";
        }
    }
}
