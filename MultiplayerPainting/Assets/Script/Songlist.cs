using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Songlist : MonoBehaviour
{
    public AudioClip[] songs;
    int currentSong = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<AudioSource>().isPlaying == false)
        {
            currentSong++;
            if (currentSong >= songs.Length)
            {
                currentSong = 0;
                GetComponent<AudioSource>().clip = songs[currentSong];
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
