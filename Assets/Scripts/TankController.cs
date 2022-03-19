using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankController : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField]
    public Transform head;

    [SerializeField]
    float speed;

    [SerializeField]
    float turnSpeed;

    [SerializeField]
    float aimSpeed;

    [SerializeField]

    TankVFX vfx;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TryGetComponent<TankVFX>(out vfx);
    }

    public void Drive(float _input)
    {
        rb.AddForce(transform.forward * _input * speed);
    }

    public void Turn(float _input)
    {
        rb.AddTorque(Vector3.up * _input * turnSpeed);
    }

    public void Aim(float _input)
    {
        head.Rotate(Vector3.up * _input * aimSpeed);
    }

    float timer;
    float coooldown = 2f;
    bool canShoot;
    public bool Cooldown()
    {
        
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return false;
        }

        return true;
    }

    private void Update()
    {
        canShoot = Cooldown();    
    }

    public int Shoot()
    {
        if (!canShoot) return 0;

        timer = coooldown;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, head.forward, out hit, Mathf.Infinity))
        {
            if (vfx) { vfx.Shoot(hit.point, hit.collider.tag, hit.normal); }

            if (hit.collider.tag == "Enemy")
            {
                if (hit.collider.GetComponent<Health>().Damage(15) <= 0) return 2;

                return 1;
            }

            Debug.DrawLine(transform.position, transform.position + head.forward * 10, Color.red);
        }
        return 0;
    }
}
