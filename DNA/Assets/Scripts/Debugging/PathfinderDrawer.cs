using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Paths.Dijkstra;
using Delaunay;
using Delaunay.Geo;

public class PathfinderDrawer : MonoBehaviour {

	public enum Path { All, Free, Clear };
	public Path path = Path.All;
	Path prevPath = Path.Clear;

	Path<GridPoint>[] currentPaths;

	void OnDrawGizmos () {

		if (prevPath != path) {
			prevPath = path;
			if (path == Path.All) {
				currentPaths = Pathfinder.Paths;
			} else if (path == Path.Free) {
				currentPaths = Pathfinder.FreePaths;
			} else if (path == Path.Clear) {
				currentPaths = Pathfinder.ClearPaths;
			}
		}

		if (currentPaths == null)
			return;
			
		foreach (Path<GridPoint> p in currentPaths) {
			if (p.Cost == 0) {
				Gizmos.color = Color.white;
			} else if (p.Cost == int.MaxValue) {
				Gizmos.color = Color.red;
			} else {
				Gizmos.color = Color.green;
			}
			Vector3 from = p.Source.Position;
			Vector3 to = p.Destination.Position;
			from.y += 0.5f;
			to.y += 0.5f;
			Gizmos.DrawLine (from, to);
		}
	}
}
