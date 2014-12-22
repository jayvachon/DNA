using UnityEngine;
using System.Collections;

public interface IEnableable {

	bool Enabled { get; set; }

	void OnEnable ();
	void OnDisable ();
}
