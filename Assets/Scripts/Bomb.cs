using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float LifeSpan;
    public float Damage;

    [SerializeField] GameMaster master;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        LifeSpan = 3f;
        Damage = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (LifeSpan > 0)
        {
            LifeSpan -= Time.deltaTime;
        }
        if (LifeSpan <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
            GameObject.Destroy(transform.gameObject); // too soon?


            // spawn explosion
            master.SpawnExplosion(transform.position);
            // TODO :: deal damage around
            // create a CircleCollider to check against
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                // if it is an enemy - deal damage
                GameObject colliderGO = collider.transform.gameObject;
                if (colliderGO.tag == "Enemy")
                {
                    colliderGO.GetComponent<Enemy>().TakeDamage(Damage);
                }
            }
    }
    public void SetDamage(float damageFactor)
    {
        Damage = Damage * (1 + damageFactor/20); // each unit of damage factor increases the damage by 5%
    }
}
