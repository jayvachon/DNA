using UnityEngine;
using System.Collections;
using DNA.EventSystem;
using DNA.InputSystem;
using DNA.Units;

namespace Pathing {

	public class PathManager : MonoBehaviour {
		
		static PathManager instanceInternal = null;
		static public PathManager Instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType (typeof (PathManager)) as PathManager;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("PathManager");
						DontDestroyOnLoad (go);
						instanceInternal = go.AddComponent<PathManager>();
					}
				}
				return instanceInternal;
			}
		}

		Path selectedPath = null;
		public Path SelectedPath {
			get { return selectedPath; }
			set { selectedPath = value; }
		}

		public void EnterPathPoint (DragSettings dragSettings, PathPoint pathPoint) {
			if (selectedPath != null) {
				selectedPath.PointDragEnter (dragSettings, pathPoint);
			}
		}

		public void ExitPathPoint (DragSettings dragSettings, PathPoint pathPoint) {
			if (selectedPath != null) {
				selectedPath.PointDragExit (dragSettings, pathPoint);
			}	
		}
	}
}