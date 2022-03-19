using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Trainer : MonoBehaviour
{
    public int id;
    MO_NetworkManager manager;
    NeuralNetworkEvolution network;
    Transform food;
    [SerializeField]
    float speed = .2f;
    [SerializeField]
    float rotSpeed = .2f;
    Rigidbody rb;
    float fitness;
    [SerializeField]
    float lifetime = 100;
    [SerializeField]
    float time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    [SerializeField]
    

    // Update is called once per frame
    void Update()
    {



    }

    private void ResetAgent()
    {
        time = lifetime;
        manager.UpdateBest();
        network.RandomMutate(0.1f);
        network.SetFitness(0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(Random.Range(-17f, 17f), 0, Random.Range(-11f, 11f));
    }

    float lastDistance;
    float distance;
    private void FixedUpdate()
    {
        time -= Time.deltaTime;

        if(time <= 0)
        {
            ResetAgent();
            return;
        }

        float[] input = new float[4];

        Debug.DrawLine(transform.position, transform.position + transform.forward * 5f, Color.red, 0);



        input[0] = transform.position.x;

        input[1] = transform.position.z;



        //RaycastHit hit_forward;
        //RaycastHit hit_left;
        //RaycastHit hit_right;
        //RaycastHit hit_back;

        /*
        if (Physics.Raycast(transform.position, transform.forward, out hit_forward, 5f))
        {
            input[2] = hit_forward.distance;
        }
        else
        {
            input[2] = -1;
        }
        */
/*
        if (Physics.Raycast(transform.position,transform.right, out hit_left, 5f))
        {
            input[4] = hit_left.distance;
        }
        else
        {
            input[4] = 5;
        }

        if (Physics.Raycast(transform.position,-transform.right, out hit_right, 5f))
        {
            input[5] = hit_right.distance;
        }
        else
        {
            input[6] = 5;
        }*/


        Vector3 v = GetNearestFood();

        input[2] = v.x;
        input[3] = v.z;
        //input[5] = v.y;


        //Debug.Log(v);



        float[] output = network.FeedForward(input);

        //Vector2 velocity = Vector2.zero;

        //velocity.x = output[0] * speed;

        //velocity.y = output[1] * speed;



        rb.velocity += transform.forward * Mathf.Abs(output[0]) * speed;

        rb.angularVelocity = new Vector3(0, output[1] * rotSpeed, 0);


        fitness -= Mathf.Abs(rb.angularVelocity.y);
        //fitness -= v.y * 10;

        //fitness += Time.deltaTime;

        network.AddFitness(Time.deltaTime - v.y / 10);

        fitness = 0;


        //transform.position += transform.forward * output[0] * speed;

        //transform.Rotate(new Vector3(0, output[1] * rotSpeed, 0));

    }

    Vector3 GetNearestFood()
    {
        Vector3 nearest = Vector3.zero;
        float dist = -1;
        foreach(GameObject f in foods)
        {
            if(Vector3.Distance(transform.position, f.transform.position) < dist || dist == -1)
            {
                nearest = f.transform.position;
                dist = Vector3.Distance(transform.position, f.transform.position);
            }
        }
        return new Vector3(nearest.x, dist, nearest.z);
    }

   
    public void ResetTime()
    {
        time = lifetime;
    }
    /*
   private void OnCollisionStay(Collision collision)
   {
       fitness -= 1;

   }

   public void EndReward()
   {
       if (distance < 1f) network.AddFitness(500);
   }

   */

    List<GameObject> foods;
    public void Init(NeuralNetworkEvolution _network, List<GameObject> _foods, MO_NetworkManager _manager)
    {
        this.network = _network;
        this.foods = _foods;
        this.manager = _manager;
    }
}
