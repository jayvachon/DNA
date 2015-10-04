using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public class ConnectionsManager : MBRefs {

		new void Awake () {
			CreateConnections ();
		}

		void CreateConnections () {

			List<Connection> connections = TreeGrid.Connections;

			for (int i = 0; i < connections.Count; i ++) {
				ConnectionContainer c = ObjectPool.Instantiate<ConnectionContainer> ();
				c.Connection = connections[i];
				c.Parent = MyTransform;
			}
		}
	}
}