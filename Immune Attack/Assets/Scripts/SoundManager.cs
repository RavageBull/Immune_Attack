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

    // Start is called before the first frame update
    void Start()
    {
        SoundSpawn(gameObject);

        source = GetComponent<AudioSource>();

        source.clip = startLoop;
        source.Play();
        StartCoroutine(SongEnd(source.clip.length - 0.25f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SongEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        source.clip = mainLoop;
        source.Play();
        source.loop = true;
    }
}
