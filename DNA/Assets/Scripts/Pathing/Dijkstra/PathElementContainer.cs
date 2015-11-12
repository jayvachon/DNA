using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA {

	public class PathElementContainer : MBRefs, IPointerDownHandler {

		public virtual PathElement Element { get; protected set; }
		protected virtual Vector3 Anchor { get { return Vector3.zero; } }

		Unit UnitProject {
			get { return project as Unit; }
		}

		int constructionCost;

		IPathElementObject project;
		ConstructionSite site;
		RepairSite repairSite;

		DamageHandler damageHandler;

		void Awake () {
			damageHandler = new DamageHandler (OnDamage);
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

		void BeginRepair (float damageAmount) {
			if (Element.State == DevelopmentState.Damaged) {
				if (UnitProject.gameObject.activeSelf) {
					UnitProject.gameObject.SetActive (false);
					repairSite = (RepairSite)SetObject<RepairSite> (false);
					repairSite.Inventory["Labor"].onEmpty += EndRepair;
				}
				Element.State = DevelopmentState.UnderRepair;
			}
			if (Element.State == DevelopmentState.UnderRepair) {
				repairSite.LaborCost = (int)(DataManager.GetConstructionCost (UnitProject.Settings.Symbol) * damageAmount);
			}
		}

		void EndRepair () {
			repairSite.Inventory["Labor"].onEmpty -= EndRepair;
			UnitProject.gameObject.SetActive (true);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
			repairSite = null;
		}

		void Abandon () {
			if (Element.State == DevelopmentState.UnderRepair)
				EndRepair ();
			Element.State = DevelopmentState.Abandoned;
		}

		public virtual T SetObject<T> (bool destroyPrevious=true) where T : MonoBehaviour, IPathElementObject {
			IPathElementObject obj = ObjectPool.Instantiate<T> ();
			SetObject (obj, destroyPrevious);
			return obj as T;
		}

		protected void SetObject (IPathElementObject obj, bool destroyPrevious=true) {
			if (destroyPrevious) RemoveObject ();
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

		[DebuggableMethod ()]
		public void SetFloodLevel (float floodLevel) {
			damageHandler.SetFloodLevel (floodLevel, Element);
		}

		void OnDamage (float damageAmount) {
			if (Mathf.Approximately (0f, damageAmount)) return;
			Unit u = project as Unit;
			if (u != null && u.Settings.TakesDamage) {
				if (damageAmount < 1f) {
					Element.State = DevelopmentState.Damaged;
					BeginRepair (damageAmount);
				} else {
					Abandon ();
				}
			}
		}

		protected virtual void OnSetObject (IPathElementObject obj) {}
		protected virtual void OnEndConstruction (IPathElementObject obj) {}

		#region IPointerDownHandler implementation
		public virtual void OnPointerDown (PointerEventData e) {

		}
		#endregion

		class DamageHandler {

			public delegate void OnDamage (float amount);

			public OnDamage onDamage;

			float damage = 0;
			public float Damage {
				get { return damage; }
			}

			bool Flooded {
				get { return startFloodTime > -1; }
				set { startFloodTime = value ? Time.time : -1; }
			}

			const float maxFloodTime = 10f;//120f;
			float startFloodTime = -1;
			DevelopmentState stateBeforeFlood;

			public DamageHandler (OnDamage onDamage) {
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
						damage += Mathf.Clamp01 (Mathf.InverseLerp (0, maxFloodTime, Time.time - startFloodTime));
						element.State = stateBeforeFlood;
						if (element.State != DevelopmentState.Undeveloped) {
							onDamage (damage);
						}
						Flooded = false;
					}
				}
			}
		}
	}
}