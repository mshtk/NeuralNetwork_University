using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballNwtworkManager : MonoBehaviour
{

    List<NeuralNetworkEvolution> networks = new List<NeuralNetworkEvolution>();

    [SerializeField]
    int[] layers = { 4, 4, 4, 2 };

    [SerializeField]
    GenerationData networkData;

    [SerializeField]
    bool saveData;

    bool isTraining;

    int generation;

    Transform target;



    private void FixedUpdate()
    {
        float[] input = new float[4];

        input[0] = transform.position.x;

        input[1] = transform.position.z;

    }

    void LessonEnd()
    {
        networks.Sort();

        Debug.Log(networks[0].GetFitness() + " : " + networks[networks.Count - 1].GetFitness());

        if (networkData && saveData)
        {
            networkData.layers = networks[networks.Count - 1].layers;

            networkData.fitness = networks[networks.Count - 1].GetFitness();

            networkData.json = Newtonsoft.Json.JsonConvert.SerializeObject(networks[networks.Count - 1].weights);

            networkData.generation = generation;
        }

        isTraining = false;

    }
}
