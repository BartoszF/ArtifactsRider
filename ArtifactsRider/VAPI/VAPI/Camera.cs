using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VAPI
{
    public static class Camera
    {
        static Rectangle rect;

        public static void Init(Rectangle r, Rectangle area)
        {
            rect = r;
        }

        public static Rectangle GetRect
        {
            get
            {
                return rect;
            }
            set
            {
                rect = value;
            }
        }
    }
}
