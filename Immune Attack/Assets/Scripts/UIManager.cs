using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject player;

    public Text health;

    private void OnEnable()
    {
        Player.DamageTaken += UpdateHealth;
    }

    private void OnDisable()
    {
        Player.DamageTaken -= UpdateHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ideally whenever the player's health is changed in any way, an event gets triggered which this script will listen to and
    //in turn trigger this function
    void UpdateHealth()
    {
        if (GameManager.manager.player != null)
        {
            health.text = GameManager.manager.player.GetComponent<Stats>().health.ToString();
        }
    }
}
