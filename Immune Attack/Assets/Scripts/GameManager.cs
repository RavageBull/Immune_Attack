using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<int> roomList;

    public GameObject player;

    public GameObject room;

    bool isLoading;

    private void OnEnable()
    {
        Portal.EnterPortal += NextScene;
        SceneManager.sceneLoaded += Load;
        Player.PlayerSpawn += SetPlayer;
        RoomManager.RoomSpawn += SetRoom;
    }

    private void OnDisable()
    {
        Portal.EnterPortal -= NextScene;
        SceneManager.sceneLoaded -= Load;
        Player.PlayerSpawn -= SetPlayer;
        RoomManager.RoomSpawn -= SetRoom;
    }

    void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Load(Scene scene, LoadSceneMode mode)
    {
        room = null;
        isLoading = true;
        StartCoroutine("Loading");
    }
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
            }

            yield return new WaitForSeconds(0.1f);        
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

    void NextScene()
    {
        int sceneIndex;

        sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings); //randomly searches for next room. temporary.

        SceneManager.LoadScene(sceneIndex);
    }
}
