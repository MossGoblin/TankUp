using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // refs
    [SerializeField] public PlayerMaster player;
    [SerializeField] public TerrainBuilder terrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LayerData GenerateRandomLayer()
    {
        Array weaponTypes = Enum.GetValues(typeof(WeaponTypes));
        int randomWeaponIndex = UnityEngine.Random.Range(0, 3);
        WeaponTypes randomWeapon = (WeaponTypes)weaponTypes.GetValue(randomWeaponIndex);
        LayerData newLayer = new LayerData(randomWeapon);

        return newLayer;
    }

    public TankData GenerateNewTankData()
    {
        LayerData newLayerData = GenerateRandomLayer();
        TankData newTankData = new TankData(newLayerData);

        return newTankData;
    }
}
