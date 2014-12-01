using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using System.Xml.Serialization;

namespace ArtifactsRider.Items.Weapons
{
    class AK47 : Weapon
    {
        public AK47()
            : base("AK47", "Assault Rifle", 10, 3.5f, Hands.Two, 10, 1, 220, 10, 1)
        {
            this.ItemIcon = GeneralManager.Textures["Items/Weapons/" + name + "_icon"];
        }
    }
}
