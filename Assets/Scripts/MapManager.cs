using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    Map map;

    [SerializeField]
    int obstacleCount;

    public Vector3 agentStart;
    public Vector2Int mapSize;

    [SerializeField]
    Transform target;

    [SerializeField]
    Transform tank;

    [SerializeField]
    List<Obstacle> obstaclePrefabs = new List<Obstacle>();

    List<Obstacle> obstacles = new List<Obstacle>();
    List<GameObject> obstacleObjects = new List<GameObject>();

    private void Awake()
    {

        map = new Map(mapSize.x, mapSize.y);

        ResetTank();

        ResetTarget();

        //Set Opstacles
        for (int i = 0; i < obstacleCount; i++)
        {

            Obstacle obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count - 1)];

            for (int j = 0; j < 15; j++)
            {

                int x = Random.Range(0, mapSize.x - obstacle.size.x);

                int y = Random.Range(mapSize.y / 6, mapSize.y - obstacle.size.y - mapSize.y / 6);


                Vector2Int newPos = new Vector2Int(x,y);

                Debug.Log(newPos);
                if (map.Place(newPos.x, newPos.y, obstacle.size.x, obstacle.size.y))
                {
                    GameObject g = Instantiate(obstacle.prefab, transform);

                    Vector3 p = new Vector3(newPos.x, -1, newPos.y);

                    g.transform.localPosition = p;

                    obstacles.Add(obstacle);

                    obstacleObjects.Add(g);

                    
                    break;
                }
            }
        }




    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
        }
    }

    bool spawnTop;
    void ResetTarget()
    {
        for (int j = 0; j < 15; j++)
        {
            int x = Random.Range(0, mapSize.x - 2);
            
            int y = Random.Range(0, (int)((mapSize.y - 2) / 4 ));

            if (spawnTop)
            {
                y = Random.Range((int)((mapSize.y - 2) / 4 * 3), mapSize.y - 2);
            }


            Vector2Int newPos = new Vector2Int(x, y);

            if (map.Place(newPos.x, newPos.y, 2, 2))
            {
                target.localPosition = new Vector3(newPos.x + 1, 0, newPos.y + 1);

                break;
            }
        }
    }

    void ResetTank()
    {
        for (int j = 0; j < 15; j++)
        {

            int x = Random.Range(0, mapSize.x - 3);
            int y = Random.Range((int)((mapSize.y - 3) / 4 * 3), mapSize.y - 3);
            if (spawnTop)
            {
                y = Random.Range(0, (int)((mapSize.y - 3) / 4));
            }


            Vector2Int newTankPos = new Vector2Int(x,y);

            if (map.Place(newTankPos.x, newTankPos.y, 3, 3))
            {
                tank.localPosition = new Vector3(newTankPos.x + 1.5f, 0, newTankPos.y + 1.5f);

                tank.localEulerAngles = (Vector3.up * -90);
                break;
            }
        }
    }


    public void ResetMap()
    {
        map.Clear();

        ResetTarget();
        ResetTank();
        spawnTop = !spawnTop;

        //reset Obstacles
        for (int i = 0; i < obstacleObjects.Count; i++)
        {

            for (int j = 0; j < 15; j++)
            {

                int x = Random.Range(0, mapSize.x - obstacles[i].size.x);

                int y = Random.Range(mapSize.y / 6, mapSize.y - obstacles[i].size.y - mapSize.y / 6);


                Vector2Int newPos = new Vector2Int(x, y);

                if (map.Place(newPos.x, newPos.y, obstacles[i].size.x, obstacles[i].size.y))
                {
                    obstacleObjects[i].transform.localPosition = new Vector3(newPos.x, -1, newPos.y);

                    break;
                }
            }

        }






        //restet Target
        //target.localPosition = new Vector3(Random.Range(-mapSize.x, mapSize.x), -1, Random.Range(-mapSize.y/3, mapSize.y));



 



        //Reset Tank
        //tank.localPosition = new Vector3(Random.Range(-mapSize.x, mapSize.x), -1, Random.Range(-mapSize.y, -mapSize.y / 3));
        
    }
}


