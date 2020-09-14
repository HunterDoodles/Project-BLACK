using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace BLACK.Combat
{
    
    public class MissileRack : ProjectileFire
    {
        [SerializeField]
        Text CurrentWeapon;
        [SerializeField]
        Missile[] Loadout;
        private int indexer = 0;
        // Start is called before the first frame update
        protected override void Init()
        {
            RackMissile(Loadout[indexer]);
            CurrentWeapon.text = Loadout[indexer].name + " : " + Loadout[indexer].ammo;
        }
        protected override void Update2()
        {
            if (volleying)
                return;
            if (Input.GetKeyDown("e"))
            {
                indexer++;
                if (indexer > Loadout.Length - 1)
                    indexer = 0;
                RackMissile(Loadout[indexer]);
                CurrentWeapon.text = Loadout[indexer].name + " : " + Loadout[indexer].ammo;
            }
            else if (Input.GetKeyDown("q"))
            {
                indexer--;
                if (indexer < 0)
                    indexer = Loadout.Length - 1;
                RackMissile(Loadout[indexer]);
                CurrentWeapon.text = Loadout[indexer].name + " : " + Loadout[indexer].ammo;
            }
        }
        protected override bool isLoaded()
        {
            return (Loadout[indexer].ammo > 0);
        }
        protected override void DecAmmo()
        {
            Loadout[indexer].ammo--;
            CurrentWeapon.text = Loadout[indexer].name + " : " + Loadout[indexer].ammo;
            
        }

    }
}
