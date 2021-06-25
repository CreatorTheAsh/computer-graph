using Converter.Models;
using System.Collections.Generic;
using System.Numerics;
using Converter.Interfaces;

namespace Converter.OcTree
{
	public class Tree : ITree
	{
		private float maxSize;
		private float minSize;
		private TreeNode head;
		
		public void Initialize(float max, List<Triangle> triangles)
		{
			minSize = max / 10;
			maxSize = max;
			head = new TreeNode(-max, max, -max, max, -max, max) {Triangles = triangles};
			head.Rebuild(minSize);
		}

		public void FindIntersections(Vector3 rayOrigin, Vector3 rayVector, List<Triangle> result)
		{
			head.FindIntersections(rayOrigin, rayVector, result);
		}
	}
}
