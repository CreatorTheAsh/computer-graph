using Converter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Converter.OcTree
{
	class TreeNode
	{
		public List<Triangle> Triangles { get; set; }
		public List<TreeNode> DaughterNodes { get; set; }

		private bool List = true;
		private Cube SpaceCube { get; set; }


		public TreeNode(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
		{
			Vector3 x1 = new Vector3(maxX, maxY, maxZ);
			Vector3 x2 = new Vector3(minX, maxY, maxZ);
			Vector3 x3 = new Vector3(minX, minY, maxZ);
			Vector3 x4 = new Vector3(maxX, minY, maxZ);

			Vector3 x5 = new Vector3(maxX, maxY, minZ);
			Vector3 x6 = new Vector3(minX, maxY, minZ);
			Vector3 x7 = new Vector3(minX, minY, minZ);
			Vector3 x8 = new Vector3(maxX, minY, minZ);

			SpaceCube = new Cube(x1, x2, x3, x4, x5, x6, x7, x8);
			Triangles = new List<Triangle>();
			DaughterNodes = new List<TreeNode>();
		}

		public void Rebuild(float min)
		{
			float minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew;


			// 1, 2, 3, 4 oct
			Vector3 center = new Vector3((SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f, (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f, (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f);

			minXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			maxXnew = SpaceCube.x1.X;
			minYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			maxYnew = SpaceCube.x1.Y;
			minZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			maxZnew = SpaceCube.x1.Z;

			if (Triangles.Count() < 100 || (maxXnew - minXnew) < min)
				return;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			maxXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			minXnew = SpaceCube.x2.X;
			minYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			maxYnew = SpaceCube.x1.Y;
			minZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			maxZnew = SpaceCube.x1.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			maxXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			minXnew = SpaceCube.x2.X;
			maxYnew = (SpaceCube.x2.Y + SpaceCube.x3.Y) * 0.5f;
			minYnew = SpaceCube.x3.Y;
			minZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			maxZnew = SpaceCube.x1.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			minXnew = (SpaceCube.x3.X + SpaceCube.x4.X) * 0.5f;
			maxXnew = SpaceCube.x4.X;
			maxYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			minYnew = SpaceCube.x4.Y;
			minZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			maxZnew = SpaceCube.x1.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			// 5, 6, 7, 8 oct

			minXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			maxXnew = SpaceCube.x1.X;
			minYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			maxYnew = SpaceCube.x1.Y;
			maxZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			minZnew = SpaceCube.x5.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			maxXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			minXnew = SpaceCube.x2.X;
			minYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			maxYnew = SpaceCube.x1.Y;
			maxZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			minZnew = SpaceCube.x5.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			maxXnew = (SpaceCube.x1.X + SpaceCube.x2.X) * 0.5f;
			minXnew = SpaceCube.x2.X;
			maxYnew = (SpaceCube.x2.Y + SpaceCube.x3.Y) * 0.5f;
			minYnew = SpaceCube.x3.Y;
			maxZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			minZnew = SpaceCube.x5.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			minXnew = (SpaceCube.x3.X + SpaceCube.x4.X) * 0.5f;
			maxXnew = SpaceCube.x4.X;
			maxYnew = (SpaceCube.x1.Y + SpaceCube.x4.Y) * 0.5f;
			minYnew = SpaceCube.x4.Y;
			maxZnew = (SpaceCube.x1.Z + SpaceCube.x5.Z) * 0.5f;
			minZnew = SpaceCube.x5.Z;

			DaughterNodes.Add(new TreeNode(minXnew, maxXnew, minYnew, maxYnew, minZnew, maxZnew));

			List<Triangle> newVectors = new List<Triangle>();

			while (Triangles.Count() != 0)
			{
				int counter1 = -1;
				int counter2 = -1;
				int counter3 = -1;

				Triangle i = Triangles.Last();
				Triangles.RemoveAt(Triangles.Count() - 1);

				counter1 = PositionFinder(i.A, center);
				counter2 = PositionFinder(i.B, center);
				counter3 = PositionFinder(i.C, center);

				if (counter1 == counter2 && counter2 == counter3)
					DaughterNodes[counter1].Triangles.Add(i);
				else
					newVectors.Add(i);
			}

			Triangles = newVectors;

			List = false;

			DaughterNodes[0].List = true;
			DaughterNodes[0].Rebuild(min);
			DaughterNodes[1].List = true;
			DaughterNodes[1].Rebuild(min);
			DaughterNodes[2].List = true;
			DaughterNodes[2].Rebuild(min);
			DaughterNodes[3].List = true;
			DaughterNodes[3].Rebuild(min);
			DaughterNodes[4].List = true;
			DaughterNodes[4].Rebuild(min);
			DaughterNodes[5].List = true;
			DaughterNodes[5].Rebuild(min);
			DaughterNodes[6].List = true;
			DaughterNodes[6].Rebuild(min);
			DaughterNodes[7].List = true;
			DaughterNodes[7].Rebuild(min);
		}

		public void FindIntersections(Vector3 rayOrigin, Vector3 rayVector, List<Triangle> result)
		{

			foreach (Triangle i in Triangles)
				result.Add(i);
			if (List)
				return;

			for (int i = 0; i < 8; i++)
			{
				if (DaughterNodes[i] != null && DaughterNodes[i].SpaceCube.IntersectionBetweenRayAndCube(rayOrigin, rayVector))
				{
					DaughterNodes[i].FindIntersections(rayOrigin, rayVector, result);
				}
			}

		}

		public int PositionFinder(Vector3 X, Vector3 center)
		{
			if (X.Z > center.Z)
			{
				if (X.Y > center.Y)
				{
					return X.X > center.X ? 0 : 1;
				}

				return X.X > center.X ? 3 : 2;
			}

			if (X.Y > center.Y)
			{
				return X.X > center.X ? 4 : 5;
			}

			return X.X > center.X ? 7 : 6;
		}
	}
}
