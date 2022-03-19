using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(TankController))]
public class TankAgent : Agent
{

    Health health;

    [SerializeField]
    Transform target;

    TankController tank;
    MapManager map;
    float lastDist;

    public override void Initialize()
    {
        tank = GetComponent<TankController>();
        map = GetComponentInParent<MapManager>();
        health = GetComponent<Health>();
    }


    public override void OnEpisodeBegin()
    {
        health.Reset();
        map.ResetMap();
        lastDist = Vector3.Distance(transform.localPosition, target.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.ContinuousActions[0]);

        tank.Drive(Mathf.Abs(actions.ContinuousActions[0]) > 0.2f ? actions.ContinuousActions[0] : 0);

        tank.Turn(actions.ContinuousActions[1]);

        tank.Aim(actions.ContinuousActions[2]);

        if(actions.ContinuousActions[3] > 0.5f)
        { 
            int hit = tank.Shoot();
            if ( hit > 0)
            {
                AddReward(0.5f);

                if (hit > 1) { 
                    AddReward(1f);

                    //float dist = Vector3.Distance(transform.localPosition, target.localPosition);

                    //AddReward(-10* dist/lastDist);

                    EndEpisode(); 
                }
                //Debug.Log("Kill");
                //
            }
            else
            {
                AddReward(-0.01f);
            }
        }
        Vector3 deltaVector = (transform.localPosition - target.localPosition);
        deltaVector.y = 0;

        if(Vector3.Angle(deltaVector, -transform.forward) < 5f)
        {
            //AddReward(0.01f / MaxStep);
        }

        //Debug.Log(Vector3.Angle(deltaVector, transform.forward));


        if (Vector3.Angle(deltaVector, -tank.head.transform.forward) < 1f)
        {
            AddReward(0.1f / MaxStep);
        }
        else
        {
            //AddReward(-0.02f / MaxStep);

        }


        float dist = Vector3.Distance(transform.localPosition, target.localPosition);

        //if (dist >= lastDist - 0.4f) AddReward(-1f / MaxStep);

        if (dist < lastDist - 1f) AddReward(0.1f / MaxStep);
        else AddReward(-0.01f / MaxStep);

        lastDist = dist;
                  /*  //
          */
    }


    private void Update()
    {
        if (Vector3.Distance(transform.localPosition, target.localPosition) < 5f) {
            AddReward(1f);
            EndEpisode();
        }

        //AddReward(-(1 - Vector3.Distance(transform.localPosition, target.localPosition)/lastDist) / MaxStep );
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //observe own position
        sensor.AddObservation(transform.localPosition);

        //observe target position
        sensor.AddObservation(target.localPosition);

        //observe own rotation
        //sensor.AddObservation(transform.localRotation);


        Vector3 deltaVector = (transform.localPosition - target.localPosition);
        deltaVector.y = 0;

        sensor.AddObservation(Vector3.Angle(deltaVector, transform.forward));

        //observe angle between turret an target
        sensor.AddObservation(Vector3.Angle(deltaVector,-tank.head.transform.forward));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Vertical");

        continuousActions[1] = Input.GetAxisRaw("Horizontal");

        continuousActions[2] = Input.GetAxisRaw("Aim");

        continuousActions[3] = Input.GetAxisRaw("Fire1");
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Wall")
        {
            //AddReward(-Vector3.Distance(transform.localPosition, target.localPosition) / lastDist );
            AddReward(-10f);

            //EndEpisode();
        }
    }
    */

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            AddReward(-0.01f);
        }
    }
}
    