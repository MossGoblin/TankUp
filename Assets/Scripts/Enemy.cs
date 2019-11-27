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


        movementSpeed = 0.75f;
        rotationSpeed = 1.75f;
        Debug.Log("Chasing the scum");

        // FIXME : TEMP ALIGN WITH PLAYER
        Vector3 tempStartPosition = new Vector3(49, 42);
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
    }

    private void UpdateVisuals()
    {
        Color[] tempColorScheme = new Color[] {
        new Color(1, 0, 0, 1),
        new Color(0, 1, 0, 1),
        new Color(0, 0, 1, 1)
        };
        transform.GetComponentInChildren<SpriteRenderer>().color = master.player.tempColorScheme[(int)tankData.CurrentWeapon()];
    }
}

