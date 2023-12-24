using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevels;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);


        if (currentLevel > maxLevels)
        {
            if (PlayerPrefs.GetInt("loop", 0)==0)
            {
                PlayerPrefs.SetInt("loop", 1);
            }
            int newId = currentLevel % maxLevels;
            if (newId == 0)
            {
                newId = maxLevels;
            }
            SceneManager.LoadScene("Level " + (newId));
        }
        else
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
    }

}
