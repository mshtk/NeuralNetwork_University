using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankVFX : MonoBehaviour
{
    [SerializeField]
    LineRenderer line;

    [SerializeField]
    Transform lineOffset;

    [SerializeField]
    GameObject impactVFX;
    public void Shoot(Vector3 impactPoint, string tag, Vector3 normal)
    {
        StartCoroutine(BulletTrail(impactPoint, normal));
    }

    IEnumerator BulletTrail(Vector3 impactPoint, Vector3 normal)
    {
        Instantiate(impactVFX, impactPoint, Quaternion.LookRotation(normal));
        line.enabled = true;
        line.SetPosition(0, lineOffset.position );
        line.SetPosition(1, impactPoint + new Vector3(0, lineOffset.position.y,0));
        yield return new WaitForSeconds(0.1f);
        line.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
