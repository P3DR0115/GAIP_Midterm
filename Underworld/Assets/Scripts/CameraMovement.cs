using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject Parent;
    Vector2 cameraDir;
    Vector3 RelativePlayer;
    public float HSpeed, VSpeed;
    private float Yaw, YawAxis, YawAxisPrev, Pitch;

    // Start is called before the first frame update
    void Start()
    {
        HSpeed = VSpeed = 2.0f;

        if(Parent == null)
        {
            GameObject[] candidates = FindObjectsOfType<GameObject>();

            foreach(GameObject go in candidates)
            {
                if (go.tag == "Player")
                    Parent = go;
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //    if (Input.GetAxis("Mouse X") != YawAxisPrev)
    //    {
    //        YawAxis = Input.GetAxis("Mouse X");
    //        Yaw += HSpeed * Input.GetAxis("Mouse X");

    //    }

    //    YawAxisPrev = YawAxis;

    //    //cameraDir = new Vector2(Yaw, Pitch);
    //    //cameraDir.Normalize();
    //    RelativePlayer = Parent.transform.position;
    //    RelativePlayer.y = 0;
        
    //    transform.RotateAround(Parent.transform.position, Vector3.up, Yaw);
    //    transform.rotation.SetLookRotation(RelativePlayer);
    //    //transform.eulerAngles = new Vector3(Pitch, 0.0f, 0.0f);

    //}
}
