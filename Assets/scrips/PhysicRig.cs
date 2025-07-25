using UnityEngine;

public class PhysicRig : MonoBehaviour
{
    public Transform playerHead;
    public Transform leftHandController;
    public Transform rightHandController;    

    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;

    public CapsuleCollider bodyCollider;


    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;

    void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height/2, playerHead.localPosition.z);

        leftHandJoint.targetPosition = leftHandController.localPosition;
        leftHandJoint.targetRotation = leftHandController.localRotation;

        rightHandJoint.targetPosition = rightHandController.localPosition;
        rightHandJoint.targetRotation = rightHandController.localRotation;

        headJoint.targetPosition = playerHead.localPosition;
    }
}
