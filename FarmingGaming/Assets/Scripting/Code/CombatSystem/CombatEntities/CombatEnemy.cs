using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatEnemy : CombatEntity
{
    [SerializeField] private ParticleSystem captureParcicles;
    [SerializeField] private UnityEvent CaptureEvent;
    public void CaptureON()
    {
        CaptureEvent.Invoke();
        captureParcicles.Play();
    }
    public void CaptureOFF()
    {
        captureParcicles.Stop();
    }
}
