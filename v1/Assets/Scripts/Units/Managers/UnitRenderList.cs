using UnityEngine;
using System.Collections;

public class UnitRenderList : MonoBehaviour {

	public UnitRender[] renders;

	public static UnitRenderList instance;

	void Awake () {
		if (instance == null) 
			instance = this;
	}

	public UnitRender GetRender (string name) {
		name = name + "Render";
		for (int i = 0; i < renders.Length; i ++) {
			if (renders[i].name == name)
				return renders[i];
		}
		return null;
	}
}
