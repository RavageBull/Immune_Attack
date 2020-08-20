using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [HideInInspector] public GameObject player;

    //public List<int> roomList;

    GameObject room;
    /*[HideInInspector]*/ public GameObject sound;
    bool isLoading;

    [SerializeField] GameObject loadInSound = null;

    public delegate void FinishLoadingDelegate();
    public static FinishLoadingDelegate FinishLoading;

    private void OnEnable()
    {
        Portal.EnterPortal += NextScene;
        SceneManager.sceneLoaded += Load;
        Player.PlayerSpawn += SetPlayer;
        RoomManager.RoomSpawn += SetRoom;
        SoundManager.SoundSpawn += SetSound;
        Player.Death += PlayerDeath;
    }

    private void OnDisable()
    {
        Portal.EnterPortal -= NextScene;
        SceneManager.sceneLoaded -= Load;
        Player.PlayerSpawn -= SetPlayer;
        RoomManager.RoomSpawn -= SetRoom;
        SoundManager.SoundSpawn -= SetSound;
        Player.Death -= PlayerDeath;
    }

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //once a scene is loaded, we start to assign some values that need to be created
    void Load(Scene scene, LoadSceneMode mode)
    {
        room = null;
        isLoading = true;
        StartCoroutine("Loading");

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            GameObject obj = Instantiate(loadInSound, transform.position, Quaternion.identity);
            Destroy(obj, 1);
        }
    }

    //this loop waits for the room and player variables to be assigned and possible other important values
    //after this finishes, a finish loading event occurs for other scripts to know
    IEnumerator Loading()
    {
        while (isLoading)
        {
            if (room != null && player != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = room.GetComponent<RoomManager>().playerSpawn.position;
                player.GetComponent<CharacterController>().enabled = true;

                isLoading = false;

                FinishLoading();
            }

            yield return new WaitForSeconds(0.05f);        
        }
    }

    //this triggers when a room is spawned
    void SetRoom(GameObject inRoom)
    {
        room = inRoom;
    }

    //this triggers when a player is spawned
    void SetPlayer(GameObject inPlayer)
    {
        //if the player variable is empty, set the recently spawned player as the player
        //if we already have a player, then destroy the recently spawned player

        if (player == null)
        {
            player = inPlayer;
        }
        else
        {
            Destroy(inPlayer.gameObject);
        }
        
    }

    void SetSound(GameObject inSound)
    {
        if (sound == null)
        {
            sound = inSound;
        }
        else
        {
            Destroy(inSound.gameObject);
        }

        //if it is the boss room, play the start song again
        if (SceneManager.GetActiveScene().name == "TestScene 2")
        {
            sound.GetComponent<SoundManager>().PlayStart();
        }
    }

    void NextScene()
    {
        /*int sceneIndex;

        sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings); //randomly searches for next room. temporary.

        SceneManager.LoadScene(sceneIndex);*/

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void PlayerDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }
}
