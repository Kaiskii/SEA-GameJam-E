using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipData 
{
    public float maxHp;
    public int maxAmmo;
    public float maxFuel;

    public float boostSpeed;
    public float normalSpeed;
    public float turnSpeed;

    public float attackAngleArc;
    public float attackWidth;

    public ShipData(ShipData data)
    {
        this.maxHp = data.maxHp;
        this.maxAmmo = data.maxAmmo;
        this.maxFuel = data.maxFuel;

        this.boostSpeed = data.boostSpeed;
        this.normalSpeed = data.normalSpeed;
        this.turnSpeed = data.turnSpeed;

        this.attackAngleArc = data.attackAngleArc;
        this.attackWidth = data.attackWidth;
    }
}
