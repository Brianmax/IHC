using UnityEngine;
using System.Collections;

public class CharacterSelector : MonoBehaviour
{
    void SavePrefs(string character)
    {
        PlayerPrefs.SetString("character", character);
    }
}
