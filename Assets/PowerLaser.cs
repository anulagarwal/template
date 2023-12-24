using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PowerLaser : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Disc pos, bool final)
    {
        GetComponent<AutoRotate>().OrbitCenterTransform = pos.transform.position;
        GetComponent<AutoRotate>().enabled = true;
        GetComponent<AutoRotate>().Orbit(true);
        GetComponent<AutoRotate>().OrbitRotationSpeed = 5500;

        Sequence mySequence = DOTween.Sequence().PrependInterval(0.3f); // initial 0.2s delay
        mySequence.AppendCallback(() =>
        {
            transform.DOMove(pos.transform.position, 0.2f)          
           .OnComplete(() => {
               pos.currentStack.Match(pos.stackIndex, 1, final);
               Destroy(gameObject);
           });

        });

       
    }
}
