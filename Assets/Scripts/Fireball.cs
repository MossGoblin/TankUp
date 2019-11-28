using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    // stats
    float movementSpeed = 1.5f;
    float lifeSpan = 3f;
    float damage = 0f;
    float explosionRadius = 0f;

    string targetTag = "";

    // refs
    GameMaster master;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update() // if lifetime elapses - blow up for larger radius and smaller damage
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            GameObject.Destroy(transform.gameObject); // too soon?
            GameObject explosion = master.SpawnExplosion(transform.position);
            explosion.GetComponent<Explosion>().SetUpStats(explosionRadius, damage, targetTag);
        }
    }

    public void SetUpStats(float lifeSpan, float damage, float explosionRadius, string target)
    {
        this.lifeSpan = lifeSpan;
        this.damage = damage * 0.75f;
        this.explosionRadius = explosionRadius;
        this.targetTag = target;
    }

    private void OnCollisionEnter2D(Collision2D other) // if we hit a player - give damage and die
    {
        if (other.gameObject.tag == targetTag)
        {
            GameObject.Destroy(transform.gameObject);
            other.gameObject.GetComponent<PlayerMaster>().TakeDamage(damage);
        }    
    }
}
