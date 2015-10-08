﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.EventSystem;
using DNA.InventorySystem;
using DNA.Tasks;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	// TODO: Move a bunch of this stuff to a ConstructionManager

	public class Player : MonoBehaviour, IInventoryHolder, ITaskPerformer {

		static Player instance = null;
		static public Player Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (Player)) as Player;
				}
				return instance;
			}
		}

		Inventory inventory;
		public Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory (this);
					inventory.Add (new MilkshakeHolder (100000, 30));
					inventory.Add (new CoffeeHolder (100000, 50));
					inventory.Add (new YearHolder (5, 0));
				}
				return inventory;
			}
		}

		public MilkshakeHolder Milkshakes {
			get { return (MilkshakeHolder)Inventory["Milkshakes"]; }
		}

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					performableTasks.Add (new BuildRoad ());
					performableTasks.Add (new GenerateUnit<CoffeePlant> ()).onEnd += OnGenerateUnit;
					performableTasks.Add (new GenerateUnit<MilkshakePool> ()).onEnd += OnGenerateUnit;
				}
				return performableTasks;
			}
		}

		readonly List<PointContainer> constructionTargets = new List<PointContainer> ();

		PointContainer ConstructionTarget {
			get { return constructionTargets[constructionTargets.Count-1]; }
		}

		System.Type pen;

		void Awake () {
			Events.instance.AddListener<ClickPointEvent> (OnClickPointEvent);
			Events.instance.AddListener<MouseEnterPointEvent> (OnMouseEnterPointEvent);
			Events.instance.AddListener<MouseExitPointEvent> (OnMouseExitPointEvent);
		}

		void OnGenerateUnit (PerformerTask task) {
			StaticUnit unit = ((GenerateUnit)task).GeneratedUnit as StaticUnit;
			ConstructionTarget.SetStaticUnit (unit);
			constructionTargets.Clear ();
		}

		public void SetConstructionPen<T> () where T : IConstructable {
			if (pen != typeof (T)) {
				UI.Instance.ConstructPrompt.Close ();
				RoadConstructor.Instance.Clear ();
			}
			pen = typeof (T);
			PlayerActionState.Set (ActionState.Construction);
		}

		void Construct (GridPoint point) {

			if (pen == typeof (BuildRoad)) {
				RoadConstructor.Instance.AddPoint (point);
				if (RoadConstructor.Instance.PointCount < 2)
					return;
			}

			CostTask t = (CostTask)PerformableTasks[pen];

			string text = "Purchase: ";
			foreach (var cost in t.Settings.Costs) {
				text += cost.Value.ToString () + cost.Key.Substring (0, 1);
			}
			UI.Instance.ConstructPrompt.Open (text, () => t.Start ());
		}

		bool CanConstructOnPoint (GridPoint point) {
			return PlayerActionState.State == ActionState.Construction 
				&& (PerformableTasks[pen] as IConstructable).CanConstructOnPoint (point);
		}

		void OnClickPointEvent (ClickPointEvent e) {
			constructionTargets.Add (e.Container);
			if (CanConstructOnPoint (e.Point)) {
				Construct (e.Point);
			}
		}

		void OnMouseEnterPointEvent (MouseEnterPointEvent e) {
			if (CanConstructOnPoint (e.Point)) {
				ObjectPool.Instantiate ("PointHighlight", e.Point.Position);
			}
		}

		void OnMouseExitPointEvent (MouseExitPointEvent e) {
			if (CanConstructOnPoint (e.Point)) {
				ObjectPool.Destroy ("PointHighlight");
			}
		}
	}
}