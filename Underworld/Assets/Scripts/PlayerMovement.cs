using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody RB;
    public Vector3 Direction; // checking to see if any keys are being pressed for movement
    
    public float MoveHorizontal, MoveVertical, Speed;
    public bool RestrainY, RestrainX;
    public bool IsKeyDown   //Boolean the lets player know is a key is down useful to stop player if no keys are pressed
    {
        get
        {
            if (Direction.sqrMagnitude == 0) return false;
            return true;
        }
    }

    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        this.Speed = 15f;
        this.Direction = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.Direction.x = this.Direction.z = 0;
        //rb2D.drag = 0;

        MoveHorizontal = Input.GetAxis("Horizontal");
        MoveVertical = Input.GetAxis("Vertical");

        // x is lateral, y is vertical, z is forward/backward
        this.Direction = new Vector3(MoveHorizontal, 0, MoveVertical);
        this.Direction.Normalize();
        RB.AddForce(Direction * Speed);

        //this.transform.Translate(Direction.x * Speed, Direction.z * Speed, 0);

        if ((Direction.x < 1 || Direction.x > -1)
            && (Direction.z < 1 || Direction.z > -1))
        {
            //rb2D.velocity.Scale(new Vector2(0, 0));// .Set(0, 0);
            //rb2D.velocity.Set(0, 0);
            //rb2D.
            //rb2D.drag = 100000;

        }

        //Get Keys From Input Directly
        Direction.x = Direction.y = 0;
        //direction += direction;
        Direction.Normalize();  //Normalize
        

    }
}
