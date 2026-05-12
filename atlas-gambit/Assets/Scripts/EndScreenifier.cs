using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenifier : MonoBehaviour
{
    public void lose(){
        SceneManager.LoadScene("LoseMenu");
    }

    public void win(){
        SceneManager.LoadScene("WinMenu");
    }
}
