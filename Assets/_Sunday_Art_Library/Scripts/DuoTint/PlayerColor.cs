using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer[] renderers = null;
    [SerializeField] private int matIndex = 0;
    [Header("Settings")]
    [SerializeField] private ColorScheme colorScheme = default;
    // Start is called before the first frame update
    void Awake()
    {
        SetColorScheme(colorScheme);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColorScheme(ColorScheme colorScheme)
    {
        foreach (var renderer in renderers)
        {
            renderer.materials[matIndex].SetColor("_Color1", colorScheme.color1);
            renderer.materials[matIndex].SetColor("_Color2", colorScheme.color2);
        }
    }
}
