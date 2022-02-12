using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Player : MonoBehaviour
{

    //config params

    [Header("Player")]
    [SerializeField] float moveXSpeed = 15f;
    [SerializeField] float moveYSpeed = 5f;
    [SerializeField] float paddingX = 0.5f;
    [SerializeField] float paddingY = 0.25f;
    [SerializeField] int health = 500;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float playerDamageVolume = 0.5f;
    [SerializeField] float explosionDuration = 0.5f;
    [SerializeField] float damageSoundIntermission;
    [SerializeField] float damageSoundDuration = 1f;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip playerDamage;
    [SerializeField] GameObject explosionParticles;
    

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 0.075f;
    [SerializeField] bool leftRightFire = false; // changes the player fire from right (0) to left (1)

    

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        damageSoundIntermission = damageSoundDuration;
        SetUpMoveBoubaries();   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        CheckIfDead();
    }

    public int GetHealth()
    {
        return health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) 
        {
            Minion minion = other.gameObject.GetComponent<Minion>();
            if(!minion)
            {
                return;
            }

            minion.DestroyShip();
            ProcessMinionCrash(minion);
        }
        else
        {
            ProcessHit(damageDealer);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {


        Enemy boss = other.gameObject.GetComponent<Enemy>();
        if (!boss) { return; }

        health -= 1;

        damageSoundIntermission -= Time.deltaTime;

        if (damageSoundIntermission <= 0)
        {
            AudioSource.PlayClipAtPoint(playerDamage, Camera.main.transform.position, playerDamageVolume);
            damageSoundIntermission = damageSoundDuration;
        }

        
    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            DestroyShip();
        }
    }


    private void ProcessMinionCrash(Minion minion)
    {
        health -= minion.GetDamage();

        damageSoundIntermission -= Time.deltaTime;

        if (damageSoundIntermission <= 0)
        {
            AudioSource.PlayClipAtPoint(playerDamage, Camera.main.transform.position, playerDamageVolume);
            damageSoundIntermission = damageSoundDuration;
        }


    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        
        health -= damageDealer.GetDamage();

        damageSoundIntermission -= Time.deltaTime;

        if (damageSoundIntermission <= 0)
        {
            AudioSource.PlayClipAtPoint(playerDamage, Camera.main.transform.position, playerDamageVolume);
            damageSoundIntermission = damageSoundDuration;
        }

        damageDealer.Hit();

        
    }

    private void DestroyShip()
    {
        FindObjectOfType<LevelLoading>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);

        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(explosionClip, Camera.main.transform.position, deathSoundVolume);
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            if (leftRightFire == false)
            {
                GameObject laser = Instantiate(laserPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                yield return new WaitForSeconds(projectileFiringPeriod);

                leftRightFire = true;
            }
            else
            {
                GameObject laser = Instantiate(laserPrefab, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                yield return new WaitForSeconds(projectileFiringPeriod);

                leftRightFire = false;
            }
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime;

        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX * moveXSpeed, xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY * moveYSpeed, yMin, yMax);
        Mathf.Clamp(newXPos, xMin, xMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoubaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingX;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingY;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingY;
    }

  
}
