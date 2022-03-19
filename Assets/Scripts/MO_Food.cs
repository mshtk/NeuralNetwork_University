using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Food : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MO_Trainer agent))
        {
            agent.ResetTime();
            transform.position = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-20f, 20f));
        }
    }
}
