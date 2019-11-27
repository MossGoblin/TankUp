using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // refs
    [SerializeField] GameMaster master;
    LayerData layerData;
    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        layerData = master.GenerateRandomLayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
