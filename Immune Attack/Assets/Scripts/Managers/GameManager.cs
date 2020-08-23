using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility.Inspector;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [HideInInspector] public GameObject player;

    GameObject room;
    /*[HideInInspector]*/ public GameObject sound;
    bool isLoading;

    [SerializeField] GameObject loadInSound = null;

    List<int> roomIndex = new List<int>(); // the indexes of the basic monster rooms
    List<int> roomSequence = new List<int>(); //the list that will contain the sequence of the rooms that are randomised

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
        //room = null;
        isLoading = true;
        StartCoroutine("Loading");

        //spawns the warp sound.
        if (scene.name != "StartRoom" && scene.name != "DeathRoom")
        {
            GameObject obj = Instantiate(loadInSound, transform.position, Quaternion.identity);
            Destroy(obj, 1);
        }

        //if the game loads to the start room, sets up the randomised sequence of rooms.
        //the intended sequence is 2 monster rooms, 1 power up room, 1 monster room, 1 boss room and so on.
        //this sequence is very hard coded and any changes to the indexes of the scenes will need appropriate adjustments to the script.
        if (scene.name == "StartRoom")
        {
            roomIndex.Clear();
            roomSequence.Clear();

            //adds the scene indexes of all the basic rooms to the room list
            roomIndex.Add(2);
            roomIndex.Add(3);
            roomIndex.Add(4);
            roomIndex.Add(5);
            roomIndex.Add(6);
            roomIndex.Add(7);

            //randomises the list
            RandomiseRooms();

            //10 is the combination of monster rooms, power up rooms, and boss rooms possible in the sequence.
            for (int i = 0; i < 10; i++)
            {
                //these indexes are where powerup rooms will be
                if (i == 2 || i == 7)
                {
                    //8 is the scene index of the power up room
                    roomSequence.Add(8);
                }
                else if (i == 4)
                {
                    //9 is the index of the bladder boss
                    roomSequence.Add(9);
                }
                else if (i == 9)
                {
                    //10 is the index of the heart boss
                    roomSequence.Add(10);
                }
                else
                {
                    //else adds the next randomised room index to the sequence.
                    roomSequence.Add(roomIndex[0]);
                    roomIndex.RemoveAt(0);
                }

            }
        }
    }

    void RandomiseRooms()
    {
        for (int i = 0; i < roomIndex.Count; i++)
        {
            int temp = roomIndex[i];
            int randomIndex = Random.Range(i, roomIndex.Count);
            roomIndex[i] = roomIndex[randomIndex];
            roomIndex[randomIndex] = temp;

            Debug.Log(roomIndex[i]);
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
        if (roomSequence.Count > 0)
        {
            SceneManager.LoadScene(roomSequence[0]);
            roomSequence.RemoveAt(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            //EndGame
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void PlayerDeath()
    {
        SceneManager.LoadScene("DeathRoom");
    }
}
