using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    AudioSource source;

    [SerializeField] AudioClip startLoop = null;
    [SerializeField] AudioClip mainLoop = null;
    [SerializeField] AudioClip bossMusic = null;
    [SerializeField] AudioClip bossDeath = null;

    public delegate void SoundSpawnDelegate(GameObject sound);
    public static SoundSpawnDelegate SoundSpawn;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewScene;
        Enemy.BossDeath += BossDeathSound;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewScene;
        Enemy.BossDeath -= BossDeathSound;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        source = GetComponent<AudioSource>();
    }

    void NewScene(Scene scene, LoadSceneMode mode)
    {
        source.loop = true;

        if (CheckIfBoss())
        {
            StopAllCoroutines();
            source.Stop();
            source.clip = bossMusic;
            source.Play();
        }
        else if (source.clip != startLoop && source.clip != mainLoop)
        {
            Debug.Log(source.clip.name);
            PlayStart();
        }
        else if (!source.isPlaying)
        {
            PlayStart();
        }
    }

    bool CheckIfBoss()
    {
        if (SceneManager.GetActiveScene().name == "BladderBoss" ||
            SceneManager.GetActiveScene().name == "HeartBoss" ||
            SceneManager.GetActiveScene().name == "BrainBoss")
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void Start()
    {
        SoundSpawn(gameObject);
    }

    public void PlayStart()
    {
        if (source != null)
        {
            StopAllCoroutines();

            if (source.isPlaying)
            {
                source.Stop();
            }

            source.clip = startLoop;
            source.Play();
            StartCoroutine(SongEnd(source.clip.length - 0.25f));
        }

        
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
    }

    void BossDeathSound()
    {
        source.clip = bossDeath;
        source.Play();
        source.loop = false;
    }

}
