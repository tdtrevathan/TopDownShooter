using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    [SerializeField] float health = 100;
    
    [SerializeField] float explosionDuration = 0.5f;
    [SerializeField] GameObject explosionParticles;

    [Header("Projectiles")]
    [SerializeField] GameObject standardShotPrefab;
    [SerializeField] GameObject spinnerShotPrefab;
    [SerializeField] GameObject sideShotPrefab;
    [SerializeField] float shotCounter; // serialized for debug
    [SerializeField] float sideShotCounter; // serialized for debug
    [SerializeField] float burstIntermissionCounter; // serialized for debug
    [SerializeField] float burstDurationCounter; // serialized for debug
    [SerializeField] float laserIntervalCounter; // serialized for debug
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 0.3f;
    [SerializeField] float minTimeBetweenSideShots = 0.5f;
    [SerializeField] float maxTimeBetweenSideShots = 2f;
    [SerializeField] float minTimeBetweenBursts = 3f;
    [SerializeField] float maxTimeBetweenBursts = 5f;
    [SerializeField] float minTimeBetweenLaser = 3f;
    [SerializeField] float maxTimeBetweenLaser = 5f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float spinnerProjectileSpeed = 7f;
    [SerializeField] float sideShotProjectileSpeed = 12f;

    

    [Header ("Audio")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip spinnerShotSound;
    [SerializeField] AudioClip sideShotSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float shotVolume = 0.5f;

    [SerializeField] int scoreValue = 10000;

    int damage = 10;
    float scrapeDuration = 3f;
    float maxHealth;
    bool fireLeftRight = false; // left = false/right = true
    bool laserSwitch = false;
    int laserSwitchVal;


    
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;

        //set projectile timers
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        sideShotCounter = UnityEngine.Random.Range(minTimeBetweenSideShots, maxTimeBetweenSideShots);
        burstIntermissionCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
        burstDurationCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
        laserIntervalCounter = UnityEngine.Random.Range(minTimeBetweenLaser, maxTimeBetweenLaser);
    }

    // Update is called once per frame
    void Update()
    {
        
        //decide which paterns the boss will follow
        if (health >= maxHealth / 2)
        {
            FireStandard();
        }
        else if(health >= maxHealth / 4 && health <= maxHealth / 2)
        {
            BurstFireIntermission();
        }
        else if(health <= maxHealth / 8)
        {
            FinalIntervalIntermission();
        }
       
    }

  //Getters
    public int GetDamage() { return damage; }
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetScrapeDuration() { return scrapeDuration; }
    public bool GetLeftRight() { return fireLeftRight; }

    //shots
    private void FireStandard()
    {
        if (laserSwitch == false)
        {
            GameObject laserRight = Instantiate(standardShotPrefab, transform.position + new Vector3(1.65f, -1.5f, 0), Quaternion.identity) as GameObject;
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

            GameObject laserLeft = Instantiate(standardShotPrefab, transform.position + new Vector3(-1.65f, -1.5f, 0), Quaternion.identity) as GameObject;
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        }
        if (laserSwitch == true)
        {
            GameObject laserRight = Instantiate(standardShotPrefab, transform.position + new Vector3(0.75f, 1.5f, 0), Quaternion.identity) as GameObject;
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            GameObject laserLeft = Instantiate(standardShotPrefab, transform.position + new Vector3(-0.75f, 1.5f, 0), Quaternion.identity) as GameObject;
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        }
    }

    private void FireSpinShot()
    {
        if (fireLeftRight == false)
        {
            GameObject laserRight = Instantiate(spinnerShotPrefab, transform.position + new Vector3(0.75f, 0f, 0), Quaternion.identity) as GameObject;
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -spinnerProjectileSpeed);
            
            fireLeftRight = true;
        }
        else
        { 
            GameObject laserLeft = Instantiate(spinnerShotPrefab, transform.position + new Vector3(-0.75f, 0f, 0), Quaternion.identity) as GameObject;
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -spinnerProjectileSpeed);
           
            fireLeftRight = false;
        }

        AudioSource.PlayClipAtPoint(spinnerShotSound, Camera.main.transform.position, shotVolume);


    }

    private void FireSideShot()
    {
        if (fireLeftRight == false)
        {
            GameObject laserRight = Instantiate(sideShotPrefab, transform.position + new Vector3(1.25f, 0f, 0), Quaternion.identity) as GameObject;
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -sideShotProjectileSpeed);

            fireLeftRight = true;
        }
        else
        {
            GameObject laserLeft = Instantiate(sideShotPrefab, transform.position + new Vector3(-1.25f, 0f, 0), Quaternion.identity) as GameObject;
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -sideShotProjectileSpeed);

            fireLeftRight = false;
        }

        AudioSource.PlayClipAtPoint(sideShotSound, Camera.main.transform.position, shotVolume);

    }

    //shot loops
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            FireSpinShot();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        }
    }

    private void CountDownAndShootSideShot()
    {
        sideShotCounter -= Time.deltaTime;
        if (sideShotCounter <= 0f)
        {
            FireSideShot();
            sideShotCounter = UnityEngine.Random.Range(minTimeBetweenSideShots, maxTimeBetweenSideShots);

        }
    }

    private void BurstFireIntermission()
    {

        burstIntermissionCounter -= Time.deltaTime;

        if (burstIntermissionCounter >= 0)
        {
            CountDownAndShootSideShot();
        }

        if (burstIntermissionCounter <= 0f)
        {
            burstDurationCounter -= Time.deltaTime;
            CountDownAndShoot();

            if(burstDurationCounter <= 0f)
            {

                burstIntermissionCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
                burstDurationCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
            }

        }
    }

    private void FinalIntervalIntermission()
    {
        
        burstIntermissionCounter -= Time.deltaTime;

        if (burstIntermissionCounter >= 0)
        {
            CountDownAndShootSideShot();
        }

        if (burstIntermissionCounter <= 0f)
        {
            burstDurationCounter -= Time.deltaTime;
            CountDownAndShoot();

            if (burstDurationCounter <= 0f)
            {
                laserIntervalCounter -= Time.deltaTime;
                FireStandard();

                if (laserIntervalCounter <= 0f)
                {
                    laserSwitchVal = UnityEngine.Random.Range(0,2);

                    if (laserSwitchVal == 0)
                    {
                        laserSwitch = false;
                    }
                    else
                    {
                        laserSwitch = true;
                    }
                    burstIntermissionCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
                    burstDurationCounter = UnityEngine.Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
                    laserIntervalCounter = UnityEngine.Random.Range(minTimeBetweenLaser, maxTimeBetweenLaser);

                }
            }

        }
    }


    //collsion
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    //damage
    private void ProcessHit(DamageDealer damageDealer)
    {

        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            DestroyShip();
            
        }
    }


    //ship death
    private void DestroyShip()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        FindObjectOfType<LevelLoading>().LoadWin();
        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
        
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(explosionClip, Camera.main.transform.position, deathSoundVolume);
    }
}
