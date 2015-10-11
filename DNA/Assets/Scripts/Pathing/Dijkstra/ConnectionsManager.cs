using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public class ConnectionsManager : MBRefs {

		static List<ConnectionContainer> connections = new List<ConnectionContainer> ();

		public static ConnectionContainer GetContainer (Connection connection) {
			return connections.Find (x => x.Connection == connection);
		}

		public void Init () {
			CreateConnections ();
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