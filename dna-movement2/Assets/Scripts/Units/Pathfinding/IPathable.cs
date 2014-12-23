using UnityEngine;
using System.Collections;

public interface IPathable {

	Path MyPath { get; }
	bool ActivePath { set; }

	void OnUpdatePath ();
	void StartMoveOnPath ();
}
