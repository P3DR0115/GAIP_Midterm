using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSense : MonoBehaviour
{
    GameObject PlayerObject;
    GameObject ParentObject;
    GameObject IntruderObject;

    private void Awake()
    {
        if(PlayerObject == null)
        {
            PlayerObject = FindObjectOfType<PlayerMovement>().gameObject;
        }

        if (ParentObject == null)
        {
            ParentObject = this.gameObject;
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
        if(other.tag == "Player")
        {
            if(PlayerObject.GetComponent<PlayerMovement>().MovementState == PlayerMovementSpeed.Run)
            {
                RotateToTarget(other);

            }
        }
    }

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
}
