using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [Header("Dropable Tag")]
    public string tagName = "Player";
    [Header("Layer Index")]
    public int canDropLayer;
    public int cannotDropLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(tagName))
        {
            other.gameObject.layer = canDropLayer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag(tagName))
        {
            other.gameObject.layer = cannotDropLayer;
        }
    }
}
