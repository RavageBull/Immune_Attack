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

    public delegate void SoundSpawnDelegate(GameObject sound);
    public static SoundSpawnDelegate SoundSpawn;

    private void OnEnable()
    {
        Boss.BossAppear += BossMusic;
        SceneManager.sceneLoaded += NewScene;
    }

    private void OnDisable()
    {
        Boss.BossAppear -= BossMusic;
        SceneManager.sceneLoaded -= NewScene;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        source = GetComponent<AudioSource>();
    }

    void NewScene(Scene scene, LoadSceneMode mode)
    {
        if (CheckIfBoss())
        {
            StopCoroutine("SongEnd");
            source.clip = bossMusic;
            source.Play();
        }
        else if (source.clip == bossMusic)
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

        if (source.isPlaying == false)
        {
            PlayStart();
        }
    }

    public void PlayStart()
    {
        if (source != null)
        {
            StopCoroutine("SongEnd");

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
        source.loop = true;
    }

    void BossMusic(string name)
    {

    }

}
