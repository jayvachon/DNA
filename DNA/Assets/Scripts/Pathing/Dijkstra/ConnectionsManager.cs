using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public class ConnectionsManager : MBRefs {

		List<ConnectionContainer> connections = new List<ConnectionContainer> ();

		public void Init () {
			CreateConnections ();
		}

		public void EnableRoadAtIndex (int index) {
			connections[index].Connection.SetCost ("free");
		}

		void CreateConnections () {

			List<Connection> gconnections = TreeGrid.Connections;

			for (int i = 0; i < gconnections.Count; i ++) {
				ConnectionContainer c = ObjectPool.Instantiate<ConnectionContainer> ();
				c.Connection = gconnections[i];
				c.Parent = MyTransform;
				connections.Add (c);
			}
		}
	}
}