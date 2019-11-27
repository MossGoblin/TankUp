using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // refs
    [SerializeField] GameMaster master;
    public LayerData LayerData { get; private set; }

    GameObject lastOwner;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        LayerData = master.GenerateRandomLayer();
    }

    public void SetOwner(GameObject owner)
    {
        lastOwner = owner;
    }
}
