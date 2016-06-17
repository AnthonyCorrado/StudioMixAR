using UnityEngine;
using System.Collections;

public class TrackManager : MonoBehaviour {

}

public class Track
{
    public string name;
    public string type;
    public float volume;
    public bool isMuted;
    public bool isSoloed;
    public string clipName;
    public AudioSource audioSource;
    public string songName;

    public Track(string name, string type, float volume, bool isMuted, bool isSoloed, string clipName, AudioSource audioSource, string songName)
    {
        this.name = name;
        this.type = type;
        this.volume = volume;
        this.isMuted = isMuted;
        this.isSoloed = isSoloed;
        this.clipName = clipName;
        this.audioSource = audioSource;
        this.songName = songName;
    }
}