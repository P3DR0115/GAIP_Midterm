using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSense : MonoBehaviour
{
    GameObject ParentObject;
    GameObject IntruderObject;
    Collider TouchCollider;

    private void Awake()
    {
        if(ParentObject == null)
        {
            ParentObject = this.gameObject;
        }

        if(TouchCollider == null)
        {
            TouchCollider = ParentObject.GetComponent<Collider>();
        }
    }

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
        if (other.gameObject.tag == "Player")
        {
            //RotateToFaceTarget(other);
            RotateToTarget(other);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    // Working Attempt
    private void RotateToTarget(Collider collision)
    {
        IntruderObject = collision.gameObject;

        Vector3 relativePosition = IntruderObject.transform.position - ParentObject.transform.position;
        relativePosition.y = 0; // Set to zero so that there's no DeltaY, making it only a difference in two dimensions with DeltaX and DeltaZ
        Quaternion targetRotation = new Quaternion();

        targetRotation.y = Mathf.Tan(relativePosition.z / relativePosition.x);
        
        targetRotation.SetLookRotation(relativePosition);
        ParentObject.transform.rotation = targetRotation; //= Mathf.Tan((relativePosition.z / relativePosition.x));
    }

    // Failed Attempts
    private void RotateToFaceTarget(Collider collision)
    {
        IntruderObject = collision.gameObject;

        Transform enemyPosition = IntruderObject.transform;
        Vector3 relativePostion = enemyPosition.transform.position - ParentObject.transform.position;

        relativePostion.y = ParentObject.transform.position.y;

        // Getting X and Z coordinates becasue Unity uses Y coordinate to go up! Not Z coordinate to go up!
        float ownerX, intruderX, ownerZ, intruderZ;
        ownerX = ParentObject.transform.position.x;
        intruderX = enemyPosition.position.x;
        ownerZ = ParentObject.transform.position.z;
        intruderZ = enemyPosition.position.z;

        // Making a right triangle, the deltaPosition is the diagonal in the right triangle
        float deltaX, deltaZ, deltaPosition;

        deltaX = ownerX - intruderX;
        deltaZ = ownerZ - intruderZ;
        //deltaX *= -1;
        //deltaZ *= -1;
        deltaPosition = (deltaX * deltaX) + (deltaZ * deltaZ);

        float OppOverAdj = deltaZ / deltaX;
        float targetRotationY = (float)System.Math.Tan(OppOverAdj);
        //targetRotationY *= Mathf.Rad2Deg;

        //if(deltaX >= 0 && deltaZ >= 0)
        //{
        //    // Don't compensate
        //}
        //else if(deltaX < 0 && deltaZ > 0)
        //{
        //    targetRotationY += 90;
        //}
        //else if (deltaX < 0 && deltaZ < 0)
        //{
        //    targetRotationY += 180;
        //}
        //else if (deltaX > 0 && deltaZ < 0)
        //{
        //    targetRotationY += 270;
        //}

        //Vector3 rotation = new Vector3();
        //ParentObject.transform.Rotate(Vector3.up, targetRotationY);
        Quaternion TargetRotationQuaternion = new Quaternion();
        //TargetRotationQuaternion = ParentObject.transform.rotation;
        TargetRotationQuaternion.SetLookRotation(relativePostion);
        
        Quaternion newRotationFinal = new Quaternion(ParentObject.transform.rotation.x, TargetRotationQuaternion.y, ParentObject.transform.rotation.z, ParentObject.transform.rotation.w);
        //newRotationFinal.y -= (90 * Mathf.Deg2Rad);
        ParentObject.transform.rotation = newRotationFinal;

    }
}
