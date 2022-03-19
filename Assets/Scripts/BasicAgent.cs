using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgent : MonoBehaviour
{

    NeuralNetworkEvolution network;
    Transform food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(food.position);


    }

    float lastDistance;
    float distance;
    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        float[] input = new float[4];

        input[0] = transform.position.x;
        input[1] = transform.position.z;

        input[2] = food.position.x;
        input[3] = food.position.z;

        float[] output = network.FeedForward(input);

        //Debug.Log(output[0] + " : " + output[1]);
        velocity.x = output[0] * .2f;
        velocity.y = output[1] * .2f;

        transform.position += new Vector3(velocity.x,0,velocity.y);

        distance = Vector3.Distance(transform.position, food.position);

        //network.AddFitness(distance < lastDistance ? 5f : -0.1f);

        network.AddFitness(-distance);

        lastDistance = distance;



    }

    public void EndReward()
    {
        if(distance < 1f) network.AddFitness(500);
    }

    public void Init(NeuralNetworkEvolution _network, Transform _food)
    {
        this.network = _network;
        this.food = _food;
    }
}
