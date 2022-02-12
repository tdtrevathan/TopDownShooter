using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    List<Transform> waypoints;
    [SerializeField] float moveSpeed = 0.2f;
    bool reverseMove = false;
    [SerializeField] int waypointIndex = 1;
    [SerializeField] int waveIndex = 0; //serialize for debug purposes
    

    Enemy enemy;
    float health;
    float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfigs[waveIndex].GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;

        enemy = FindObjectOfType<Enemy>();

        maxHealth = enemy.GetMaxHealth();



    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();

        CheckWaveStatus();

        Move();

        
    }

    private void CheckHealth()
    {
        health = enemy.GetHealth();
        
    }

    private void CheckWaveStatus()
    {
        //decide which paterns the boss will follow
        if (health >= maxHealth / 4 && health <= maxHealth / 2)
        {
            //next wave
            if (waveIndex == 0)
            {
                SetNextWave();
            }
        }
        else if (health <= maxHealth / 8)
           
            //next wave
            if (waveIndex == 1)
            {
                SetNextWave();
            }
    }

    private void SetNextWave()
    {
        
        waypointIndex = 1;
        //move the boss back to starting spot before swap
        var targetPosition = waypoints[waypointIndex].transform.position;
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

        //resest reverseMove
        reverseMove = false;

        if (transform.position == targetPosition)
        {
            if (waveIndex == 0)
            {
                waveIndex = 1;
            }
            else if (waveIndex == 1)
            {
                waveIndex = 2;
            }
            waypoints = waveConfigs[waveIndex].GetWaypoints();      
        }
    }

    private void Move()
    {
        

        if (waypointIndex <= waypoints.Count - 1 && reverseMove == false)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {

                if(waypointIndex < waypoints.Count)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex--;
                }
                
            }

        }
        else
        {
            if (waypointIndex >= 0 && reverseMove == true)
            {
                var targetPosition = waypoints[waypointIndex].transform.position;
                var movementThisFrame = moveSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    if (waypointIndex > 0)
                    {
                        waypointIndex--;
                    }
                    else
                    {
                        waypointIndex++;
                    }
                    
                }

            }
        }

        CalcMoveReversal();
        
    }

    private void CalcMoveReversal()
    {
        if (waypointIndex >= waypoints.Count - 1)
        {
            reverseMove = true;
            
        }
        else if (waypointIndex <= 0)
        {
            reverseMove = false;
        }
    }
}
