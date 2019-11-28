using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float tempTime = 0.5f;
    private float radius;
    private float damage;
    private string targetTag;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tempTime -= Time.deltaTime;
        if (tempTime <= 0)
        {
            GameObject.Destroy(transform.gameObject);
        }
        DealDamage();

    }

    private void DealDamage()
    {
            // TODO :: deal damage around
            // create a CircleCollider to check against
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                // if it is an enemy - deal damage
                GameObject colliderGO = collider.transform.gameObject;
                if (colliderGO.tag == targetTag)
                {
                    colliderGO.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
    }

    public void SetUpStats(float radius, float damage, string target)
    {
        this.radius = radius;
        this.damage = damage;
        this.targetTag = target;
    }

}
