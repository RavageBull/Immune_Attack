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
    public List<int> roomSequence = new List<int>(); //the list that will contain the sequence of the rooms that are randomised

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

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //once a scene is loaded, we start to assign some values that need to be created
    void Load(Scene scene, LoadSceneMode mode)
    {
        isLoading = true;
        StartCoroutine("Loading");

        if (scene.name == "Title")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //spawns the warp sound.
        if (scene.name != "Title" /*&& scene.name != "DeathRoom"*/)
        {
            GameObject obj = Instantiate(loadInSound, transform.position, Quaternion.identity);
            Destroy(obj, 1);
        }

        //if the game loads to the start room, sets up the randomised sequence of rooms.
        //the intended sequence is 1 monster room, 1 power up room, 1 monster room, 1 boss room and so on.
        //this sequence is very hard coded and any changes to the indexes of the scenes will need appropriate adjustments to the script.
        //the first monster room will always be the same, since it will act as an introdutory level
        if (scene.name == "StartRoom")
        {
            roomIndex.Clear();
            roomSequence.Clear();

            //adds the scene indexes of all the basic rooms that will be randomised to the room list 
            //roomIndex.Add(2); this room will now always be the first one in the sequence
            roomIndex.Add(3);
            roomIndex.Add(4);
            roomIndex.Add(5);
            roomIndex.Add(6);
            roomIndex.Add(7);

            //randomises the list
            RandomiseRooms();

            //12 is the combination of monster rooms, power up rooms, and boss rooms possible in the sequence.
            //sequence is planned out as    M>P>M>B> M>P>M>B> M>P>M>B>
            //                              0 1 2 3  4 5 6 7  8 9 10 11
            for (int i = 0; i < 12; i++)
            {
                //the first room in the sequence is always the same
                if (i == 0)
                {
                    roomSequence.Add(2); //the scene index of the first room in the build settings
                }
                //these indexes are where powerup rooms will be
                else if (i == 1 || i == 5 || i == 9)
                {
                    roomSequence.Add(8); //the scene index of the power up room
                }
                //first boss room
                else if (i == 3)
                {
                    roomSequence.Add(9); //the index of the bladder boss
                }
                //second boss room
                else if (i == 7)
                {
                    roomSequence.Add(10); //the index of the heart boss
                }
                //third boss room
                else if (i == 11)
                {
                    roomSequence.Add(11); //index of brain boss
                }
                //if the current sequence is not a power up or a boss room, then add a monster room from the list
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
        else if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            Win();
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

    void Win()
    {
        SceneManager.LoadScene("WinRoom");
    }
}
