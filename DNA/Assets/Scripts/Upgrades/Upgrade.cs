using UnityEngine;
using System.Collections;

public abstract class Upgrade {
	public object CurrentValue { get { return null; } }
	public virtual int CurrentLevel { get; set; }
}

public abstract class Upgrade<T> : Upgrade {
	
	protected abstract T[] Levels { get; }
	
	new public T CurrentValue { 
		get { return Levels[CurrentLevel]; } 
	}

	int currentLevel = 0;
	public override int CurrentLevel {
		get { return currentLevel; }
		set { currentLevel = Mathf.Clamp (value, 0, Levels.Length-1); }
	}
}
