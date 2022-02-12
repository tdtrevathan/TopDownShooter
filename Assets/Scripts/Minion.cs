using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField] float health = 50;
    [SerializeField] float shotCounter; // serialized for debug
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 15f;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float explosionDuration = 0.5f;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float shotVolume = 0.75f;
    [SerializeField] int scoreValue = 150;
    int damage = 20;

    [SerializeField] GameObject standardShotPrefab;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip shotSound;


    bool standardShotLeftRightFire = false; // switches standard shot between left and right (Left = false, Right = true)
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

 
    public int GetDamage() { return damage; }

    private void Fire()
    {
        if (standardShotLeftRightFire == false)
        {
            GameObject laserRight = Instantiate(standardShotPrefab, transform.position + new Vector3(0.4f, -0.5f, 0), Quaternion.identity) as GameObject;
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

            standardShotLeftRightFire = true;
            
        }
        else
        {
            GameObject laserLeft = Instantiate(standardShotPrefab, transform.position + new Vector3(-0.4f, -0.5f, 0), Quaternion.identity) as GameObject;
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

            standardShotLeftRightFire = false;
        }

        AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, shotVolume);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {

        health -= damageDealer.GetDamage();
        
        
        if (health <= 0)
        {
            DestroyShip();
        }
    }

    public void DestroyShip()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);

        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);

        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(explosionClip, Camera.main.transform.position, deathSoundVolume);
    }
}
