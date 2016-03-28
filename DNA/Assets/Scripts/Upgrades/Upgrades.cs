using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Upgrades {

	static Upgrades instance = null;
	public static Upgrades Instance {
		get {
			if (instance == null) {
				instance = new Upgrades();
			}
			return instance;
		}
	}

	public delegate void UpgradeDelegate<T> (T u) where T : Upgrade;
	delegate void UpgradeDelegate (Upgrade u);
	Dictionary<System.Type, KeyValuePair<Upgrade, UpgradeDelegate>> delegates = new Dictionary<System.Type, KeyValuePair<Upgrade, UpgradeDelegate>>();
	Dictionary<System.Delegate, UpgradeDelegate> delegateLookup = new Dictionary<System.Delegate, UpgradeDelegate> ();

	public void AddListener<T> (UpgradeDelegate<T> del) where T : Upgrade, new () {

		if (delegateLookup.ContainsKey (del))
			return;

		UpgradeDelegate internalDelegate = (u) => del((T)u);
		delegateLookup[del] = internalDelegate;
		KeyValuePair<Upgrade, UpgradeDelegate> tempDel;

		if (delegates.TryGetValue (typeof (T), out tempDel)) {
			delegates[typeof (T)] = new KeyValuePair<Upgrade, UpgradeDelegate> (
				tempDel.Key,
				tempDel.Value + internalDelegate
			);
			SetLevel<T> (tempDel.Key.CurrentLevel);
		} else {
			delegates[typeof (T)] = new KeyValuePair<Upgrade, UpgradeDelegate> (
				new T (),
				internalDelegate				
			);
			SetLevel<T> (0);
		}
	}

	public void RemoveListener<T> (UpgradeDelegate<T> del) where T : Upgrade {

		UpgradeDelegate internalDelegate;
		if (delegateLookup.TryGetValue (del, out internalDelegate)) {
			KeyValuePair<Upgrade, UpgradeDelegate> tempDel;
			if (delegates.TryGetValue (typeof (T), out tempDel)) {
				delegates[typeof (T)] = new KeyValuePair<Upgrade, UpgradeDelegate> (
					tempDel.Key,
					tempDel.Value - internalDelegate
				);
				if (delegates[typeof (T)].Value == null)
					delegates.Remove (typeof (T));
			}
			delegateLookup.Remove (del);
		}
	}

	public void NextLevel<T> () where T : Upgrade {
		SetLevel<T> (-1);
	}

	public void SetLevel<T> (int level) where T : Upgrade {
		KeyValuePair<Upgrade, UpgradeDelegate> del;
		if (delegates.TryGetValue (typeof (T), out del)) {
			if (level == -1) {
				del.Key.CurrentLevel ++;
			} else {
				del.Key.CurrentLevel = level;
			}
			del.Value.Invoke (del.Key);
		}
	}

	public T GetUpgrade<T> () where T : Upgrade {
		KeyValuePair<Upgrade, UpgradeDelegate> del;
		if (delegates.TryGetValue (typeof (T), out del)) {
			return (T)del.Key;
		} else {
			throw new System.Exception ("Could not find upgrade of type " + typeof (T));
		}
	}
}
