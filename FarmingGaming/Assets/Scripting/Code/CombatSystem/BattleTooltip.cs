using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BattleTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text textPlanel;
    [SerializeField] private string _tooltipText;
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textPlanel.text = _tooltipText;
    }
    
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
        textPlanel.text = "";
    }
}
