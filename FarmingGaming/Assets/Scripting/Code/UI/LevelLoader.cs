using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int levelIndex;
  
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}

