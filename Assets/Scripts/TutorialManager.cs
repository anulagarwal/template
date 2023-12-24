using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] Animator anim;
    [SerializeField] public int currentIndex = 1;
    [SerializeField] public int maxSteps;

    [SerializeField] public int currentLevel=1;


    public static TutorialManager Instance = null;

    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("loop", 0) == 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        anim.Play("L" + currentLevel + "S" + currentIndex);

    }

  
    public void PlayNext()
    {
        currentIndex++;
        if (currentIndex > maxSteps)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        anim.Play("L" + currentLevel + "S" + currentIndex);
    }
        
}
