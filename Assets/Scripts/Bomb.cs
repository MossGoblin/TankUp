using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float LifeSpan;
    public float Damage;
    private float radius;
    private string targetTag;

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
            GameObject explosion = master.SpawnExplosion(transform.position);
            explosion.GetComponent<Explosion>().SetUpStats(radius, Damage, targetTag);

    }
    public void SetStats(float radius, float damageFactor, string target)
    {
        Damage = Damage * (1 + damageFactor/20); // each unit of damage factor increases the damage by 5%
        this.radius = radius;
        targetTag = target;
    }
}
