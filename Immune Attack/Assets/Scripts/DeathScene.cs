using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameManager.manager.player);
        Destroy(GameManager.manager.sound);

        StartCoroutine("Restart");
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("StartRoom");
    }
}
