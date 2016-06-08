using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

    public List<Track> song;
    AudioClip audioClip;
    MixerController mixer;

    public class Song
    {
        public string name;
        public string genre;
        public List<Track> tracks;
        public bool isActive;

        public Song(string name, string genre, List<Track> tracks, bool isActive)
        {
            this.name = name;
            this.genre = genre;
            this.tracks = tracks;
            this.isActive = isActive;
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
        mixer = GetComponent<MixerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<Song> getAllSongs()
    {
        List<Song> SongList = new List<Song>();

        List<Track> recall = new List<Track>();
        List<Track> evolution = new List<Track>();

        recall = getRecall();
        //evolution = getEvolution();

        // (song name, genre, tracks, isActive)
        SongList.Add(new Song("Recall", "pop", recall, true));
        //SongList.Add(new Song("Evolution", "soundtrack", evolution, false));

        return SongList;
    }

    //public List<Track> getSong(string songTitle)
    void getSong(string songTitle)
    {
        Debug.Log(songTitle);
        if (songTitle == "Recall")
        {
            song = getRecall();
        }
        else if (songTitle == "Evolution")
        {
            song = getEvolution();
        }
        //return song;
        mixer.SendMessage("buildSong", song);
    }

    public List<Track> initTracks()
    {
        List<Track> songList = new List<Track>();
        List<Track> recall = new List<Track>();
        List<Track> evolution = new List<Track>();

        recall = getRecall();
        //evolution = getEvolution();

        songList.AddRange(recall);
        //songList.AddRange(evolution);
        return songList;
    }


    /// <summary>
    /// Below is the list of track details for each song.
    /// Will need to come up with a more efficient approach in the future.
    /// </summary>
    List<Track> getRecall()
    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, name of audio clip to use, song name)
        InstrumentList.Add(new Track("drums", "drums", 7.0f, false, "recallDrums", "Recall"));
        InstrumentList.Add(new Track("vox", "leadVox", 7.0f, false, "recallLeadVocals", "Recall"));
        //InstrumentList.Add(new Track("pizzicato", "synth", 7.0f, false, "recallPizzicato", "Recall"));
        //InstrumentList.Add(new Track("electricBass", "electricBass", 7.0f, false, "recallEBass", "Recall"));
        //InstrumentList.Add(new Track("strings", "string", 7.0f, false, "recallStrings", "Recall"));
        //InstrumentList.Add(new Track("harmony", "harmonyVox", 7.0f, false, "recallHarmonyVocals", "Recall"));

        return InstrumentList;
    }

    List<Track> getEvolution()
    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, name of audio clip to use, song name)
        InstrumentList.Add(new Track("brass", "brass", 7.0f, false, "evolutionBrass", "Evolution"));
        InstrumentList.Add(new Track("cello", "string", 7.0f, false, "evolutionCello", "Evolution"));
        InstrumentList.Add(new Track("chorusFemale", "chorus", 7.0f, false, "evolutionChorusFemale", "Evolution"));
        InstrumentList.Add(new Track("chorusMale", "chorus", 7.0f, false, "evolutionChorusMale", "Evolution"));
        InstrumentList.Add(new Track("glockenspiel", "xylophone", 7.0f, false, "evolutionGlockenspiel", "Evolution"));
        InstrumentList.Add(new Track("harp", "string", 7.0f, false, "evolutionHarp", "Evolution"));
        InstrumentList.Add(new Track("percussion", "percussionSymbols", 7.0f, false, "evolutionPercussion", "Evolution"));
        InstrumentList.Add(new Track("piano", "piano", 7.0f, false, "evolutionPiano", "Evolution"));
        InstrumentList.Add(new Track("strings", "strings", 7.0f, false, "evolutionStrings", "Evolution"));
        InstrumentList.Add(new Track("violas", "strings", 7.0f, false, "evolutionViolas", "Evolution"));
        InstrumentList.Add(new Track("violin", "string", 7.0f, false, "evolutionViolin", "Evolution"));
        InstrumentList.Add(new Track("windDeep", "wind", 7.0f, false, "evolutionWindDeep", "Evolution"));
        InstrumentList.Add(new Track("windMid", "wind", 7.0f, false, "evolutionWindMid", "Evolution"));

        return InstrumentList;
    }
}
