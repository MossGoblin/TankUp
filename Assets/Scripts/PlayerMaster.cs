using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    // temp refs
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject bombPrefab;
    
    //refs
    [SerializeField] GameMaster master;
    [SerializeField] TankData tankData;

    // components and modifiers

    private float durability;
    private WeaponTypes weapon;
    private float speedFactor;
    private float rotationFactor;
    private float sizeFactor;
    private float damageFactor;

    private float sizeFactorWeight = 0.05f;
    private float currentSizeModifier = 0;
    private float speed = 5;
    private float rotationSpeed = 70;


    // Start is called before the first frame update
    void Start()
    {

        // generate tank data
        tankData = master.GenerateNewTankData();

        // test layer
        LayerData testSecondLayer = master.GenerateRandomLayer();
        tankData.AddLayer(testSecondLayer);


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
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = - Input.GetAxis("Horizontal") * rotationSpeed;

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
        
        sizeFactor = tankData.SizeFactor * 0.05f;
        damageFactor = tankData.DamageFactor;
        weapon = tankData.CurrentWeapon();

        if (currentSizeModifier != tankData.SizeFactor)
        {
            currentSizeModifier = tankData.SizeFactor;
            UpdateSize(currentSizeModifier * sizeFactorWeight);
        }
    }

    private void UpdateSize(float sizeFactor)
    {
        Vector3 newScale = new Vector3(transform.localScale.x * (1 + sizeFactor), transform.localScale.y * (1 + sizeFactor), transform.localScale.z);
        transform.localScale = newScale;
    }

    private void HandleTempInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject newLoot = Instantiate(coinPrefab, positionForLoot, Quaternion.identity);
            newLoot.GetComponent<Loot>().SetOwner(master.transform.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Vector3 positionForBomb = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject newBomb = Instantiate(bombPrefab, positionForBomb, Quaternion.identity);
        }
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
            // TODO : HERE
            // we hit a bomb
            Debug.Log("We hit a bomb");
            if (!tankData.TakeDamage(100)) // if we lost a layer as a result of the damage
            {
                LayerData droppedlayer = tankData.RemoveLayer();
                // check if the layer is used up
                if (droppedlayer.Uses > 0)
                {
                    // instantiate new drop with the layer data
                    // for test drop the lew loot in behind the player
                    Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
                    GameObject newLoot = Instantiate(coinPrefab, positionForLoot, Quaternion.identity);
                    newLoot.GetComponent<Loot>().SetOwner(master.transform.gameObject);
                }
            }
        }
    }

}
