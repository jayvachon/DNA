﻿using UnityEngine;
using System.Collections;

namespace Pathing {
	
	public class PathDrawer {
		
		readonly Path path;
		readonly LineRenderer line;

		public PathDrawer (Path path, LineRenderer line, Color color) {
			this.path = path;
			this.line = line;
			line.SetColor (color);
		}

		public void Update () {
			line.SetVertexPositions (path.GetPositions ());
		}
	}
}