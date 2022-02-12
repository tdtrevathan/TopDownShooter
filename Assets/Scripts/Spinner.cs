using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speedOfSpin = 540f;

    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Random.Range(speedOfSpin / 2 * Time.deltaTime, speedOfSpin * Time.deltaTime));
    }
}
