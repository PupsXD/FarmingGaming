using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveButtonLeha : MonoBehaviour
{
    public GameObject investigateButton;
    // void Update()
    // {
    //     
    // }

    private void OnTriggerEnter2D(Collider2D col)
    {
        investigateButton.SetActive(true);
        if (col.tag == "EnemyOnMap")
        {
            SceneManager.LoadScene(3);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        investigateButton.SetActive(false);
    }
}
