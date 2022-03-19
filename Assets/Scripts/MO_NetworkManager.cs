using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_NetworkManager : MonoBehaviour
{

    [SerializeField]
    GameObject agentPrefab;

    [SerializeField]
    GameObject foodPrefab;

    [SerializeField]
    GenerationData networkData;


    [SerializeField]
    int agentCount = 10;

    [SerializeField]
    int foodCount = 10;

    NeuralNetworkEvolution[] nets;

    List<NeuralNetworkEvolution> networks = new List<NeuralNetworkEvolution>();

    List<GameObject> agents = new List<GameObject>();
    List<MO_Trainer> agentTrainer = new List<MO_Trainer>();
    List<GameObject> foods = new List<GameObject>();

    bool isTraining;

    int generation;

    [SerializeField]
    int[] layers = { 4, 4, 4, 2 };


    [SerializeField]
    bool saveData;

    [SerializeField, Range(1,10)]
    float timeScale;

    [SerializeField]
    float trainingTime = 3;

    NeuralNetworkEvolution best ;

    // Start is called before the first frame update
    void Start()
    {
        nets = new NeuralNetworkEvolution[agentCount];

        

        for (int i = 0; i < foodCount; i++)
        {
            foods.Add(Instantiate(foodPrefab));
            foods[i].transform.position = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-20f, 20f));

        }

    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;



        if (!isTraining)
        {
            if (generation == 0)
            {
                for (int i = 0; i < agentCount; i++)
                {
                    if (networkData && networkData.json != null)
                    {
                        float[][][] savedWeights = Newtonsoft.Json.JsonConvert.DeserializeObject<float[][][]>(networkData.json);

                        networks.Add(new NeuralNetworkEvolution(networkData.layers, savedWeights, 1));

                        generation = networkData.generation;
                    }
                    else
                    {
                        networks.Add(new NeuralNetworkEvolution(layers));
                    }
                    best = networks[i];
                }
            }
            else
            {
                networks.Sort();

                for (int i = 0; i < networks.Count / 2; i++)
                {


                    //networks[i] = new NeuralNetworkEvolution(networks[i + networks.Count / 2]);
                    //networks[i] = new NeuralNetworkEvolution(networks[networks.Count - 1]);
                    networks[i] = new NeuralNetworkEvolution(best);

                    networks[i].RandomMutate(Random.Range(0.01f,10f)) ;

                    networks[i + networks.Count / 2] = new NeuralNetworkEvolution(networks[i + networks.Count / 2]);
                    // networks[i + networks.Count / 2] = new NeuralNetworkEvolution(networks[networks.Count  - 1]);
                }
            }


            generation++;

            isTraining = true;

            Invoke("LessonEnd", trainingTime);

            //Debug.Log("End");

            InitAgents();

            return;
        }

    }


    public void UpdateBest()
    {
        for (int i = 0; i < networks.Count; i++)
        {
            if (networks[i].GetFitness() > best.GetFitness())
            {
                best = new NeuralNetworkEvolution(networks[i]);
                best.SetFitness(networks[i].GetFitness());
            }
        }
    }

    void LessonEnd()
    {

        foreach (GameObject obj in agents)
        {
            //obj.GetComponent<MO_Trainer>().EndReward();
            Destroy(obj);
        }


        for (int i = 0; i < foodCount; i++)
        {
            foods[i].transform.position = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-20f, 20f));

        }

        networks.Sort();

        UpdateBest();

        Debug.Log(networks[0].GetFitness() + " : " + networks[networks.Count - 1].GetFitness() + " : " +  best.GetFitness());

        if (networkData && saveData)
        {
            networkData.layers = best.layers;

            networkData.fitness = best.GetFitness();

            networkData.json = Newtonsoft.Json.JsonConvert.SerializeObject(best.weights);

            networkData.generation = generation;
        }

        isTraining = false;

        //food.transform.position = new Vector3(Random.Range(-17f, 17f), 0, Random.Range(-11f, 11f));
    }

    void InitAgents()
    {
        //networks.Clear();
        agents.Clear();
        agentTrainer.Clear();


        for (int i = 0; i < agentCount; i++)
        {
            //networks.Add(new NeuralNetworkEvolution(layers));
            agents.Add(Instantiate(agentPrefab));

            MO_Trainer m = agents[i].GetComponent<MO_Trainer>();
            m.Init(networks[i], foods, this);
            agentTrainer.Add(m);



            agents[i].transform.position = new Vector3(Random.Range(-17f, 17f), 0, Random.Range(-11f, 11f));

        }
    }
}
