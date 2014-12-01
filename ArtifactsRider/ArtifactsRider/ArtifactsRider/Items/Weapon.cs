using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using Microsoft.Xna.Framework;

namespace ArtifactsRider.Items
{
    [Serializable]
    public class Weapon : Item
    {
        #region Field Region
        public Hands hands;
        public int attackValue;
        public float attackModifier;
        public int cooldown;
        public int actCD;
        public int damageValue;
        public float damageModifier;
        #endregion

        #region Constructor Region

        private Weapon():base("","",0,0) { } //Serialization
        public Weapon(
        string weaponName,
        string weaponType,
        int price,
        float weight,
        Hands hands,
        int attackValue,
        float attackModifier,
        int cooldown,
        int damageValue,
        float damageModifier)
            : base(weaponName, weaponType, price, weight)
        {
            this.hands = hands;
            this.attackValue = attackValue;
            this.attackModifier = attackModifier;
            this.cooldown = cooldown;
            this.damageValue = damageValue;
            this.damageModifier = damageModifier;
            ItemIcon = GeneralManager.Textures["Items/Weapons/" + name+"_icon"];
        }
        #endregion

        #region Abstract Method Region
        public override object Clone()
        {
            Weapon weapon = new Weapon(
            name,
            type,
            price,
            weight,
            hands,
            attackValue,
            attackModifier,
            cooldown,
            damageValue,
            damageModifier);
            return weapon;
        }

        public virtual void Attack(Vector2 pos)
        {

        }

        public override string ToString()
        {
            string weaponString = base.ToString() + ", ";
            weaponString += hands.ToString() + ", ";
            weaponString += attackValue.ToString() + ", ";
            weaponString += attackModifier.ToString() + ", ";
            weaponString += cooldown.ToString() + ", ";
            weaponString += damageValue.ToString() + ", ";
            weaponString += damageModifier.ToString();
            return weaponString;
        }
        #endregion

    }
}
