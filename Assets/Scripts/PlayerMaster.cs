using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    // temp refs
    [SerializeField] GameObject lootLayerPrefab;
    [SerializeField] GameObject bombPrefab;
    
    //refs
    [SerializeField] GameMaster master;
    [SerializeField] TankData tankData;

    // components and modifiers -- some are temporarily Seralized for visibility in the Inspector for debugging properties
    Vector3 originalLocalScale;
    [SerializeField] private float durability;
    [SerializeField] private WeaponTypes weapon;
    [SerializeField] private float speedFactor;
    [SerializeField] private float rotationFactor;
    [SerializeField] private float sizeFactor;
    [SerializeField] private float damageFactor;

    private float sizeFactorWeight = 0.05f;
    private float speed = 5;
    private float rotationSpeed = 95;

    // visual references
    [SerializeField] Transform bullseye;

    // Start is called before the first frame update
    void Start()
    {

        // generate tank data
        tankData = master.GenerateNewTankData();

        // // test layer
        // LayerData testSecondLayer = master.GenerateRandomLayer();
        // tankData.AddLayer(testSecondLayer);

        // keep original size
        originalLocalScale = transform.localScale;
        
        // position player
        int startPosX = master.terrain.Width;
        int startPosY = master.terrain.Height;
        this.transform.Translate(new Vector3(startPosX, startPosY, 0));
        Debug.Log($"Player position set up to {transform.position.x} / {transform.position.y} / {transform.position.z}");
        durability = tankData.Durability;
        // get movement modifiers
        UpdateState();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        HandleMovement();
        HandleTempInput();
    }

    private void HandleMovement()
    {
        float translation = Input.GetAxis("Vertical") * speed * (1 + speedFactor/100);
        float rotation = - Input.GetAxis("Horizontal") * rotationSpeed * (1 + rotationFactor/100);

        // reverse
        if (Input.GetAxis("Vertical") < 0)
        {
            translation = translation / 2;
        }

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, rotation);
    }

    private void UpdateState()
    {
        speedFactor = tankData.SpeedFactor;
        rotationFactor = tankData.RotationFactor;
        
        sizeFactor = tankData.SizeFactor;
        damageFactor = tankData.DamageFactor;
        weapon = tankData.CurrentWeapon();


        UpdateSize(sizeFactor * sizeFactorWeight);

        TempColor();
    }

    private void UpdateSize(float sizeFactor)
    {
        Vector3 updatedScale = new Vector3(originalLocalScale.x * (1 + sizeFactor), originalLocalScale.y * (1 + sizeFactor), originalLocalScale.z);
        transform.localScale = updatedScale;
    }

    private void HandleTempInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject newLoot = Instantiate(lootLayerPrefab, positionForLoot, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(weapon);
        }
    }

    private void TempColor()
    {
        // TODO : TEMP player coloring
        int weaponIndex = Array.IndexOf(Enum.GetValues(weapon.GetType()), weapon);
        transform.GetComponentInChildren<SpriteRenderer>().color = master.tempColorScheme[weaponIndex];
    }

    // collision handling
    private void OnCollisionEnter2D(Collision2D other)
    {
        // check if we hit a temp loot
        if (other.gameObject.layer == 9)
        {
            // we hit loot
            Debug.Log("We hit loot");
            // get loot data; modify player
            LayerData layerFromLoot = other.gameObject.GetComponent<Loot>().LayerData;
            tankData.AddLayer(layerFromLoot);
            UpdateState();
            // destroy loot
            GameObject.Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 10)
        {
            // we hit a bomb
            Debug.Log("We hit a bomb");

            // if (!tankData.TakeDamage(50)) // if we lost a layer as a result of the damage
            // {
            //     // check if this is our last layer
            //     if (tankData.LayersCount() == 1)
            //     {
            //         // can not destroy last layer
            //         // TODO :: Elaborate when last layer is desrtoyed
            //         Debug.Log("Last layer busted");
            //         GameObject.Destroy(transform.gameObject);
            //         // TODOFIXME : Camera still searching for the player
            //     }
            //     else
            //     {
            //         LayerData droppedlayer = tankData.RemoveLayer();
            //         // reduce the uses of the dropped layer
            //         droppedlayer.UseUp();
            //         // check if the layer is used up
            //         if (droppedlayer.Uses > 0)
            //         {
            //             // instantiate new drop with the layer data
            //             // for test drop the lew loot in behind the player
            //             Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            //             GameObject newLoot = Instantiate(coinPrefab, positionForLoot, Quaternion.identity);
            //             newLoot.GetComponent<Loot>().SetLayerData(droppedlayer);
            //             // temp loot coloring
            //             newLoot.GetComponentInChildren<SpriteRenderer>().color = tempColorScheme[(int)droppedlayer.WeaponType];
            //         }
            //         UpdateState();
            //     }
            // }
            
            // take half damage from the bomb
            TakeDamage(other.gameObject.GetComponent<Bomb>().Damage / 2);

            // destroy the bomb
            GameObject.Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
            if (!tankData.TakeDamage(damage)) // if we lost a layer as a result of the damage
            {
                // check if this is our last layer
                if (tankData.LayersCount() == 1)
                {
                    // can not destroy last layer
                    // TODO :: Elaborate when last layer is desrtoyed
                    Debug.Log("Last layer busted");
                    GameObject.Destroy(transform.gameObject);
                    // TODOFIXME : Camera still searching for the player
                }
                else
                {
                    LayerData droppedlayer = tankData.DropLayer();
                    // reduce the uses of the dropped layer
                    droppedlayer.UseUp();
                    // check if the layer is used up
                    if (droppedlayer.Uses > 0)
                    {
                        // instantiate new drop with the layer data
                        // for test drop the lew loot in behind the player
                        Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                        GameObject newLoot = Instantiate(lootLayerPrefab, positionForLoot, Quaternion.identity);
                        newLoot.GetComponent<Loot>().SetLayerData(droppedlayer);
                        // temp loot coloring
                        newLoot.GetComponentInChildren<SpriteRenderer>().color = master.tempColorScheme[(int)droppedlayer.WeaponType];
                    }
                    UpdateState();
                }
            }

    }
    

    private void Shoot(WeaponTypes weapon)
    {
        // currently we use only one shooting mode - for Mortar
        // get the position of the billseye
        Vector3 positionForBomb = bullseye.position;
        GameObject newBomb = Instantiate(bombPrefab, positionForBomb, Quaternion.identity);
        newBomb.GetComponent<Bomb>().SetStats(0.5f, damageFactor, "Enemy");
    }

}
