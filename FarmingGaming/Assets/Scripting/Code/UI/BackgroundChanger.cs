using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;      
    [SerializeField] private Sprite[] images;

    void Start()
    {
        int randomIndex = Random.Range(0, images.Length);        
        backgroundImage.sprite = images[randomIndex];
    }
}