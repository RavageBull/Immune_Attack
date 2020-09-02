using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.manager != null)
        {
            Destroy(GameManager.manager.player);
            Destroy(GameManager.manager.sound);
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //StartCoroutine("Restart");
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("StartRoom");
    }
}
