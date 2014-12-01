using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrKlang;
using System.IO;
using Microsoft.Xna.Framework;

namespace VAPI
{
    public static class SoundEngine
    {
        public static ISoundEngine engine = new ISoundEngine();
        private static Vector2 Position;

        public static void LoadSound(string name, string path)
        {
            List<byte> bin = new List<byte>();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while(br.BaseStream.Position != br.BaseStream.Length)
                        bin.Add(br.ReadByte());
                }
            }

            //sounds.Add(name, bin.ToArray());
            engine.AddSoundSourceFromMemory(bin.ToArray(), name);
        }

        public static void SetPosition(Vector2 v, float angle)
        {
            Position = v;
            engine.SetListenerPosition(v.X, 0, v.Y, (float)Math.Cos(angle), 0, (float)Math.Sin(angle));
        }

        public static ISound PlaySound(Vector2 v, string name)
        {
            ISound t = engine.Play3D(name, v.X, 0, v.Y, false,false,StreamMode.AutoDetect,true);
            //t.MaxDistance = 5;
            t.MinDistance = 45;
            t.Volume = 100;
            //t.SoundEffectControl.EnableI3DL2ReverbSoundEffect(-700,1,0.1f,2.1f,0.2f,-2000,0.01f,250,0.05f,95,99,4900);

            return t;
        }

        public static ISound PlaySound(Vector2 v, string name, float Volume)
        {
            ISound t = engine.Play3D(name, v.X, 0, v.Y, false, false, StreamMode.AutoDetect, true);
            //t.MaxDistance = 5;
            t.MinDistance = 150;
            t.Volume = Volume;
            //t.SoundEffectControl.EnableI3DL2ReverbSoundEffect(-700,1,0.1f,2.1f,0.2f,-2000,0.01f,250,0.05f,95,99,4900);

            return t;
        }

        public static ISound PlayLooped(string name)
        {
            ISound t = engine.Play3D(name, 0, 0, 0, true);
            t.Volume = 100;

            return t;
        }

    }
}
