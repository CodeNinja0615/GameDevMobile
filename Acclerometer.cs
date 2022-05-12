
using UnityEngine;
public class Acclerometer : MonoBehaviour
{
    // Move object using accelerometer
    public Rigidbody rb;
    public float speed = 15.0f;

   // void Start()
   // {
   //     speed = PlayerPrefs.GetFloat("accelerometerSave");
   // }
    void Update()
    {
        speed = PlayerPrefs.GetFloat("accelerometerSave");
        if (rb.position.y < 2)
        {
            Vector3 dir = Vector3.zero;
            // we assume that device is held parallel to the ground
            // and Home button is in the right hand
            // remap device acceleration axis to game coordinates:
            //  1) XY plane of the device is mapped onto XZ plane
            //  2) rotated 90 degrees around Y axis
            dir.x = Input.acceleration.x;
            //dir.y = -Input.acceleration.y;
            // clamp acceleration vector to unit sphere
            if (dir.sqrMagnitude > 1)
                dir.Normalize();
            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;
            // Move object
            transform.Translate(dir * speed);
        }
    }

    
}
