using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    // temp refs
    [SerializeField] GameObject coinPrefab;
    
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

    private float speed = 5;
    private float rotationSpeed = 70;


    // Start is called before the first frame update
    void Start()
    {
        // generate tank data
        tankData = master.GenerateNewTankData();
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
        UpdateSize();
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
    }

    private void UpdateSize()
    {
        Vector3 newScale = new Vector3(transform.localScale.x * (1 + sizeFactor), transform.localScale.y * (1 + sizeFactor), transform.localScale.z);
        transform.localScale = newScale;
    }

    private void HandleTempInput()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
        Vector3 positionForLoot = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(coinPrefab, positionForLoot, Quaternion.identity);
        }
    }
}
