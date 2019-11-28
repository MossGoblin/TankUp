using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData
{
    // components
    private Stack<LayerData> layers;
    public float Durability;

    // modifiers
    public float SpeedFactor { get; private set; }
    public float RotationFactor { get; private set; }
    public float SizeFactor { get; private set; }
    public float DamageFactor { get; private set; }

    public TankData(LayerData startingLayer)
    {
        layers = new Stack<LayerData>();
        layers.Push(startingLayer);
        Durability = (startingLayer.Uses*100/3);
        UpdateModifiers();
    }

    // add layer
    public void AddLayer(LayerData layer)
    {
        layers.Push(layer);
        UpdateModifiers();
    }

    public LayerData DropLayer()
    {
        LayerData droppedLayer = null;
        if (layers.Count > 0)
        {
            droppedLayer = layers.Pop();
        }

        if (layers.Count > 0)
        {
            UpdateModifiers();
        }
        else
        {
            Debug.Log("Last layer dropped - WE DED");
        }

        return droppedLayer;
    }

    public void UseUp()
    {
        if (!layers.Peek().UseUp())
        {
            layers.Pop();
            UpdateModifiers();
        }
        // if the layer has been used, destroy it and update above
    }

    public bool TakeDamage(float damage)
    {
        Durability -= damage;

        if (Durability <= 0)
        {
            // send a message that a layer is lost
            return false;
        }
        // a layer is NOT lost
        return true;
    }

    public WeaponTypes CurrentWeapon()
    {
        return layers.Peek().WeaponType;
    }

    public int UsesLeft()
    {
        return layers.Peek().Uses;
    }

    public int LayersCount()
    {
        return layers.Count;
    }

    private void UpdateModifiers()
    {
        // size is proportional to the number of layers
        // the speed goes down with number of different layers
        // the rotation speed goes down with the number of layers that are the same as the outermost
        // the damage goes up with the number of layers that are the same as the outermost

        SizeFactor = layers.Count; // each will probably give about 5%
        
        int currentWeaponCount = 0; // number of layers same as the current layer
        // int numberOfTypes = Enum.GetNames(typeof(WeaponTypes)).Length; // why do I need that now
        WeaponTypes currentWeapon = layers.Peek().WeaponType;
        LayerData[] layerArray = layers.ToArray();
        List<LayerData> diffLayers = new List<LayerData>();
        for (int count = 0; count < layerArray.Length; count++)
        {
            if (!diffLayers.Contains(layerArray[count]))
            {
                diffLayers.Add(layerArray[count]);
            }
            if (layerArray[count].WeaponType == CurrentWeapon())
            {
                currentWeaponCount++;
            }
        }
        int numberOfDiffLayers = diffLayers.Count;

        SpeedFactor = numberOfDiffLayers;
        RotationFactor = currentWeaponCount;
        DamageFactor = currentWeaponCount;
    }

}
