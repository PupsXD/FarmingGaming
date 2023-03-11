using System;
using UnityEngine;

public class PlayerTriggerDetector : MonoBehaviour
{
    public static event Action WorldEventApproached;
    public static event Action WorldEventLeft;

    private void OnTriggerEnter(Collider other)
    {
        WorldEventApproached?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        WorldEventLeft?.Invoke();   
    }
}
