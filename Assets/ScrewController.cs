using UnityEngine;
using DG.Tweening; // Don't forget to include this for DOTween!

public class ScrewController : MonoBehaviour
{
    public static bool isScrewLifted = false;
    private Vector3 originalPosition;
    public BlockController parentBlock;
    private HingeJoint2D joint;

    void Start()
    {
        originalPosition = transform.position;
        joint = GetComponent<HingeJoint2D>();
        parentBlock = joint.connectedBody.GetComponent<BlockController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject && !isScrewLifted)
            {
                print(hitCollider.name);
                LiftScrew();
            }
           // print(hitCollider.name);

            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, 0.5f);


            if (hit.collider == null || hit.collider.gameObject.tag == "Hole" && isScrewLifted)
            {
                print(hit.collider.name);
                parentBlock.DetachScrew(joint);

                // Here's DOTween in action!
                transform.DOMove(hit.collider.transform.position, 0.5f).OnComplete(() =>
                {
                    isScrewLifted = false;
                });
            }
            else
            {
                // Do something if the screw can't be placed, maybe snap it back?
            }

            

        }

        if (isScrewLifted)
        {
           // Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0) && isScrewLifted)
        {
            //PlaceScrew();
        }
    }

    void LiftScrew()
    {
        
        isScrewLifted = true;
    }

    void PlaceScrew()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, 0.5f);

        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

        if (hitCollider != null && hitCollider.gameObject == this.gameObject && isScrewLifted)
        {
            print(hitCollider.name + "ad");
        }
       
        if (hit.collider == null || hit.collider.gameObject.tag == "Hole")
        {
            print(hit.collider.name);
            parentBlock.DetachScrew(joint);

            // Here's DOTween in action!
            transform.DOMove(hit.collider.transform.position, 0.5f).OnComplete(() =>
            {
                isScrewLifted = false;
            });
        }
        else
        {
            // Do something if the screw can't be placed, maybe snap it back?
        }
    }
}
