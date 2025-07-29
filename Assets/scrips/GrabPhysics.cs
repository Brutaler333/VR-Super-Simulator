using UnityEngine;
using UnityEngine.InputSystem;

public class GrabPhysics : MonoBehaviour
{
    public InputActionProperty grabInputSource;
    public float radius = 0.1f;
    public LayerMask grabbable;

    private FixedJoint fixedJoint;

    private bool isGrabbing = false;
    
    void FixedUpdate()
    {
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;

        if(isGrabButtonPressed && !isGrabbing)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, grabbable, QueryTriggerInteraction.Ignore);

            if(nearbyColliders.Length > 0)
            {
                Rigidbody nearbyRigidbody = nearbyColliders[0].attachedRigidbody;
                fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.autoConfigureConnectedAnchor = false;

                if(nearbyRigidbody)
                {
                    fixedJoint.connectedBody = nearbyRigidbody;
                    fixedJoint.connectedAnchor = nearbyRigidbody.transform.InverseTransformPoint(transform.position);   
                }else{
                    fixedJoint.connectedAnchor = transform.position;
                }

                isGrabbing = true;
            }
        }else if(!isGrabButtonPressed && isGrabbing){
            isGrabbing = false;
            if(fixedJoint)
            {
                Destroy(fixedJoint);
            }
        }
    }
}
