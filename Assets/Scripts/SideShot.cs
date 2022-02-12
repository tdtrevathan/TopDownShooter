using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideShot : MonoBehaviour
{
    [SerializeField] float offsetSpeed = 5f;
    Enemy enemy;
    bool leftRight;
    // Update is called once per frame

    private void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        leftRight = enemy.GetLeftRight();
    }
    void Update()
        {
         if (leftRight == false)
            {
                transform.position += transform.TransformDirection(Random.Range(offsetSpeed / 2 * Time.deltaTime, offsetSpeed * Time.deltaTime), 0, 0);
            }
         else
            {
                transform.position += transform.TransformDirection(Random.Range(-offsetSpeed / 2 * Time.deltaTime, -offsetSpeed * Time.deltaTime), 0, 0);
            }

        }
    
}
