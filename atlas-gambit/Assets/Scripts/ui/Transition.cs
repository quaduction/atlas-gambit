using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    public void ShowPanel()
    {
            panel1.SetActive(false);
            panel2.SetActive(true);
    }

    public void HidePanel()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }
}
