using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtifactsRider.Items
{
    public class Armor : Item
    {
        #region Field Region
        public ArmorLocation location;
        public int defenseValue;
        public int defenseModifier;
        #endregion

        #region Constructor Region
        public Armor(
             string armorName,
             string armorType,
             int price,
             float weight,
             ArmorLocation location,
             int defenseValue,
             int defenseModifier)
            : base(armorName, armorType, price, weight)
        {
            this.location = location;
            this.defenseValue = defenseValue;
            this.defenseModifier = defenseModifier;
        }
        #endregion

        #region Abstract Method Region
        public override object Clone()
        {
            Armor armor = new Armor(
            name,
            type,
            price,
            weight,
            location,
            defenseValue,
            defenseModifier);
            return armor;
        }

        public override string ToString()
        {
            string armorString = base.ToString() + ", ";
            armorString += location.ToString() + ", ";
            armorString += defenseValue.ToString() + ", ";
            armorString += defenseModifier.ToString();
            return armorString;
        }
        #endregion
    }
}
