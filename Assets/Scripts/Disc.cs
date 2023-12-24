using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class Disc : MonoBehaviour
{

    [SerializeField] public DiscColor color;
    [SerializeField] public GameObject vfx;
    [SerializeField] public Stack currentStack;
    [SerializeField] public int stackIndex;
    [SerializeField] public GameObject normalObject;
    [SerializeField] public GameObject outLineObject;
    [SerializeField] public GameObject greyObject;
    [SerializeField] public GameObject wowVFX;

    [SerializeField] public TextMeshProUGUI requirement;
    [SerializeField] public Image img;


    [SerializeField] public bool isSpin;

    [SerializeField] public bool isLocked;




    public void SetupDisc(Stack s, int index)
    {
        currentStack = s;
        stackIndex = index;
    }

    public void LockDisc(UnlockRequirement ur)
    {
        normalObject.SetActive(false);
        greyObject.SetActive(false);
        outLineObject.SetActive(true);
        requirement.text = "3x";
        img.color = Color.red;
    }

    //Check selected disc
        //If other stack then check for color
            //If same color then Stack
            //IF other color then snap back
        //If same stack then reset selection

    private void OnMouseDown()
    {

        if(TutorialManager.Instance !=null && StackManager.Instance.selectedDisc != null)
        {

                currentStack.SelectDisc(stackIndex);
                return;
        }

        if (TutorialManager.Instance != null && GetComponent<TutorialStep>() != null && TutorialManager.Instance.currentIndex == GetComponent<TutorialStep>().ID)
        {
            TutorialManager.Instance.PlayNext();
            currentStack.SelectDisc(stackIndex);
            return;
        }
        if((TutorialManager.Instance != null && GetComponent<TutorialStep>() == null) || (TutorialManager.Instance != null && GetComponent<TutorialStep>() != null && TutorialManager.Instance.currentIndex != GetComponent<TutorialStep>().ID))
        {
            return;
        }

        if (color != DiscColor.NULL || StackManager.Instance.selectedDisc != null)
            {
                currentStack.SelectDisc(stackIndex);
            }
        

    }
    public void Wow()
    {
        wowVFX.transform.parent = null;
        wowVFX.transform.Translate(new Vector3(0, 2, 0));
        wowVFX.SetActive(true);
    }

    public void SelectDisc()
    {
        // Original scale
        // Original scale
        Highlight();

        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.localPosition;


        // Scale up-down "punch" using DOTween
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 1, 0.5f).OnComplete(() =>
        {
            // Reset the scale back to original just in case
            transform.localScale = originalScale;
        });

        transform.DOMoveY(originalPosition.y +1f, 0.1f);

    }

    public void AddDiscTo(Stack s)
    {
        Dehighlight();
        currentStack.RemoveDisc(this);
        currentStack = s;
        stackIndex = s.AddDisc(this);
        transform.SetParent(s.transform);        
        transform.DOMove(currentStack.GetDiscTransformPosition(this), 0.2f);
    }
    public void PlayVFX()
    {
        vfx.SetActive(true);
    }

    public void Pop()
    {        
        currentStack.RemoveDisc(this);
        currentStack = null;
        gameObject.SetActive(false);
        transform.SetParent(null);
        Destroy(gameObject);
    }

    public void Blink()
    {
        Sequence mySequence = DOTween.Sequence();

      
            Color originalColor = normalObject.GetComponent<Renderer>().material.color;
        Color c = Color.red;
        if(color == DiscColor.Red)
        {
            c = Color.white;
        }      
        mySequence.Append(normalObject.GetComponent<Renderer>().material.DOColor(c, 0.15f))
          
    .Join(normalObject.transform.DOShakePosition(0.15f, new Vector3(0f, 0.15f, 0f), 20, 90, false, false))
    .Append(normalObject.GetComponent<Renderer>().material.DOColor(originalColor, 0.15f))
    .SetLoops(4) // Set the number of loops here.
    .OnKill(() =>
    {
        normalObject.GetComponent<Renderer>().material.color = originalColor;
  



    });



    }

    public void UpdateIndex(int index)
    {
        stackIndex = index;
    }

    public void ResetSelection()
    {
        transform.DOMoveY(currentStack.GetDiscTransformPosition(this).y, 0.1f);
        Dehighlight();
    }

    public void Highlight()
    {
        normalObject.SetActive(false);
  //      greyObject.SetActive(false);
        outLineObject.SetActive(true);
    }

    public void Dehighlight()
    {
        normalObject.SetActive(true);
//        greyObject.SetActive(false);

        outLineObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
