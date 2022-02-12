using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWDamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 20;


    public int GetDamage()
    {
        return damage;
    }
}
