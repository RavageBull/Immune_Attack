using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<int> roomList;

    GameObject player;

    private void OnEnable()
    {
        Portal.EnterPortal += NextScene;
    }

    private void OnDisable()
    {
        Portal.EnterPortal -= NextScene;
    }

    void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void NextScene()
    {
        int sceneIndex;

        sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings);

        SceneManager.LoadScene(sceneIndex);
    }
}
