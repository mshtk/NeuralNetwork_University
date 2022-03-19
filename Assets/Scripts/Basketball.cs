using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball : MonoBehaviour
{

    Rigidbody rb;
    Vector3 start;
    bool hasShot;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0) ResetBall();

        if(!hasShot && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }


    void ResetBall()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = start;
        hasShot = false;
    }

    void Shoot()
    {
        Vector3 dir = Vector3.Normalize(Vector3.up + Vector3.forward);
        rb.useGravity = true;
        rb.AddForce(dir * power, ForceMode.Impulse);
        hasShot = true;
    }
}
