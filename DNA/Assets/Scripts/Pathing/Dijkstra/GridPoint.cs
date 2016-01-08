using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public class GridPoint : PathElement {

		public Vector3 Position { get; set; }

		public Unit Unit {
			get {
				try {
					return (Unit)Object;
				} catch {
					throw new System.Exception ("Object is not type Unit");
				}
			}
		}

		public readonly List<Connection> connections = new List<Connection> ();
		public List<Connection> Connections {
			get { return connections; }
		}

		public bool HasRoad {
			get { return Connections.Find (x => x.Cost == x.Costs["free"]) != null; }
		}

		FogOfWar fog = null;
		public FogOfWar Fog {
			get { return fog; }
			set {
				fog = value;
				/*if (fog != null) {
					fog.onUpdateState -= OnUpdateFog;
				}
				fog = value;
				MonoBehaviour obj = Object as MonoBehaviour;
				if (obj != null) {
					if (fog != null) {
						fog.onUpdateState += OnUpdateFog;
						obj.gameObject.SetActive (fog.State == FogOfWar.FogState.Faded || fog.State == FogOfWar.FogState.Empty);
					} else {
						obj.gameObject.SetActive (true);
					}
				}*/
			}
		}

		public bool HasFog {
			get { return Fog != null; }
		}
	}
}