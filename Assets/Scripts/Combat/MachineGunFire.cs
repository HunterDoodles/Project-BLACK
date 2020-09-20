using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Combat
{
    public class MachineGunFire : ProjectileFire
    {
        
        protected override void Init()
        {
            holdDown = true;
        }

        protected override void Update2()
        {

        }
        protected override bool isLoaded()
        {
            return true;
        }
        protected override void DecAmmo()
        {
            
        }
    }
}
