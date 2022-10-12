using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlatformHeightSelector : MonoBehaviour
{
    void SavePrefs(int height)
    {
        PlayerPrefs.SetInt("platform_height", height);

        // TODO: Async
        SceneManager.LoadScene("Game");
    }
}
