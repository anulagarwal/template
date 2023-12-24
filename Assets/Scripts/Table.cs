using UnityEngine;

public class Table : MonoBehaviour
{
    private float initialYRotation;
    private Vector3 startMousePosition;
    public bool isTutorial;

    [SerializeField] GameObject tutorialObj;
    void OnMouseDown()
    {
        // Capture initial rotation and mouse position when mouse is pressed down
        initialYRotation = transform.eulerAngles.y;
        startMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        // Calculate how far the mouse has moved
        float dragDistance = Input.mousePosition.x - startMousePosition.x;

        // Adjust this factor as you need to make rotation smoother or more responsive
        float rotationFactor = -0.1f;

        // Set the new rotation
        transform.rotation = Quaternion.Euler(0, initialYRotation + dragDistance * rotationFactor, 0);

        if (Mathf.Abs(dragDistance) > 250f && isTutorial)
        {
            tutorialObj.SetActive(false);

        }
    }
}
