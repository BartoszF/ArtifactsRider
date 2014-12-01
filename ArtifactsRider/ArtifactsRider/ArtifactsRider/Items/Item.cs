using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ArtifactsRider
{
    public enum Hands { One, Two }
    public enum ArmorLocation { Body, Head, Hands, Feet }

    /// <summary>
    /// Item class
    /// </summary>
    [Serializable]
    public abstract class Item
    {
        #region Field Region
        public string name;
        public string type;
        public int price;
        public float weight;
        public bool equipped;
        #endregion

        public Texture2D ItemIcon;

        #region Constructor Region
        public Item(string name, string type, int price, float weight)
        {
            this.name = name;
            this.type = type;
            this.price = price;
            this.weight = weight;
            this.equipped = false;
        }
        #endregion

        #region Abstract Method Region
        public abstract object Clone();

        public override string ToString()
        {
            string itemString = "";
            itemString += name + ", ";
            itemString += type + ", ";
            itemString += price.ToString() + ", ";
            itemString += weight.ToString();
            return itemString;
        }
        #endregion
    }
}
