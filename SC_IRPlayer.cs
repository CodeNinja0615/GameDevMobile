using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SC_IRPlayer : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 8;
    //float SidewayForce = 60f;
    Rigidbody r;
    public Rigidbody rb;
    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;
    //public SwipeDetector swipeDetector;
   
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        r.freezeRotation = true;
        r.useGravity = false;
        defaultScale = transform.localScale;
    }

    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            r.velocity = new Vector3(r.velocity.x, CalculateJumpVerticalSpeed(), r.velocity.z);
        }

        //Crouch
        crouch = Input.GetKey(KeyCode.S);
        if (crouch)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime * 7);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * 7);
        }
        
        
        if (rb.position.y < 0)
        {
            SC_GroundGenerator.instance.gameOver = true;
        }
        // For the character to move Right
        //if (Input.GetKeyDown(KeyCode.D) && grounded)
        ////if (joystick.Horizontal >=0.6f)
        //{
        //    rb.AddForce(SidewayForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        //}
        //
        //// For the character to move Left
        //if (Input.GetKeyDown(KeyCode.A) && grounded)
        ////if (joystick.Horizontal >= -0.6f)
        //{
        //
        //    rb.AddForce(-SidewayForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        //}
    }
// Update is called once per frame
void FixedUpdate()
    {
        // We apply gravity manually for more tuning control
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            //print("GameOver!");
            FindObjectOfType<AudioManager>().Play("CollisionSound");
            SC_GroundGenerator.instance.gameOver = true;
            //Time.timeScale = 0f;
            //FindObjectOfType<AudioManager>().Play("CollisionSound");
        }
    }
}