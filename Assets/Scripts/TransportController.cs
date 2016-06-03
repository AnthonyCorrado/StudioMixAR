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

        // remove mock vector
        Vector3 mockPos = new Vector3(0.275f, 0.06f, 0.995f);
        for (int i = 0; i < songList.Count; i++)
        {
            songPrefab = Resources.Load("Prefabs/SongButton");

            // instantiates song prefab
            Object song = Instantiate(songPrefab, mockPos, Quaternion.identity);
            song.name = songList[i].name;
        }
    }
}
