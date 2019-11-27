using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float LifeSpan;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        LifeSpan = 3f;
        Damage = 50;
    }

    // Update is called once per frame
    void Update()
    {
        LifeSpan -= Time.deltaTime;
        if (LifeSpan <= 0)
        {
            // spawn explosion
            GameObject.Destroy(transform.gameObject);
        }
    }

    public void SetDamage(float damageFactor)
    {
        Damage = Damage * (1 + damageFactor/20); // each unit of damage factor increases the damage by 5%
    }
}
