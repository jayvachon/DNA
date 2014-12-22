using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

	public static PathManager instance = null;

	Path path = null;
	PathDrawer drawer = null;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<SelectUnitEvent> (OnSelectUnitEvent);
		Events.instance.AddListener<UnselectUnitEvent> (OnUnselectUnitEvent);
	}

	// Remove this?
	/*public Path TogglePoint (IPathPoint point) {
		if (path != null) {
			path.TogglePoint (point);
			drawer.Update ();
		}
		return path;
	}*/

	public Path AddPoint (IPathPoint point) {
		if (path != null) {
			path.AddPoint (point);
			drawer.Update ();
		}
		return path;
	}

	public Path RemovePoint (IPathPoint point) {
		if (path != null) {
			path.RemovePoint (point);
			drawer.Update ();
		}
		return path;
	}

	void OnSelectUnitEvent (SelectUnitEvent e) {
		if (e.unit is MovableUnit) {
			MovableUnit mu = e.unit as MovableUnit;
			path = mu.MyPath;
			drawer = mu.MyPathDrawer;
		} else {
			path = null;
			drawer = null;
		}
	}

	void OnUnselectUnitEvent (UnselectUnitEvent e) {
		path = null;
		drawer = null;
	}
}
