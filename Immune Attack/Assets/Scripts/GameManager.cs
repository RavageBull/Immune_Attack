using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<int> roomList;

    public GameObject player;

    private void OnEnable()
    {
        Portal.EnterPortal += NextScene;
        SceneManager.sceneLoaded += Check;
    }

    private void OnDisable()
    {
        Portal.EnterPortal -= NextScene;
        SceneManager.sceneLoaded -= Check;
    }

    void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Check(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void NextScene()
    {
        int sceneIndex;

        sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings); //randomly searches for next room. temporary.

        SceneManager.LoadScene(sceneIndex);
    }
}
