using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleFeedback : MonoBehaviour
{
    [SerializeField] private Animator animationFeedback = null;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            animationFeedback.SetTrigger("in");
           
        }
    }
}
