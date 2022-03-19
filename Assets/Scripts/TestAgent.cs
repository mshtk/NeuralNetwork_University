using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgent : MonoBehaviour
{

    NeuralNetworkEvolution network;
    [SerializeField]
    Transform food;
    [SerializeField]
    GenerationData networkData;
    // Start is called before the first frame update
    void Start()
    {
        float[][][] savedWeights = Newtonsoft.Json.JsonConvert.DeserializeObject<float[][][]>(networkData.json);

        network = new NeuralNetworkEvolution(networkData.layers, savedWeights, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        float[] input = new float[4];

        input[0] = transform.position.x;
        input[1] = transform.position.z;

        input[2] = food.position.x;
        input[3] = food.position.z;

        float[] output = network.FeedForward(input);

        velocity.x = output[0] * .2f;
        velocity.y = output[1] * .2f;

        transform.position += new Vector3(velocity.x, 0, velocity.y);

    }
}
