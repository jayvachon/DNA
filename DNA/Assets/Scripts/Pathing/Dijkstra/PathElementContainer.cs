using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class PathElementContainer : MBRefs {

		public virtual PathElement Element { get; protected set; }
		protected virtual Vector3 Anchor { get { return Vector3.zero; } }

		IPathElementObject project;
		ConstructionSite site;

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

		void RemoveObject () {
			if (Element.Object != null)
				ObjectPool.Destroy (((MonoBehaviour)Element.Object).transform);
		}

		protected virtual void OnSetObject (IPathElementObject obj) {}
		protected virtual void OnEndConstruction (IPathElementObject obj) {}
	}
}