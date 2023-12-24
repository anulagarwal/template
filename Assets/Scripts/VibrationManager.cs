using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance = null;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMedium()
    {
        if (PlayerPrefs.GetInt("vibrate", 1) == 1)
        {
           HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
    }

    public void PlayHeavy()
    {
        if (PlayerPrefs.GetInt("vibrate", 1) == 1)
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
    }
}
