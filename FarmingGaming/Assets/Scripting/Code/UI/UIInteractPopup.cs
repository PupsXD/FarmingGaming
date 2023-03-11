using UnityEngine;

public class UIInteractPopup : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerTriggerDetector.WorldEventApproached += ActivatePopUp;
        PlayerTriggerDetector.WorldEventLeft += DeactivatePopUp;
    }

    private void ActivatePopUp()
    {
        this.gameObject.SetActive(true);
    }

    private void DeactivatePopUp()
    {
        this.gameObject.SetActive(false); 
    }
}
