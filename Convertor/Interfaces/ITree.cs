using System.Collections.Generic;
using System.Numerics;
using Converter.Models;

namespace Converter.Interfaces
{
    public interface ITree
    {
        public void FindIntersections(Vector3 rayOrigin, Vector3 rayVector, List<Triangle> result);
        public void Initialize(float max, List<Triangle> triangles);
    }
}