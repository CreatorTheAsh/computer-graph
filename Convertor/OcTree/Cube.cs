using Converter.Models;
using System.Collections.Generic;
using System.Numerics;

namespace Converter.OcTree
{
	class Cube
	{
		public Vector3 x1;
		public Vector3 x2;
		public Vector3 x3;
		public Vector3 x4;
		public Vector3 x5;
		public Vector3 x6;
		public Vector3 x7;
		public Vector3 x8;

		readonly List<Triangle> Sides;

		public Cube(Vector3 x, Vector3 y, Vector3 z, Vector3 k, Vector3 n1, Vector3 y1, Vector3 z1, Vector3 k1)
		{
			Sides = new List<Triangle>();
			x1 = x;
			x2 = y;
			x3 = z;
			x4 = k;

			x5 = n1;
			x6 = y1;
			x7 = z1;
			x8 = k1;

			Vector3 temp = new Vector3(0, 0, 0);
			Sides.Add(new Triangle(x7, x6, x8, temp, temp, temp));
			Sides.Add(new Triangle(x5, x6, x8, temp, temp, temp));

			Sides.Add(new Triangle(x1, x5, x8, temp, temp, temp));
			Sides.Add(new Triangle(x1, x4, x8, temp, temp, temp));

			Sides.Add(new Triangle(x5, x2, x6, temp, temp, temp));
			Sides.Add(new Triangle(x5, x2, x1, temp, temp, temp));

			Sides.Add(new Triangle(x7, x2, x3, temp, temp, temp));
			Sides.Add(new Triangle(x7, x2, x6, temp, temp, temp));

			Sides.Add(new Triangle(x3, x7, x8, temp, temp, temp));
			Sides.Add(new Triangle(x3, x4, x8, temp, temp, temp));

			Sides.Add(new Triangle(x3, x2, x4, temp, temp, temp));
			Sides.Add(new Triangle(x1, x2, x4, temp, temp, temp));
		}

		bool ThereIsIntersectionBetweenRayAndTriangle(Vector3 rayOrigin, Vector3 rayVector, Triangle inTriangle)
		{

			Vector3 vertex0 = inTriangle.A;
			Vector3 vertex1 = inTriangle.B;
			Vector3 vertex2 = inTriangle.C;
			Vector3 edge1 = vertex1 - vertex0;
			Vector3 edge2 = vertex2 - vertex0;
			Vector3 h = Vector3.Cross(rayVector, edge2); 

			float a = Vector3.Dot(edge1, h);
			float EPSILON = 1e-5f;

			if (a > -EPSILON && a < EPSILON)
			{
				return false;
			}
			float f = 1 / a;
			Vector3 s = rayOrigin - vertex0;
			float u = f * Vector3.Dot(s, h);
			if (u < 0.0 || u > 1.0)
			{
				return false;
			}
			Vector3 q = Vector3.Cross(s, edge1);
			float v = f * Vector3.Dot(rayVector, q);
			if (v < 0.0 || u + v > 1.0)
			{
				return false;
			}


			float t = f * Vector3.Dot(edge2, q);
			return true;
		}

		public bool IntersectionBetweenRayAndCube(Vector3 rayOrigin, Vector3 rayVector)
		{
			foreach (Triangle i in Sides)
			{
				if (ThereIsIntersectionBetweenRayAndTriangle(rayOrigin, rayVector, i))
					return true;
			}
			return false;
		}
	}
}
