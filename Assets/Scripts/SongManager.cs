using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

    public List<Song> songList;
    public List<Track> song;
    AudioClip audioClip;

    public class Song
    {
        public string name;
        public string genre;

        public Song(string name, string genre)
        {
            this.name = name;
            this.genre = genre;
        }
    }

    public class Track
    {
        public string name;
        public string type;
        public float volume;
        public bool isMuted;
        public string clipName;
        public string songName;

        public Track(string name, string type, float volume, bool isMuted, string clipName, string songName)
        {
            this.name = name;
            this.type = type;
            this.volume = volume;
            this.isMuted = isMuted;
            this.clipName = clipName;
            this.songName = songName;
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<Song> getAllSongs()
    {
        List<Song> SongList = new List<Song>();

        SongList.Add(new Song("Recall", "pop"));
        SongList.Add(new Song("Not Recall", "soundtrack"));

        return SongList;
    }

    public List<Track> getSong(string songTitle)
    {
        if (songTitle == "recall")
        {
            song = getRecall();
        }
        else if (songTitle == "Not Recall")
        {
            song = notRecall();
        }
        return song;
    }

    List<Track> getRecall()
    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, name of audio clip to use, song name)
        InstrumentList.Add(new Track("drums", "drums", 7.0f, false, "recallDrums", "Recall"));
        InstrumentList.Add(new Track("vox", "leadVox", 7.0f, false, "recallLeadVocals", "Recall"));
        InstrumentList.Add(new Track("pizzicato", "synth", 7.0f, false, "recallPizzicato", "Recall"));
        InstrumentList.Add(new Track("electricBass", "electricBass", 7.0f, false, "recallEBass", "Recall"));
        InstrumentList.Add(new Track("strings", "string", 7.0f, false, "recallStrings", "Recall"));
        InstrumentList.Add(new Track("harmony", "harmonyVox", 7.0f, false, "recallHarmonyVocals", "Recall"));

        return InstrumentList;
    }

    List<Track> notRecall()
    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, name of audio clip to use, song name)
        InstrumentList.Add(new Track("drums", "drums", 7.0f, false, "recallDrums", "Not Recall"));
        InstrumentList.Add(new Track("electricBass", "electricBass", 7.0f, false, "recallEBass", "Not Recall"));
        InstrumentList.Add(new Track("strings", "string", 7.0f, false, "recallStrings", "Not Recall"));
        InstrumentList.Add(new Track("harmony", "harmonyVox", 7.0f, false, "recallHarmonyVocals", "Not Recall"));

        return InstrumentList;
    }
}
