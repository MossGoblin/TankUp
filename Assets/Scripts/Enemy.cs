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

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        player = GameObject.Find("PlayerController").transform;

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
        // have the player; get the vector to him
        // rotate enemy
        Vector3 direction = (player.position - transform.position);

        float rotationX = player.position.x - transform.rotation.x;
        float rotationY = player.position.y - transform.rotation.y;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

        // simple movement
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);

        // TODO :: Check for playerrange and shoot
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
}

