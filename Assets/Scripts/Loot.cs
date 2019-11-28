using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // refs
    [SerializeField] GameMaster master;
    public LayerData LayerData { get; private set; }

    GameObject lastOwner;

    float lifeSpan = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        master = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        // LayerData = master.GenerateRandomLayer();
    }

    public void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }


    public void SetLayerData(LayerData layer)
    {
        LayerData = layer;
        UpdateColor();
    }

    private void UpdateColor()
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = master.tempColorScheme[(int)LayerData.WeaponType];
    }
}
