using UnityEngine;
using System.Collections;
using GameEvents;
using GameInput;
using Units;

namespace Pathing {

	public class PathManager : MonoBehaviour {
		
		static PathManager instanceInternal = null;
		static public PathManager instance {
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

		void Awake () {
			Events.instance.AddListener<SelectEvent> (OnSelectEvent);
			Events.instance.AddListener<UnselectEvent> (OnUnselectEvent);
		}

		void OnSelectEvent (SelectEvent e) {
			MobileUnit mu = e.unit as MobileUnit;
			if (mu != null) {
				selectedPath = mu.Path;
			}
		}

		void OnUnselectEvent (UnselectEvent e) {
			selectedPath = null;
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