using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public List<HingeJoint2D> joints = new List<HingeJoint2D>();

    // Call this function when a screw is detached from this block.
    public void DetachScrew(HingeJoint2D jointToDetach)
    {
        joints.Remove(jointToDetach);
        Destroy(jointToDetach);
    }
}
