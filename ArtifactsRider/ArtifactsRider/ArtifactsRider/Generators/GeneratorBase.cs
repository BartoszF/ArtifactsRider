using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtifactsRider.MapManager;

namespace ArtifactsRider.Generators
{
    /// <summary>
    /// Base abstract class for Map Generators
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// Abstract class for Generating
        /// </summary>
        /// <param name="Chunk">Chunk to generate in</param>
        public abstract void Generate(Chunk Chunk);
    }
}
