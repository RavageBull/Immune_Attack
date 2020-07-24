using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource source;

    [SerializeField] AudioClip startLoop;
    [SerializeField] AudioClip mainLoop;

    public delegate void SoundSpawnDelegate(GameObject sound);
    public static SoundSpawnDelegate SoundSpawn;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SoundSpawn(gameObject);

        source = GetComponent<AudioSource>();

        PlayStart();

    }

    public void PlayStart()
    {
        StopCoroutine("SongEnd");

        source.clip = startLoop;
        source.Play();
        StartCoroutine(SongEnd(source.clip.length - 0.25f));
    }

    IEnumerator SongEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayLoop();
    }

    void PlayLoop()
    {
        source.clip = mainLoop;
        source.Play();
        source.loop = true;
    }
}
