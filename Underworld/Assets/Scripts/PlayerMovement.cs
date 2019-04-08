using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovementSpeed
{
    Stand,
    Walk,
    Run,
}

public class PlayerMovement : MonoBehaviour
{
    Rigidbody RB;
    public Vector3 Direction; // checking to see if any keys are being pressed for movement
    public Vector3 RotationTemp; // Used to normalize the rotation.
    public Transform TargetRotation; // The target rotation for the player when rotating.
    public PlayerMovementSpeed MovementState;

    public float MoveHorizontal, MoveVertical, MovementSpeed, RotationSpeed;
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
        this.MovementSpeed = 15f;
        this.RotationSpeed = 2.5f; ;
        this.Direction = Vector3.zero;
        this.MovementState = PlayerMovementSpeed.Stand;
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

        RotateThePlayer();

        MoveThePlayer();


        //this.transform.Translate(Direction.x * Speed, Direction.z * Speed, 0);

        //if ((Direction.x < 1 || Direction.x > -1)
        //    && (Direction.z < 1 || Direction.z > -1))
        //{
        //    //rb2D.velocity.Scale(new Vector2(0, 0));// .Set(0, 0);
        //    //rb2D.velocity.Set(0, 0);
        //    //rb2D.
        //    //rb2D.drag = 100000;

        //}

        //Get Keys From Input Directly
        //Direction.x = Direction.y = 0;
        ////direction += direction;
        //Direction.Normalize();  //Normalize


    }

    private void MoveThePlayer()
    {

        // x is lateral, y is vertical, z is forward/backward. However, I'll really be using x to rotate the player

        //First, I must determine what the player's relative forward is.


        this.Direction = new Vector3(0, 0, MoveVertical);
        this.Direction.Normalize();

        if (this.Direction.z > 0)
            RB.AddForce(this.transform.forward * MovementSpeed);
        else if(this.Direction.z < 0)
            RB.AddForce(-this.transform.forward  * MovementSpeed);
    }

    private void RotateThePlayer()
    {
        this.TargetRotation = this.transform;
        this.RotationTemp = new Vector3(MoveHorizontal, 0, 0);
        this.RotationTemp.Normalize();
        MoveHorizontal = RotationTemp.x;

        this.transform.Rotate(Vector3.up, (MoveHorizontal * RotationSpeed));
    }
}
