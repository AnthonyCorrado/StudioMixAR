using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerManager : MonoBehaviour {

    public string activeSong;

    SongManager songManager;
    List<SongManager.Song> allSongs;
    List<SongManager.Track> allTracks;
    GameObject mixer;

    Object trackPrefab;
    Vector3 cameraPos;

    string trackName;
    string action;

    float objectWidth;

    public Dictionary<Transform, AudioSource> allChannels;

    void Start()
    {
        cameraPos = Camera.main.gameObject.transform.position + new Vector3(0, 0, 1.25f);
        songManager = GetComponent<SongManager>();
        allChannels = new Dictionary<Transform, AudioSource>();
        mixer = GameObject.Find("Mixer");

        allSongs = songManager.getAllSongs();

        for (int x = 0; x < allSongs.Count; x++)
        {
            Debug.Log(allSongs[x].name);
            for (int i = 0; i < allSongs[x].tracks.Count; i++)
            {
                Debug.Log(allSongs[x].tracks[i].name);
                Debug.Log(allSongs[x].tracks[i].isMuted);
            }
        }
        


        initializeTracks(allSongs);
    }

    void Update()
    {

    }

    void initializeTracks(List<SongManager.Song> songs)
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

        Object instrumentPanel = Resources.Load("Prefabs/InstrumentPanel");

        for (int i = 0; i < song.tracks.Count; i++)
        {
            string prefabName = song.tracks[i].name + "_" + song.tracks[i].songName;

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

            //allTracks.Add()

            // sets this track prefab to child of Mixer/:songName
            GameObject currentTrack = GameObject.Find(prefabName);
            currentTrack.transform.parent = songFolder.transform;
            AudioClip currentClip;


            // dynamically sets current tracks audio clip
            currentClip = Resources.Load<AudioClip>("Audio/" + song.tracks[i].clipName);
            initTrackAudio(prefabName, currentClip, song.tracks[i].volume);

            foreach (Transform child in currentTrack.transform)
            {
                if (child.GetComponent<Collider>())
                {
                    objectWidth = child.GetComponent<Collider>().bounds.size.x;
                }
                if (child.GetComponent<AudioSource>())
                {
                    createMixingBoard(child);
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
            if (trackPrefab.name == "drums")
            {
                adjustedPos = new Vector3(-1 * adjustedPosX, 0.15f, -0.2f);
                instroPanel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                adjustedPos = new Vector3(adjustedPosX, 0.15f, 0.2f);
            }
            instroPanel.transform.localPosition = adjustedPos;
        }

        songFolder.SetActive(song.isActive);

        if (song.isActive)
        {
            activeSong = song.name;
        }
    }

    void createMixingBoard(Transform track)
    {
        AudioSource trackClip = track.GetComponent<AudioSource>();
        allChannels.Add(track, trackClip);
    }

    public Dictionary<Transform, AudioSource> getMixingBoard()
    {
        return allChannels;
    }

    private void initTrackAudio(string name, AudioClip audioClip, float volume)
    {
        AudioSource audioSource = GameObject.Find(name).transform.GetChild(0).GetComponent<AudioSource>();

        // initializes audioClip of instantiated prefab
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    void MuteOrSoloTrack(Dictionary<string, string> details)
    {
        // determine what method to call and value to pass
        foreach (KeyValuePair<string, string> detail in details)
        {
            if (detail.Key == "name")
            {
                trackName = detail.Value;
            }
            else if (detail.Key == "action")
            {
                action = detail.Value + "Track";
            }
        }
        mixer.SendMessage(action, trackName);
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
}
