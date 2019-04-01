using UnityEngine;
using System.Collections;
 
public class Jump : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float initialAngle;

    void Start()
    {
        var rigid = GetComponent<Rigidbody>();

        Vector3 p = target.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects   
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);


        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        //stop movement
        Rigidbody r = GetComponent<Rigidbody>();
        r.isKinematic = true;
        r.velocity = Vector3.zero;
        
        //works - but it's perhaps better to change target pos?????
        Vector3 pos = transform.position;
        pos.x = col.transform.position.x;
        pos.z = col.transform.position.z;
        transform.position = pos;
    }

    void jump(GameObject from, GameObject to)
    { 
}
}
