using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.manager != null)
        {
            Destroy(GameManager.manager.player);
            Destroy(GameManager.manager.sound);
        }

        StartCoroutine("ToTitle");
    }

    IEnumerator ToTitle()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Title");
    }
}
