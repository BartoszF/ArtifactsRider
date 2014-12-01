using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VAPI;

namespace ArtifactsRider
{
    class Entity
    {
        // TODO:
        // Implement entity

        int id;
        bool isPhysical;
        float Height;
        Vector2 center;
        bool needUpdate;
        Texture2D tex;
        List<Vector2> vertices;

        public Entity(string tex, Vector2 center)
        {
            this.LoadContent(tex, center);
        }

        private void LoadContent(string tex, Vector2 center)
        {
            this.center = center;
            this.tex = GeneralManager.Textures["Textures/Entities/"+tex];
        }

        public void Update(GameTime gameTime)
        {

        }
        public void Draw(GameTime gameTime)
        {
            Renderer.Draw(tex, center, Color.White);
        }
    }
}
