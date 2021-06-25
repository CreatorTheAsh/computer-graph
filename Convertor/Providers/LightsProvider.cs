using System.Collections.Generic;
using System.Numerics;
using Converter.Interfaces;
using Converter.Models;

namespace Converter.Providers
{
    public class LightsProvider : ILightsProvider
    {
        public List<Light> GetLights() => new List<Light>() {
            new Light
            {
                Intensity = 3,
                Position = new Vector3(0, 2, 0)
            }};

    }
}
