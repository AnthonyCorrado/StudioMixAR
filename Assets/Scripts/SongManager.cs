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

    // Use this for initialization
    void Start () {
        mixer = GetComponent<MixerController>();
	}

    public List<Song> getAllSongs()
    {
        List<Song> SongList = new List<Song>();

        List<Track> recall = new List<Track>();
        List<Track> evolution = new List<Track>();

        recall = getRecall();
        evolution = getEvolution();

        
        // (song name, genre, tracks, isActive)
        SongList.Add(new Song("Recall", "pop", recall, true));
        SongList.Add(new Song("Evolution", "soundtrack", evolution, false));

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

    //public List<Track> initTracks()
    //{
    //    List<Track> songList = new List<Track>();
    //    List<Track> recall = new List<Track>();
    //    List<Track> evolution = new List<Track>();

    //    recall = getRecall();
    //    evolution = getEvolution();

    //    songList.AddRange(recall);
    //    songList.AddRange(evolution);
    //    return songList;
    //}

    /// <summary>
    /// Below is the list of track details for each song.
    /// Will need to come up with a more efficient approach in the future.
    /// </summary>
    List<Track> getRecall()
    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, is soloed?, name of audio clip, component<AudioSource>, song name)
        InstrumentList.Add(new Track("drums", "drums", 7.0f, false, false, "recallDrums", null, "Recall"));
        InstrumentList.Add(new Track("vox", "leadVox", 7.0f, false, false, "recallLeadVocals", null, "Recall"));
        InstrumentList.Add(new Track("pizzicato", "synth", 7.0f, false, false, "recallPizzicato", null, "Recall"));
        InstrumentList.Add(new Track("electricBass", "electricBass", 7.0f, false, false, "recallEBass", null, "Recall"));
        InstrumentList.Add(new Track("strings", "string", 7.0f, false, false, "recallStrings", null, "Recall"));
        InstrumentList.Add(new Track("harmony", "harmonyVox", 7.0f, false, false, "recallHarmonyVocals", null, "Recall"));

        return InstrumentList;
    }

    List<Track> getEvolution()

    {
        List<Track> InstrumentList = new List<Track>();

        // (name of gameObject created, name of prefab to use, volume, is muted?, is soloed?, name of audio clip, component<AudioSource>, song name)
        InstrumentList.Add(new Track("brass", "brass", 7.0f, false, false, "evolutionBrass", null,"Evolution"));
        InstrumentList.Add(new Track("cello", "string", 7.0f, false, false, "evolutionCello", null, "Evolution"));
        //InstrumentList.Add(new Track("chorusFemale", "chorus", 7.0f, false, false, "evolutionChorusFemale", null, "Evolution"));
        //InstrumentList.Add(new Track("chorusMale", "chorus", 7.0f, false, false, "evolutionChorusMale", null, "Evolution"));
        //InstrumentList.Add(new Track("glockenspiel", "xylophone", 7.0f, false, false, "evolutionGlockenspiel", null, "Evolution"));
        InstrumentList.Add(new Track("harp", "string", 7.0f, false, false, "evolutionHarp", null, "Evolution"));
        //InstrumentList.Add(new Track("percussion", "percussionSymbols", 7.0f, false, false, "evolutionPercussion", null, "Evolution"));
        InstrumentList.Add(new Track("piano", "piano", 7.0f, false, false, "evolutionPiano", null, "Evolution"));
        //InstrumentList.Add(new Track("strings", "strings", 7.0f, false, false, "evolutionStrings", null, "Evolution"));
        //InstrumentList.Add(new Track("violas", "strings", 7.0f, false, false, "evolutionViolas", null, "Evolution"));
        InstrumentList.Add(new Track("violin", "string", 7.0f, false, false, "evolutionViolin", null, "Evolution"));
        InstrumentList.Add(new Track("windDeep", "wind", 7.0f, false, false, "evolutionWindDeep", null, "Evolution"));
        InstrumentList.Add(new Track("windMid", "wind", 7.0f, false, false, "evolutionWindMid", null, "Evolution"));

        return InstrumentList;
    }
}
