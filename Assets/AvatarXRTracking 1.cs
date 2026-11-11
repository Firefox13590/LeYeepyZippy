using UnityEngine;

[DefaultExecutionOrder(-100)]
public class AvatarXRTracking : MonoBehaviour
{
    [Header("Sources XR")]
    public Transform xrHead;
    public Transform xrLeftHand;
    public Transform xrRightHand;

    [Header("Cibles Avatar")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    void Update()
    {
        if (xrHead != null && headTarget != null)
        {
            headTarget.position = xrHead.position;
            headTarget.rotation = xrHead.rotation;
        }

        if (xrLeftHand != null && leftHandTarget != null)
        {
            leftHandTarget.position = xrLeftHand.position;
            leftHandTarget.rotation = xrLeftHand.rotation;
        }

        if (xrRightHand != null && rightHandTarget != null)
        {
            rightHandTarget.position = xrRightHand.position;
            rightHandTarget.rotation = xrRightHand.rotation;
        }
    }
}
