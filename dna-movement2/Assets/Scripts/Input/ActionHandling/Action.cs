using System.Collections;

public class Action: IEnableable {

	bool enabled = true;
	public bool Enabled {
		get { return enabled; }
		set {
			enabled = value;
			if (enabled)
				OnEnable ();
			else
				OnDisable ();
		}
	}

	public readonly float time;

	public Action (float time) {
		this.time = time;
	}
	
	public virtual void OnStartAction (IActionable point, IActionable visitor) {
		point.OnArrive ();
		visitor.OnArrive ();
	}

	public virtual void PerformAction (float progress, IActionable visitor) {}
	public virtual void OnEndAction (IActionable point, IActionable visitor) {
		point.OnDepart ();
		visitor.OnDepart ();
	}

	public virtual void OnEnable () {}
	public virtual void OnDisable () {}
}

/**
 * ------unless all actions require visiting by a MovableUnit?? (That's labor!)
 *  THERE IS ONLY SO MUCH OIL IN THE GROUND
 *	SOONER OR LATER THERE WON'T BE NONE AROUND
 *  ALTERNATE SOURCES OF POWER MUST BE FOUND
 *
 *	Action:
 *		-Something that takes some amount of time to perform-
 *		A contract between two objects that information/goods will be exchanged after time has elapsed
 *
 *	Types:
 *		a. Only performed when a MovableUnit visits a StaticUnit (e.g. "Build a new house")
 *		b. Perpetually performed on a StaticUnit (e.g. "Generate ice cream")
 *		c. Player-directed (e.g. "Collect ice cream")
 *		d. Automatically performed (e.g. "Skip over this StaticUnit")
 *	
 *	Notes:
 *		There needs to be a way of choosing a default action to take when
 *			1) a MovableUnit visits a StaticUnit
 *			2) when a StaticUnit has not been directed to do something
 *
 *
 *	Player-directed Action = "Command"
 *	
 *	Examples:
 *	DevelopableUnit
 *		Commands:
 *			Default: Skip over
 *			Build House
 *			Build Hospital
 *			Build Cow
 *	
 *
 */