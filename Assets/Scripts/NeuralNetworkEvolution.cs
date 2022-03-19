using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetworkEvolution : IComparable<NeuralNetworkEvolution>
{

    public int[] layers;
    float[][] neurons;
    public float[][][] weights;
    [SerializeField]
    float fitness;

    public NeuralNetworkEvolution(int[] _layers)
    {
        this.layers = new int[_layers.Length]; //number of layers
        for (int i = 0; i < _layers.Length; i++)
        {
            this.layers[i] = _layers[i]; //number of neurons in layer
        }

        InitNeurons();
        InitWeights();
    }

    public NeuralNetworkEvolution(int[] _layers, float[][][] _weights, float _mutation)
    {
        this.layers = new int[_layers.Length]; //number of layers
        for (int i = 0; i < _layers.Length; i++)
        {
            this.layers[i] = _layers[i]; //number of neurons in layer
        }

        InitNeurons();
        InitWeights();

        CopyWeights(_weights);

        if (_mutation > 0) RandomMutate();

    }


    public NeuralNetworkEvolution(NeuralNetworkEvolution _firstParent, NeuralNetworkEvolution _secondParent)
    {
        this.layers = new int[_firstParent.layers.Length]; //number of layers
        for (int i = 0; i < _firstParent.layers.Length; i++)
        {
            this.layers[i] = _firstParent.layers[i]; //number of neurons in layer
        }

        InitNeurons();
        InitWeights();

        Reproduce(_firstParent, _secondParent);
    }



    public NeuralNetworkEvolution(NeuralNetworkEvolution oldNetwork)
    {
        this.layers = new int[oldNetwork.layers.Length]; //number of layers
        for (int i = 0; i < oldNetwork.layers.Length; i++)
        {
            this.layers[i] = oldNetwork.layers[i]; //number of neurons in layer
        }

        InitNeurons();
        InitWeights();

        CopyWeights(oldNetwork.weights);
    }



    void InitNeurons()
    {
        List<float[]> neuronList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++) // for each layer
        {
            neuronList.Add(new float[layers[i]]); //add neurons in layer to list
        }

        neurons = neuronList.ToArray(); //list to array
    }

    void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>(); //list of weights for each neuron in layers

        //
        for (int i = 1; i < layers.Length; i++) //ignore input leayer(first)
        {
            List<float[]> layerWeightsList = new List<float[]>(); //weights for each neuron in current layer

            int neuronsInPreviousLayer = layers[i - 1];

            //each neuron in current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //weights for current neuron


                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //set random weights 
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights); //add current neurons weights to layer weights list
            }

            weightsList.Add(layerWeightsList.ToArray()); //add current layer weights to to weights list 
        }

        weights = weightsList.ToArray(); //convert to 3D array 
    }


    public float[] FeedForward(float[] _inputs)
    {
        for (int i = 0; i < _inputs.Length; i++) //save values of each input to first layer
        {
            neurons[0][i] = _inputs[i];
            //Debug.Log(neurons[0][i]);
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i - 1].Length; k++)//each neuron in privious layer
                {
                    //throw new Exception("I am an error through new Exception");
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //multiply weights of prv layer neurons with weights of prv layer neurons
                }

                neurons[i][j] = (float)Math.Tanh(value); //activation function (HyperbolicTangtent [-1,1]) 
            }
        }

        return neurons[neurons.Length - 1]; //return last layers neuron values 
    }


    public void AddFitness(float _points)
    {
        fitness += _points;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public void SetFitness(float _value)
    {
        fitness = _value;
    }


    public void Reproduce(NeuralNetworkEvolution _parent1Weights, NeuralNetworkEvolution _parent2Weights)
    {
        for(int i = 0; i < _parent1Weights.weights.Length; i++)
        {
            for (int j = 0; j < _parent1Weights.weights[i].Length; j++)
            {
                for (int k = 0; k < _parent1Weights.weights[i][j].Length; k++)
                {
                    //randomly picks one of the Parents weights 
                    float distribution = _parent1Weights.fitness / (_parent1Weights.fitness + _parent2Weights.fitness); //define chance for passing 

                    float difference = _parent2Weights.weights[i][j][k] - _parent1Weights.weights[i][j][k]; //calculate weight diffrence 

                    float weight = _parent1Weights.weights[i][j][k] + distribution * difference; //get avarage weight form parents based on their fitness 

                    //float pass = UnityEngine.Random.Range(0, 1) > distribution ? _parent1Weights.weights[i][j][k] : _parent2Weights.weights[i][j][k]; //get parent weights by chance

                    weights[i][j][k] = weight + UnityEngine.Random.Range(-0.4f, 0.4f); //add mutation 
                }
            }
        }
    }



    public void RandomMutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //float randomNumber = UnityEngine.Random.Range(0f, 1f) * 1000f;

                    //weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    weight *= UnityEngine.Random.Range(0.8f, 1.2f);
                    //weight += UnityEngine.Random.Range(-100f, 100f) / ; 

                    int a = 1;
                    switch (a)
                    {
                        case int n when (n < 3):

                            break;
                        case int n when (n >= 7):

                            break;
                    }



                    weights[i][j][k] = weight;
                }
            }
        }
    }


    public void RandomMutate2()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //float randomNumber = UnityEngine.Random.Range(0f, 1f) * 1000f;

                    //weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    weight *= UnityEngine.Random.Range(0.8f, 1.2f);
                    //weight += UnityEngine.Random.Range(-100f, 100f) / ; 

                    weights[i][j][k] = weight;
                }
            }
        }
    }
    public void RandomMutate(float max)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    weight += UnityEngine.Random.Range(max * -1000f, max * 1000f) / 1000f;

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void CopyWeights(float[][][] _weights)
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    weights[i][j][k] = _weights[i][j][k];
                }
            }
        }
    }




    public int CompareTo(NeuralNetworkEvolution other)
    {
        if (other == null)
            return 1;
        if (fitness > other.GetFitness())
            return 1;
        else if (fitness < other.GetFitness())
            return -1;
        else
            return 0;
    }


    public SerializableNeuralNetwork SendWeightsToSave()
    {
        SerializableNeuralNetwork snn = new SerializableNeuralNetwork(weights, fitness, this);
        return snn;
    }

}



[System.Serializable]
public class SerializableNeuralNetwork
{
    public float[][][] weights;//matrice des poids
    public float fitness;       //fitness du reseau
    public NeuralNetworkEvolution net;

    public SerializableNeuralNetwork(float[][][] patternWeights, float patternFitness, NeuralNetworkEvolution _net)
    {
        weights = patternWeights;
        fitness = patternFitness;
        net = new NeuralNetworkEvolution(_net.layers);
        net.CopyWeights(_net.weights);
    }
}