
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    [SerializeField] 
    GameObject agentPrefab;

    [SerializeField]
    GameObject food;

    [SerializeField]
    GenerationData networkData;


    [SerializeField]
    int agentCount = 10;

    NeuralNetworkEvolution[] nets;

    List<NeuralNetworkEvolution> networks = new List<NeuralNetworkEvolution>();

    List<GameObject> agents =  new List<GameObject>();

    bool isTraining;

    int generation;

    [SerializeField]
    int[] layers = { 4, 4, 4, 2 };


    [SerializeField]
    bool saveData;

    // Start is called before the first frame update
    void Start()
    {
        nets = new NeuralNetworkEvolution[agentCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTraining)
        {
            if(generation == 0) //Init Networks
            {
                for (int i = 0; i < agentCount; i++)
                {
                    if (networkData && networkData.json != null) //load existing Network
                    {
                        float[][][] savedWeights = Newtonsoft.Json.JsonConvert.DeserializeObject<float[][][]>(networkData.json);

                        networks.Add(new NeuralNetworkEvolution(networkData.layers, savedWeights, 1));

                        generation = networkData.generation;
                    }
                    else
                    {
                        networks.Add(new NeuralNetworkEvolution(layers));
                    }
                }
            }
            else
            {
                networks.Sort();

                for (int i = 0; i < networks.Count / 2; i++) //Sort and Mutate Networks
                {
                    networks[i] = new NeuralNetworkEvolution(networks[networks.Count - 1]);

                    networks[i].RandomMutate();

                    networks[i + networks.Count / 2] = new NeuralNetworkEvolution(networks[i + networks.Count / 2]);
                }
            }


            generation ++;

            isTraining = true;

            Invoke("LessonEnd", 3);

            //Debug.Log("End");

            InitAgents();
        }
    }

    void LessonEnd()
    {

        foreach (GameObject obj in agents)
        {
            obj.GetComponent<BasicAgent>().EndReward();
            Destroy(obj);
        }

        networks.Sort();

        Debug.Log(networks[0].GetFitness() + " : " + networks[networks.Count - 1].GetFitness());

        if (networkData && saveData) //save best Network
        {
            networkData.layers = networks[networks.Count - 1].layers;

            networkData.fitness = networks[networks.Count - 1].GetFitness();

            networkData.json = Newtonsoft.Json.JsonConvert.SerializeObject(networks[networks.Count - 1].weights);

            networkData.generation = generation;
        }

        isTraining = false;

        food.transform.position = new Vector3(Random.Range(-17f, 17f), 0, Random.Range(-11f, 11f));
    }

    void InitAgents()
    {
        //networks.Clear();
        agents.Clear();
        

        for (int i = 0; i < agentCount; i++)
        {
            //networks.Add(new NeuralNetworkEvolution(layers));
            agents.Add(Instantiate(agentPrefab));

            agents[i].GetComponent<BasicAgent>().Init(networks[i],food.transform);
        }
    }   
}
