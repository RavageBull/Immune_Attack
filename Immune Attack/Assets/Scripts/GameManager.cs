﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<int> roomList;

    public GameObject player;
    GameObject room;
    GameObject sound;

    bool isLoading;

    public delegate void FinishLoadingDelegate();
    public static FinishLoadingDelegate FinishLoading;

    public delegate void RotatePlayerDelegate(Quaternion rotation);
    public static RotatePlayerDelegate RotatePlayer;

    private void OnEnable()
    {
        Portal.EnterPortal += NextScene;
        SceneManager.sceneLoaded += Load;
        Player.PlayerSpawn += SetPlayer;
        RoomManager.RoomSpawn += SetRoom;
        SoundManager.SoundSpawn += SetSound;
    }

    private void OnDisable()
    {
        Portal.EnterPortal -= NextScene;
        SceneManager.sceneLoaded -= Load;
        Player.PlayerSpawn -= SetPlayer;
        RoomManager.RoomSpawn -= SetRoom;
        SoundManager.SoundSpawn -= SetSound;
    }

    void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);


    }

    //once a scene is loaded, we start to assign some values that need to be created
    void Load(Scene scene, LoadSceneMode mode)
    {
        room = null;
        isLoading = true;
        StartCoroutine("Loading");
    }

    //this loop waits for the room and player variables to be assigned and possible other important values
    //after thiis finishes, a finish loading event occurs for other scripts to know
    IEnumerator Loading()
    {
        while (isLoading)
        {
            if (room != null && player != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = room.GetComponent<RoomManager>().playerSpawn.position;
                player.GetComponent<CharacterController>().enabled = true;

                RotatePlayer(room.GetComponent<RoomManager>().playerSpawn.rotation);

                isLoading = false;

                FinishLoading();
            }

            yield return new WaitForSeconds(0.05f);        
        }
    }

    private void Update()
    {
        if (Input.GetKey("e"))
        {
            player.transform.Rotate(0, 30, 0);
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
        int sceneIndex;

        sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings); //randomly searches for next room. temporary.

        //SceneManager.LoadScene(sceneIndex);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
