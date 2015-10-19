using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public delegate void OnLoadConnections ();

	public class ConnectionsManager : MBRefs {

		static List<ConnectionContainer> connections = new List<ConnectionContainer> ();

		public OnLoadConnections OnLoadConnections { get; set; }

		public static ConnectionContainer GetContainer (Connection connection) {
			return connections.Find (x => x.Connection == connection);
		}

		public void Init () {
			StartCoroutine (CreateConnections ());
		}

		IEnumerator CreateConnections () {

			const int blockSize = 100;

			List<Connection> gconnections = TreeGrid.Connections;

			for (int i = 0; i < gconnections.Count; i ++) {
				ConnectionContainer c = ObjectPool.Instantiate<ConnectionContainer> ();
				c.Connection = gconnections[i];
				c.Parent = MyTransform;
				connections.Add (c);

				if (i % blockSize == 0)
					yield return null;
			}

			if (OnLoadConnections != null)
				OnLoadConnections ();
		}
	}
}