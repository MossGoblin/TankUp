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

    public LayerData RemoveLayer()
    {
        if (layers.Count > 0)
        {
            UpdateModifiers();
           return layers.Pop();
        }
        else
        {
            Debug.Log("Only one layer left, can not remove");
            return null;
        }

    }

    public void UseUp()
    {
        layers.Peek().UseUp();
        // if the layer has been used u, destroy it and update above
    }

    public void TakeDamage(float damage)
    {
        Durability -= damage;
        if (Durability <= 0)
        {
            // destroy the layer
            LayerData layerToDrop = RemoveLayer();
            if (layerToDrop.UseUp()) // the layer has not been completely used up
            {
                // spawn loot and attach layer
            }
            else
            {
                // nothing happens (indicate visually that the layer has been used up)
            }

            // update stats for teh new layer stack
            UpdateModifiers();
        }
    }

    public WeaponTypes CurrentWeapon()
    {
        return layers.Peek().WeaponType;
    }

    public int UsesLeft()
    {
        return layers.Peek().Uses;
    }

    private void UpdateModifiers()
    {
        // size is proportional to the number of layers
        // the speed goes down with number of different layers
        // the rotation speed goes down with the number of layers that are the same as the outermost
        // the damage goes up with the number of layers that are the same as the outermost

        SizeFactor = layers.Count - 1; // each will probably give about 5%
        
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
