using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Converter.Interfaces;
using Converter.Models;

namespace Converter.Renderers
{
    public class Renderer : IRenderer
    {
        private readonly ICameraPositionProvider _positionProvider;
        private readonly ICameraDirectionProvider _directionProvider;
        private readonly IScreenProvider _screenProvider;
        private readonly IColorProvider _colorProvider;
        private readonly ILightsProvider _lightsProvider;
        private readonly ITreeProvider _treeProvider;

        private readonly List<Light> _lights;
        private readonly ITree _tree;
        
        private const float Epsilon = 0.0000001f;

        public Renderer(ICameraPositionProvider positionProvider, ICameraDirectionProvider directionProvider,
            IScreenProvider screenProvider, IColorProvider colorProvider, ILightsProvider lightsProvider,
            ITreeProvider treeProvider)
        {
            _positionProvider = positionProvider;
            _directionProvider = directionProvider;
            _screenProvider = screenProvider;
            _colorProvider = colorProvider;
            _lightsProvider = lightsProvider;
            _treeProvider = treeProvider;
            _tree = _treeProvider.GetTree();
            _lights = _lightsProvider.GetLights();
        }


        public Vector3[,] Render(List<Triangle> triangles)
        {
            Vector3[,] result = Initialize();
            var listA = triangles.Select(x => x.A);
            var listB = triangles.Select(x => x.B);
            var listC = triangles.Select(x => x.C);
            var max = listA.Concat(listB).Concat(listC).Max(x => new[]{Math.Abs(x.X), Math.Abs(x.Y), Math.Abs(x.Z)}.Max());
            
            _tree.Initialize(max, triangles);
            
            Parallel.For(0, _screenProvider.GetHeight(), x =>
            {
                Parallel.For(0, _screenProvider.GetWidth(), y =>
                {
                    ProcessPixel(x, y);
                    ProcessColor(ref result[x, y].X);
                    ProcessColor(ref result[x, y].Y);
                    ProcessColor(ref result[x, y].Z);
                });
            });
            
            var resultReversed = new Vector3[_screenProvider.GetHeight(), _screenProvider.GetWidth()];
            
            for (int i = 0; i < _screenProvider.GetHeight(); i++)
            {
                for (int j = 0; j < _screenProvider.GetWidth(); j++)
                {
                    resultReversed[i, j] = result[_screenProvider.GetHeight() - i - 1, _screenProvider.GetWidth() - j - 1];
                }
            }
            
            return resultReversed;

            void ProcessPixel(int height, int width)
            {
                float minDistance = float.MaxValue;
                var currentColor = _colorProvider.GetBackgroundColor();
                List<Triangle> resultTriangles = new List<Triangle>();
                var direction = _directionProvider.GetCameraDirection(height, width,
                    _screenProvider.GetHeight(), _screenProvider.GetWidth(),
                    _screenProvider.GetFov());
                _tree.FindIntersections(_positionProvider.GetCamera(), direction, resultTriangles);
                
                foreach (var triangle in resultTriangles)
                {
                    CastRay(_positionProvider.GetCamera(),
                        direction,
                        triangle,
                        _lights,
                        out var hit,
                        out var color);

                    var distance = Vector3.Distance(hit, _positionProvider.GetCamera());
                    
                    if (minDistance > distance && color != _colorProvider.GetBackgroundColor())
                    {
                        minDistance = distance;
                        currentColor = color;
                    }
                }

                result[height, width] = currentColor;
            }

            bool IsIntersecting(Vector3 vector, Vector3 direction, Triangle triangle, out float t, out float u, out float v)
            {
                t = 0;
                u = 0;
                v = 0;
                
                Vector3 first = triangle.B - triangle.A;
                Vector3 second = triangle.C - triangle.A;
                
                Vector3 pvector = Vector3.Cross(direction, second);
                
                float det = Vector3.Dot(first, pvector);

                if (det < Epsilon && det > -Epsilon)
                {
                    return false;
                }

                float invertedDet = 1f / det;

                var tvector = vector - triangle.A;

                u = Vector3.Dot(tvector, pvector) * invertedDet;

                if (u < 0 || u > 1)
                {
                    return false;
                }

                var qvector = Vector3.Cross(tvector, first);

                v = Vector3.Dot(direction, qvector) * invertedDet;

                if (v < 0 || u + v > 1)
                {
                    return false;
                }

                t = Vector3.Dot(second, qvector) * invertedDet;
                return true;
            }

            bool ProcessScene(Vector3 vector, Vector3 direction, Triangle triangle, out Vector3 hit, out Vector3 normal)
            {
                hit = new Vector3();
                normal = new Vector3();

                bool intersecting = IsIntersecting(vector, direction, triangle, out var t, out var u, out var v);

                if (intersecting)
                {
                    hit = vector + direction * t;
                    normal = triangle.N0 * (1f - u - v) + triangle.N1 * u + triangle.N2 * v;
                    return true;
                }

                return false;
            }

            void CastRay(Vector3 vector, Vector3 direction, Triangle triangle, List<Light> lights, out Vector3 hit,
                out Vector3 color)
            {
                bool intersecting = ProcessScene(vector, direction, triangle, out hit, out Vector3 intersectNormal);

                if (!intersecting)
                {
                    color = _colorProvider.GetBackgroundColor();
                    return;
                }

                var normal = Vector3.Dot(direction, intersectNormal) < 0 ? -intersectNormal : intersectNormal;

                float diffuseLightIntensity = 0f;
                float reverseDiffuseLightIntensity = 0f;

                foreach (var light in lights)
                {
                    var lightDirection = light.Position - hit;

                    var square = Vector3.Dot(lightDirection, lightDirection);

                    var distance = (float)Math.Sqrt(square);

                    lightDirection.X /= distance;
                    lightDirection.Y /= distance;
                    lightDirection.Z /= distance;

                    diffuseLightIntensity += light.Intensity * Math.Max(0, Vector3.Dot(normal, lightDirection));
                    reverseDiffuseLightIntensity +=
                        light.Intensity * Math.Max(0, Vector3.Dot(normal, -lightDirection));
                }

                color = _colorProvider.GetObjectColor() * Math.Max(diffuseLightIntensity, reverseDiffuseLightIntensity);
            }

            void ProcessColor(ref float colorElement)
            {
                if (colorElement > 255f)
                {
                    colorElement = 255f;
                }

                if (colorElement < 0f)
                {
                    colorElement = 0f;
                }
            }
        }

        private Vector3[,] Initialize()
        {
            Vector3[,] result = new Vector3[_screenProvider.GetHeight(),_screenProvider.GetWidth()];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = _colorProvider.GetBackgroundColor();
                }
            }
            return result;
        }
    }
}
