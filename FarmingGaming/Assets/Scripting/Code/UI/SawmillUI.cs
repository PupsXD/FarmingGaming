using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SawmillUI : MonoBehaviour
{
    [SerializeField] private Sawmill _sawmill;
    [SerializeField] private Button _collectButton;
    [SerializeField] private Image _fillImage;
    private void Awake()
    {
        _collectButton.onClick.AddListener(TryCollect);
    }

    private void Update()
    {
        if (_collectButton.gameObject.activeSelf)
            _fillImage.fillAmount = _sawmill.NormalizedProductionTime;
    }
    private void TryCollect()
    {
        if (!_sawmill.Ready) return;
        _sawmill.CollectProductionResult();
        Debug.Log("wood has been collected");
    }
}
