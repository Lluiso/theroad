using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RadioController : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] AudioSource audioSourceBroadcast;
    [SerializeField] AudioClip[] musicClips;

    [SerializeField] AudioClip startBroadcast;

    [SerializeField] AudioClip rainBroadcast;

    [SerializeField] AudioClip gameOverBroadcast;

    [SerializeField] Image radioImg;
    [SerializeField] Sprite radioActive;

    [SerializeField] Sprite radioInactive;

    int currentClip;

    // Start is called before the first frame update
    void Start()
    {
        currentClip = 0;
        updateClip();
        StartCoroutine(waitForRain());
        CarResourcesController.OnGameOver += playGameOverBroadcast;
        GameEvents.StartGame += playStartingBroadcast;
    }

    [Button]
    public void playStartingBroadcast()
    {
        playBroadcast(startBroadcast);
    }

    public void playGameOverBroadcast(string something)
    {
        playBroadcast(gameOverBroadcast);
    }

    IEnumerator waitForRain()
    {
        while (TrackGenerator.ProgressToFerry < 0.5f)
        {
            yield return null;
        }
        playBroadcast(rainBroadcast);
    }

    public void playBroadcast(AudioClip clip)
    {
        StartCoroutine(broadcastAnnouncement(clip));
    }

    IEnumerator broadcastAnnouncement(AudioClip clip)
    {
        audioSourceMusic.volume = 0.15f;
        audioSourceBroadcast.clip = clip;
        audioSourceBroadcast.Play();
        while (audioSourceBroadcast.isPlaying)
        {
            yield return null;
        }
        audioSourceMusic.volume = 0.7f;
    }

    void updateClip()
    {
        if (currentClip < musicClips.Length)
        {
            audioSourceMusic.Stop();
            audioSourceMusic.clip = musicClips[currentClip];
            audioSourceMusic.Play();
            radioImg.sprite = radioActive;
        }
        else
        {
            audioSourceMusic.Stop();
            currentClip = -1;
            radioImg.sprite = radioInactive;
        }
    }
    public void playNextClip()
    {
        ++currentClip;
        updateClip();
    }
}
