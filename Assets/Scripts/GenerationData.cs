using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Network Save Data", menuName = "ScriptableObjects/NNSaveData", order = 1)]
public class GenerationData : ScriptableObject
{

    //public float[][][] weights;
    //public NeuralNetworkEvolution network;
    public int[] layers;
    public string json;
    public float fitness;
    public int generation;
    //public SerializableNeuralNetwork net;
}
