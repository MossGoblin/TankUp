using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // refs
    [SerializeField] private GameMaster master;
    [SerializeField] private Transform player;

    // what does the enemy do?
    // find the player
    // turn towards the player
    // go towards the player (mind the weapon type)
    // shoot if close enough

    // components and data
    private TankData tankData;
    private float movementSpeed;
    private float rotationSpeed;

    private float shootDelay;
    private float shootInterval;
    bool shotProduced;

    // spacials
    Vector3 directionTowardsPlayer;
    // distance to the player
    float distanceToPlayer;
    // angle to the player
    float rotationX;
    float rotationY;
    float angle;


    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        player = GameObject.Find("PlayerController").transform;

        shootDelay = 2f;

        // Init the enemy with random tank data
        tankData = master.GenerateNewTankData();
        UpdateVisuals();

        movementSpeed = 0.25f; // good speed is 0.75f
        rotationSpeed = 1.75f;
        Debug.Log("Chasing the scum");

        // FIXME : TEMP ALIGN WITH PLAYER
        Vector3 tempStartPosition = new Vector3(24, 21);
        transform.position = tempStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // spatial relationship with the player - will be used for movement and shooting
        // direction to player
        directionTowardsPlayer = (player.position - transform.position);
        // distance to the player
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
        // angle to the player
        rotationX = player.position.x - transform.rotation.x;
        rotationY = player.position.y - transform.rotation.y;
        angle = (Mathf.Atan2(directionTowardsPlayer.y, directionTowardsPlayer.x) * Mathf.Rad2Deg) - 90;

        ProcessMovement();
        ProcessTargeting();

        if (shootInterval <= 0)
        {
            shotProduced = false;
            shootInterval = 0;
        }
        else
        {
            shootInterval -= Time.deltaTime;
        }

        // TODO :: Check for playerrange and shoot
    }

    private void ProcessMovement()
    {
        // have the player; get the vector to him
        // rotate enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

        // simple movement
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void ProcessTargeting()
    {
        // check distance
        if (distanceToPlayer <= 5f && angle <= 30)
        {
            Shoot();
        }
    }

    private void UpdateVisuals()
    {
        int weaponIndex = (int)tankData.CurrentWeapon();
        transform.GetComponentInChildren<SpriteRenderer>().sprite = master.enemySprites[weaponIndex];
    }

    public void TakeDamage(float damage)
    {
        if (!tankData.TakeDamage(damage)) // if we lost a layer as a result of the damage
        {
            // destroy self
            GameObject.Destroy(transform.gameObject); // should this work? destroyed objects are supposed to be collected at the end of the frame


            // Enemies have only one layer
            // Drop that layer
            LayerData droppedlayer = tankData.DropLayer();
            
            // reduce the uses of the dropped layer
            droppedlayer.UseUp();
            
            // check if the layer is used up
            // instantiate new drop with the layer data
            Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject newLoot = Instantiate(master.lootLayerPrefab, positionForLoot, Quaternion.identity);
            newLoot.GetComponent<Loot>().SetLayerData(droppedlayer);
            // temp loot coloring
            newLoot.GetComponentInChildren<SpriteRenderer>().color = master.tempColorScheme[(int)droppedlayer.WeaponType];
        }
    }

    private void Shoot()
    {
        if(!shotProduced)
        {
            // produce shot
            shotProduced = true;
            shootInterval = shootDelay;
            // spawn a bullet
            GameObject fireball = master.SpawnFireball(transform.position, transform.rotation);
            fireball.GetComponent<Fireball>().SetUpStats(3, 15, 0.5f, "Player");
        }
    }
}

