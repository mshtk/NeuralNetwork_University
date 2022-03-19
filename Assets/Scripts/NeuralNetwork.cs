using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{

    int[] layers;
    float[][] neurons;
    float[][][] weights;
    float fitness;

    public NeuralNetwork(int[] _layers)
    {
        this.layers = new int[_layers.Length]; //number of layers
        for(int i = 0; i < _layers.Length; i++)
        {
            this.layers[i] = _layers[i]; //number of neurons in layer
        }

        InitNeurons();
        InitWeights();
    }

    void InitNeurons()
    {
        List<float[]> neuronList = new List<float[]>(); 

        for(int i = 0; i < layers.Length; i++) // for each layer
        {
            neuronList.Add(new float[layers[i]]); //add neurons in layer to list
        }

        neurons = neuronList.ToArray(); //list to array
    }

    void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>(); //list of weights for each neuron in layers
        
        //
        for(int i = 1; i < layers.Length; i++) //ignore input leayer(first)
        {
            List<float[]> layerWeightsList = new List<float[]>(); //weights for each neuron in current layer

            int neuronsInPreviousLayer = layers[i - 1];

            //each neuron in current layer
            for(int j = 0; j < neurons.Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //weights for current neuron


                for(int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //set random weights 
                    neuronWeights[k] = UnityEngine.Random.Range(-1, 1);
                }

                layerWeightsList.Add(neuronWeights); //add current neurons weights to layer weights list
            }

            weightsList.Add(layerWeightsList.ToArray()); //add current layer weights to to weights list 
        }

        weights = weightsList.ToArray(); //convert to 3D array 
    }


    public float[] FeedForward(float[] _inputs)
    {
        for(int i = 0; i < _inputs.Length; i++) //save values of each input to first layer
        {
            neurons[0][i] = _inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f; 

                for(int k = 0; k < neurons[i-1].Length; k++)//each neuron in privious layer
                {
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


    public void Reproduce()
    {

    }

}
