using System.Collections.Generic;
using Converter.Models;

namespace Converter.Interfaces
{
    public interface ILightsProvider
    {
        public List<Light> GetLights();
    }
}
