using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MixerManager : MonoBehaviour {

    public string activeSong;

    SongManager songManager;
    Track track;
    GameObject mixer;

    public List<SongManager.Song> allSongs;
    public List<Track> allTracks;
    public List<Track> activeSongTracks;
    public List<Track> allSoloed;

    public List<Track> toBeMuted;
    public List<Track> toBeSoloed;

    Transform muteButton;
    Transform soloButton;
    IsActiveEffect activeMuteEffect;
    IsActiveEffect activeSoloEffect;
    IsActiveEffect instrumentEffect;

    Transform currentSongDir;
    Transform currentTrackDir;
    Transform instrument;

    UnityEngine.Object trackPrefab;
    Vector3 cameraPos;

    string trackName;
    string action;

    float objectWidth;

    void Start()
    {
        cameraPos = Camera.main.gameObject.transform.position + new Vector3(0, 0, 1.25f);
        songManager = GetComponent<SongManager>();
        allTracks = new List<Track>();
        activeSongTracks = new List<Track>();
        allSoloed = new List<Track>();
        toBeMuted = new List<Track>();
        toBeSoloed = new List<Track>();

        mixer = GameObject.Find("Mixer");

        allSongs = songManager.getAllSongs();
        initializeSongs(allSongs);
    }

    void initializeSongs(List<SongManager.Song> songs)
    {
        for (int i = 0; i < songs.Count; i++)
        {
            instantiateTracks(songs[i]);
        }
    }

    void instantiateTracks(SongManager.Song song)
    {
        GameObject songFolder = new GameObject(song.name);
        songFolder.transform.parent = mixer.transform;

        UnityEngine.Object instrumentPanel = Resources.Load("Prefabs/InstrumentPanel");

        for (int i = 0; i < song.tracks.Count; i++)
        {
            string prefabName = song.tracks[i].name;

            // plots prefab tracks equally spaced around the user
            int angle = i * (360 / song.tracks.Count);
            Vector3 plotPos = Circle(cameraPos, 2.8f, angle);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, cameraPos - plotPos);

            // creates prefab based on instrument/track type or family
            string prefabType = song.tracks[i].type;
            trackPrefab = Resources.Load("Prefabs/" + prefabType);

            // recalibrates rotation to ensure first prefab is facing center
            rotation.x = 0.0f;

            // instantiates a track prefab
            var track = Instantiate(trackPrefab, plotPos, rotation);
            track.name = prefabName;

            // sets this track prefab to child of Mixer/:songName
            GameObject currentTrack = GameObject.Find(prefabName);
            currentTrack.transform.parent = songFolder.transform;
            AudioClip currentClip;

            // dynamically sets current tracks audio clip
            currentClip = Resources.Load<AudioClip>("Audio/" + song.tracks[i].clipName);
            initTrackAudio(prefabName, currentClip, song.tracks[i]);

            foreach (Transform child in currentTrack.transform)
            {
                if (child.GetComponent<Collider>())
                {
                    objectWidth = child.GetComponent<Collider>().bounds.size.x;
                }
            }

            // adds instrumentPanel to newly instantiated object
            float adjustedPosX;

            GameObject instroPanel = Instantiate(instrumentPanel, plotPos, rotation) as GameObject;
            Vector3 adjustedPos;

            instroPanel.name = "InstrumentPanel";
            instroPanel.transform.parent = currentTrack.transform;
            
            // returns corrected position.x of instroPanel based on instrument
            adjustedPosX = adjustPosition(objectWidth);

            // Drums are natually rotatated 180 - instroPanel needs x and z axis corrected manually 
            if (trackPrefab.name == "drums" || trackPrefab.name == "brass")
            {
                adjustedPos = new Vector3(-1 * adjustedPosX, 0.15f, -0.2f);
                instroPanel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                adjustedPos = new Vector3(adjustedPosX, 0.15f, 0.2f);
            }
            instroPanel.transform.localPosition = adjustedPos;

            // adds current track to allTracks for mixing board
            allTracks.Add(new Track(song.tracks[i].name, song.tracks[i].type, song.tracks[i].volume, song.tracks[i].isMuted, song.tracks[i].isSoloed, song.tracks[i].clipName, song.tracks[i].audioSource, song.tracks[i].songName));

            if (song.tracks[i].isSoloed && song.isActive)
            {
                allSoloed.Add(song.tracks[i]);
            }
        }
    
        // sets GameObject active/inactive state based on SongManager  
        songFolder.SetActive(song.isActive);

        if (song.isActive)
        {
            activeSong = song.name;
        }

        if (song.isActive)
        {
            StartCoroutine(getAudibleStatus());
        }
        setActiveSong();
    }

    // organizes soloed and muted tracks into lists
    IEnumerator getAudibleStatus()
    {
        toBeMuted.Clear();
        toBeSoloed.Clear();

        foreach (Track track in activeSongTracks)
        {
            if (track.isSoloed)
            {
                toBeSoloed.Add(track);
            }
            else
            {
                toBeMuted.Add(track);
            }
        }

        yield return null;
        updateMixingBoard();
    }

    // iterates over assembled lists to determine proper track state for active song
    void updateMixingBoard()
    {
        if (allSoloed.Count == 0)
        {
            foreach (Track track in activeSongTracks)
            {
                if (track.isMuted)
                {
                    track.audioSource.mute = true;
                }
                else
                {
                    track.audioSource.mute = false;
                }
                updateTrackUI(track);
            }
        }
        else
        {
            foreach (Track track in toBeMuted)
            {
                track.audioSource.mute = true;
                updateTrackUI(track);
            }
            foreach (Track track in toBeSoloed)
            {
                track.audioSource.mute = false;
                updateTrackUI(track);
            }
        }
    }

    private void initTrackAudio(string name, AudioClip audioClip, Track track)
    {
        AudioSource audioSource = GameObject.Find(name).transform.GetChild(0).GetComponent<AudioSource>();
        track.audioSource = audioSource;

        // initializes audioClip of instantiated prefab
        track.audioSource.clip = audioClip;
        track.audioSource.Play();
    }

    void MuteTrack(string name)
    {
        if (allSoloed.Count == 0)
        {
            foreach (Track track in activeSongTracks)
            {
                if (track.name == name)
                {
                    track.isMuted = !track.isMuted;
                    track.audioSource.mute = track.isMuted;
                }
            }
        }
        StartCoroutine(getAudibleStatus());
    }

    // used as proxy to start coroutine for SendMessageUpwards 
    void SoloTrack(string name)
    {
        StartCoroutine(SoloTrackCo(name));
    }

    IEnumerator SoloTrackCo(string name)
    {
        
        foreach (Track track in activeSongTracks)
        {
            if (track.name == name)
            {
                track.isSoloed = !track.isSoloed;
                if (track.isSoloed)
                {
                    toBeSoloed.Add(track);
                    allSoloed.Add(track);
                }
                else
                {
                    toBeSoloed.Remove(track);
                    allSoloed.Remove(track);
                }
            }
        }

        // DEBUGGING
        //foreach (Track track in toBeSoloed)
        //{
        //    Debug.Log(track.name);
        //}

        yield return null;
        StartCoroutine(getAudibleStatus());
    }

    // sets current song as the active list for mute, sole, and UI iterations
    void setActiveSong()
    {
        foreach (SongManager.Song song in allSongs)
        {
            if (song.name == activeSong)
            {
                activeSongTracks = song.tracks;
            }
        }
    }

    // sets visual cues for instruments, Solo button, Mute button based on active song track state
    void updateTrackUI(Track track)
    {
        currentSongDir = mixer.transform.Find(activeSong);
        currentTrackDir = currentSongDir.transform.Find(track.name);
        Debug.Log(track.name);
        Debug.Log(currentTrackDir.name);
        instrument = currentTrackDir.transform.GetChild(0);

        muteButton = currentTrackDir.transform.Find("InstrumentPanel/InterfacePanel/MuteSoloPanel/MuteButton/MuteButtonOutline");
        soloButton = currentTrackDir.transform.Find("InstrumentPanel/InterfacePanel/MuteSoloPanel/SoloButton/SoloButtonOutline");

        activeMuteEffect = muteButton.GetComponent<IsActiveEffect>();
        activeSoloEffect = soloButton.GetComponent<IsActiveEffect>();
        instrumentEffect = instrument.GetComponent<IsActiveEffect>();

        if (track.audioSource.mute)
        {
            activeMuteEffect.AddActivePanelState();
            instrumentEffect.RemoveActivePanelState();
        }
        else if (!track.audioSource.mute)
        {
            activeMuteEffect.RemoveActivePanelState();
            instrumentEffect.AddActivePanelState();
        }

        if (track.isSoloed)
        {
            activeSoloEffect.AddActivePanelState();
        }
        else if (!track.isSoloed)
        {
            activeSoloEffect.RemoveActivePanelState();
        }
    }

    // helper function to determine where to spawn instantiated instruments
    Vector3 Circle(Vector3 center, float radius, int ang)
    {
        float angle = ang;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }

    // normalized gameObject positions on X axis
    float adjustPosition(float width)
    {
        float positionX;
        if (width < 0.1f)
        {
            positionX = 0.12f;
        }
        else if (width > 0.5f)
        {
            positionX = 0.45f;
        }
        else
        {
            positionX = 0.175f;
        }
        return positionX;
    }
    void Update()
    {
        /// <summary>
        /// for testing of functions in Unity, create key press calls here.
        /// </summary> 

        // prints current mute/solo state of all tracks
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Alpha1 pressed");
            for (int at = 0; at < allTracks.Count; at++)
            {
                Debug.Log(allTracks[at].name);
                if (allTracks[at].isMuted)
                {
                    Debug.Log(allTracks[at].name + " is muted");
                }
                if (allTracks[at].isSoloed)
                {
                    Debug.Log(allTracks[at].isSoloed + " is soloed");
                }
            }
        }    
    }
}
