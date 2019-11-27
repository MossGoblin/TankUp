using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerData
{
    // weapon type
    // uses
    public WeaponTypes WeaponType { get; private set; }
    public int Uses { get; private set; }
    public LayerData(WeaponTypes wt)
    {
        this.WeaponType = wt;
        this.Uses = 3;
    }

    public bool UseUp()
    {
        Uses--;
        if (Uses > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        // TODO : WARN if the layer has been used up
    }
}
