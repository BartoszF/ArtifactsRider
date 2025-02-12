﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VAPI.RenderEffects.Particle
{
    public class ParticleWorld2D : RenderEffect
    {
        public List<ParticleEmmiter2D> Emmiters;
        public List<Particle2D> Particles;


        SpriteBatch SpriteBatch;
        //RenderTarget2D RT;

        public ParticleWorld2D()
        {
            this.Emmiters = new List<ParticleEmmiter2D>();
            this.Particles = new List<Particle2D>();
            SpriteBatch = new SpriteBatch(Renderer.GD);
        }

        public void AddEmmiter(ParticleEmmiter2D Emmiter)
        {
            Emmiters.Add(Emmiter);
        }

        public override void Dispose()
        {
        }

        public override void BeginEffect()
        {
        }

        public override void EndEffect()
        {
            SpriteBatch.Begin();
            foreach (Particle2D P in Particles)
            {
                P.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }

        public override void UpdateEffect(GameTime GameTime)
        {
            foreach (ParticleEmmiter2D E in Emmiters)
            {
                E.Update(GameTime);
            }

            for(int i =0; i < Particles.Count; i++)
            {
                Particles[i].Update(GameTime);
                if (Particles[i].CurrentState.LifeTime <= 0)
                {
                    Particles.Remove(Particles[i]);
                }
            }
        }


    }
}
