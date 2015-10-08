using UnityEngine;
using System.Collections;

namespace DNA.Paths {

	public class PathElement {

		public IPathElementObject Object { get; set; }

		static int version = 0;
		int myVersion = 0;

		public bool UpToDate { get { return myVersion == version; } }

		protected void UpdateVersion () {
			version ++;
		}

		public void SetUpToDate () {
			myVersion = version;
		}
	}
}