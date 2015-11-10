using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA {

	public class PathElementContainer : MBRefs {

		public virtual PathElement Element { get; protected set; }
		protected virtual Vector3 Anchor { get { return Vector3.zero; } }

		int constructionCost;

		IPathElementObject project;
		ConstructionSite site;
		RepairSite repairSite;

		DamageHandler damageHandler;

		void Awake () {
			damageHandler = new DamageHandler (OnDamage, OnDestroy);
		}

		public ConstructionSite BeginConstruction<T> () where T : MonoBehaviour, IPathElementObject {

			// Create a construction site and listen for labor to be completed
			// Set the project to turn into once labor completes
			if (Element.State == DevelopmentState.Undeveloped) {
				project = (IPathElementObject)ObjectPool.Instantiate<T> ();
				(project as MonoBehaviour).gameObject.SetActive (false);
				site = (ConstructionSite)SetObject<ConstructionSite> ();
				site.Inventory["Labor"].onEmpty += EndConstruction;
				Element.State = DevelopmentState.UnderConstruction;
			}
			return site;
		}

		public void EndConstruction () {

			// Turn into the project set when construction began
			// Mark this container as developed so that nothing else can be built here
			site.Inventory["Labor"].onEmpty -= EndConstruction;
			(project as MonoBehaviour).gameObject.SetActive (true);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
			OnEndConstruction (project);
			site = null;
		}

		public void BeginRepair (float damageAmount) {
			if (Element.State == DevelopmentState.Damaged) {
				project = Element.Object;
				(project as MonoBehaviour).gameObject.SetActive (false);
				repairSite = (RepairSite)SetObject<RepairSite> ();
				repairSite.Inventory["Labor"].onEmpty += EndRepair;
				Element.State = DevelopmentState.UnderRepair;
				repairSite.LaborCost = (int)(DataManager.GetConstructionCost ((project as Unit).Settings.Symbol) * damageAmount);
			}
		}

		public void EndRepair () {
			repairSite.Inventory["Labor"].onEmpty -= EndRepair;
			(project as MonoBehaviour).gameObject.SetActive (true);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
			//OnEndRepair (project);
			repairSite = null;
		}

		public virtual T SetObject<T> () where T : MonoBehaviour, IPathElementObject {
			IPathElementObject obj = ObjectPool.Instantiate<T> ();
			SetObject (obj);
			return obj as T;
		}

		protected void SetObject (IPathElementObject obj) {
			RemoveObject ();
			Element.Object = obj;
			Transform objTransform = ((MonoBehaviour)obj).transform;
			objTransform.SetParent (MyTransform);
			objTransform.localPosition = Anchor;
			objTransform.rotation = MyTransform.rotation;
			objTransform.localScale = MyTransform.localScale;
			OnSetObject (obj);
		}

		void RemoveObject () {
			if (Element.Object != null)
				ObjectPool.Destroy (((MonoBehaviour)Element.Object).transform);
		}

		public void SetFloodLevel (float floodLevel) {
			damageHandler.SetFloodLevel (floodLevel, Element);
		}

		void OnDamage (float damageAmount) {
			Unit u = project as Unit;
			if (u != null && u.Settings.TakesDamage) {
				Element.State = DevelopmentState.Damaged;
				BeginRepair (damageAmount);
			}
		}

		void OnDestroy () {
			Element.State = DevelopmentState.Abandoned;
		}

		protected virtual void OnSetObject (IPathElementObject obj) {}
		protected virtual void OnEndConstruction (IPathElementObject obj) {}
		//protected virtual void OnEndRepair (IPathElementObject obj) {}

		class DamageHandler {

			public delegate void OnDamage (float amount);
			public delegate void OnDestroy ();

			public OnDamage onDamage;
			public OnDestroy onDestroy;

			float damage = 0;
			public float Damage {
				get { return damage; }
			}

			bool Flooded {
				get { return startFloodTime > -1; }
				set { startFloodTime = value ? Time.time : -1; }
			}

			const float maxFloodTime = 120f;
			float startFloodTime = -1;
			DevelopmentState stateBeforeFlood;

			public DamageHandler (OnDamage onDamage, OnDestroy onDestroy) {
				this.onDestroy += onDestroy;
				this.onDamage += onDamage;
			}

			public void SetFloodLevel (float floodLevel, PathElement element) {
				if (floodLevel > 0) {
					if (!Flooded) {
						Flooded = true;
						stateBeforeFlood = element.State;
						element.State = DevelopmentState.Flooded;
					}
				} else {
					if (Flooded) {
						damage = Mathf.InverseLerp (0, maxFloodTime, Time.time - startFloodTime);
						element.State = stateBeforeFlood;
						if (element.State != DevelopmentState.Undeveloped) {
							if (Mathf.Approximately (damage, 1f)) {
								onDestroy ();
							} else if (damage > 0f) {
								onDamage (damage);
							}
						}
						Flooded = false;
					}
				}
			}
		}
	}
}