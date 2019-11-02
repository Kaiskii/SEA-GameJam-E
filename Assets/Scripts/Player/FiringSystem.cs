using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringSystem : MonoBehaviour
{
    #region Testing
    public int maxAmmo;
    public int ammoRemaining;

    public int reloadAmount;
    #endregion Testing


    public void Reload()
    {
        ammoRemaining += reloadAmount;
        ammoRemaining = (ammoRemaining > maxAmmo) ? maxAmmo : ammoRemaining; 
    }

    public void Fire()
    {
        if(true) // Check if in hitbox
        {
            // Damage
        }
    }

    public void Damage(/*Reference target*/)
    {
        // Damages target
    }
}
