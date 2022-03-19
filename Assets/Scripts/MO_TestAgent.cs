using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_TestAgent : MonoBehaviour
{

    NeuralNetworkEvolution network;
    [SerializeField]
    Transform food;
    [SerializeField]
    GenerationData networkData;
    [SerializeField]
    float speed = .2f;
    [SerializeField]
    float rotSpeed = .2f;
    Rigidbody rb;
    float fitness;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        float[][][] savedWeights = Newtonsoft.Json.JsonConvert.DeserializeObject<float[][][]>(networkData.json);

        network = new NeuralNetworkEvolution(networkData.layers, savedWeights, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float[] input = new float[5];

        Debug.DrawLine(transform.position, transform.position + transform.forward * 5f, Color.red, 0);



        input[0] = transform.position.x;

        input[1] = transform.position.z;



        RaycastHit hit_forward;


        if (Physics.Raycast(transform.position, transform.forward, out hit_forward, 5f))
        {
            input[2] = hit_forward.distance;
        }
        else
        {
            input[2] = -1;
        }


        Vector3 v = GetNearestFood();

        input[3] = v.x;
        input[4] = v.z;
        //input[5] = v.y;


        float[] output = network.FeedForward(input);


        rb.velocity += transform.forward * Mathf.Abs(output[0]) * speed;

        rb.angularVelocity = new Vector3(0, output[1] * rotSpeed, 0);



    }



    Vector3 GetNearestFood()
    {
        Vector3 nearest = Vector3.zero;
        float dist = -1;

        nearest = food.position;

        dist = Vector3.Distance(transform.position, nearest);

   
        return new Vector3(nearest.x, dist, nearest.z);
    }

}
