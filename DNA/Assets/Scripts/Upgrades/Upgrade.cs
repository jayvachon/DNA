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
		set { 
			if (value < 0 || value > Levels.Length-1)
				throw new System.Exception ("Level out of range");
			currentLevel = value; 
		}
	}
}
