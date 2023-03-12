using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        investigateButton.SetActive(false);
    }
}
