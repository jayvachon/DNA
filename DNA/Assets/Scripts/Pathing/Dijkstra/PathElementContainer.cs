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
			project = null;
		}

		public void BeginRepair () {
			if (Element.State == DevelopmentState.Damaged) {
				project = Element.Object;
				(project as MonoBehaviour).gameObject.SetActive (false);
				repairSite = (RepairSite)SetObject<RepairSite> ();
				repairSite.Inventory["Labor"].onEmpty += EndRepair;
				Element.State = DevelopmentState.UnderRepair;

				// TODO: Set repair cost
			}
		}

		public void EndRepair () {
			repairSite.Inventory["Labor"].onEmpty -= EndRepair;
			(project as MonoBehaviour).gameObject.SetActive (true);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
			//OnEndRepair (project);
			repairSite = null;
			project = null;
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
			Element.SetFloodLevel (floodLevel);
			if (Element.State != DevelopmentState.Flooded && Element.Damage > 0) {
				BeginRepair ();
			}
		}

		protected virtual void OnSetObject (IPathElementObject obj) {}
		protected virtual void OnEndConstruction (IPathElementObject obj) {}
		//protected virtual void OnEndRepair (IPathElementObject obj) {}
	}
}