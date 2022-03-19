using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    int health = 100;

    public int Damage(int damage)
    {
        int h = health -= damage;
        if (h <= 0) health = 100;
        return h;
    }

    public void Heal(int health)
    {
        health += health;
    }


    public void Reset()
    {
        health = 100;
    }

}
