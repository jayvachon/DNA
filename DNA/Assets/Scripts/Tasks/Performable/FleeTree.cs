using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace DNA.Tasks {

	public class FleeTree : CostTask {

		protected override void OnEnd () {
			SceneManager.LoadScene ("Main");
			base.OnEnd ();
		}
	}
}