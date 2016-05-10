using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Paths {

	public class PathElementContainer : MBRefs, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

		public virtual PathElement Element { get; protected set; }
		protected virtual Vector3 Anchor { get { return Vector3.zero; } }

		Unit UnitProject {
			get { return project as Unit; }
		}

		int constructionCost;

		StaticUnit project;
		ConstructionSite site;
		RepairSite repairSite;
		
		enum PlotType { None, Default, Drillable }
		PlotType plotType = PlotType.None;

		DamageHandler damageHandler;

		void Awake () {
			damageHandler = new DamageHandler (OnDamage);
		}

		public ConstructionSite BeginConstruction<T> (int laborCost=0, bool autoConstruct=true) where T : StaticUnit {

			// Create a construction site and listen for labor to be completed
			// Set the project to turn into once labor completes
			if (Element.State == DevelopmentState.Undeveloped) {
				project = (StaticUnit)ObjectPool.Instantiate<T> ();
				project.gameObject.SetActive (false);
				site = (ConstructionSite)SetObject<ConstructionSite> ();
				site.Inventory["Labor"].onEmpty += EndConstruction;
				site.Inventory["Labor"].Capacity = laborCost;
				site.Inventory["Labor"].Fill ();
				site.PerformableTasks.Get<CancelConstruction> ().Init (this, project.Settings.Symbol);
				if (autoConstruct) site.AutoConstruct ();
				Element.State = DevelopmentState.UnderConstruction;
			}
			return site;
		}

		[DebuggableMethod ()]
		public void EndConstruction () {

			// Turn into the project set when construction began
			// Mark this container as developed so that nothing else can be built here

			site.Inventory["Labor"].onEmpty -= EndConstruction;
			project.gameObject.SetActive (true);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
			OnEndConstruction (project);
			site = null;
		}

		[DebuggableMethod ()]
		public void CancelConstruction () {
			site.Inventory["Labor"].onEmpty -= EndConstruction;
			Demolish ();
			site = null;
		}

		public void Demolish () {
			
			if (plotType == PlotType.Drillable)
				SetObject<DrillablePlot> ();
			else if (plotType == PlotType.Default)
				SetObject<Plot> ();

			project.gameObject.SetActive (true);
			ObjectPool.Destroy (project.transform);
			Element.State = DevelopmentState.Undeveloped;
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

		public virtual T SetObject<T> (bool destroyPrevious=true) where T : StaticUnit {

			StaticUnit obj = ObjectPool.Instantiate<T> ();
			SetObject (obj, destroyPrevious);

			if (typeof (T) == typeof (DrillablePlot))
				plotType = PlotType.Drillable;
			if (typeof (T) == typeof (Plot))
				plotType = PlotType.Default;

			return obj as T;
		}

		protected void SetObject (StaticUnit obj, bool destroyPrevious=true) {
			if (destroyPrevious) RemoveObject ();
			Element.Object = obj;
			Transform objTransform = ((MonoBehaviour)obj).transform;
			objTransform.SetParent (MyTransform);
			objTransform.localPosition = Anchor;
			objTransform.rotation = MyTransform.rotation;
			objTransform.localScale = MyTransform.localScale;
			obj.Container = this;
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

		protected virtual void OnSetObject (StaticUnit obj) {}
		protected virtual void OnEndConstruction (StaticUnit obj) {}

		#region IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler implementation
		public virtual void OnPointerDown (PointerEventData e) {}
		public virtual void OnPointerEnter (PointerEventData e) {}
		public virtual void OnPointerExit (PointerEventData e) {}
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