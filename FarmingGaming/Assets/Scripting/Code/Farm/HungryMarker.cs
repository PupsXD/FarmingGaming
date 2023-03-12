using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungryMarker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _graphics;
    [SerializeField] private Sprite _feeded;
    [SerializeField] private Sprite _hungry;

    public void Connect(Pound pound)
    {
        StartCoroutine(UpdateMarker(pound));
        pound.OnBeingFeeded.AddListener(() => UpdateGraphics(pound));
    }

    private void UpdateGraphics(Pound pound)
    {
        _graphics.sprite = pound.IsHungry ? _hungry : _feeded;
    }

    IEnumerator UpdateMarker(Pound pound)
    {
       while (true) 
       {
            UpdateGraphics(pound);
            yield return new WaitForSeconds(60);
       }
    }
}
