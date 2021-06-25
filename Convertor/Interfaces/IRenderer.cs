using System.Collections.Generic;
using System.Numerics;
using Converter.Models;

namespace Converter.Interfaces
{
    public interface IRenderer
    {
        public Vector3[,] Render(List<Triangle> triangles);
    }
}
