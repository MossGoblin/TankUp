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
        UpdateColor();
    }

    public void SetOwner(GameObject owner)
    {
        lastOwner = owner;
    }

    public void SetLayerData(LayerData layer)
    {
        LayerData = layer;
        UpdateColor();
    }

    private void UpdateColor()
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = master.player.tempColorScheme[(int)LayerData.WeaponType];
    }
}
