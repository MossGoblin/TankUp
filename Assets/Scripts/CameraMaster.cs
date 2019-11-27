using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaster : MonoBehaviour
{
    // refs
    [SerializeField] GameMaster master;

    // Start is called before the first frame update
    void Start()
    {
        float playerX = master.player.transform.position.x;
        float playerY = master.player.transform.position.y;
        Vector3 newPosition = new Vector3(playerX, playerY, -10);
        transform.position = newPosition;
        Debug.Log($"Camera set up at {transform.position.x} / {transform.position.y}");
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = master.player.transform.position.x;
        float playerY = master.player.transform.position.y;
        Vector3 newPosition = new Vector3(playerX, playerY, -10);
        transform.position = newPosition;
    }

}
