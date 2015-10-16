using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class PathElementContainer : MBRefs {

		public virtual PathElement Element { get; protected set; }
		protected virtual Vector3 Anchor { get { return Vector3.zero; } }

		IPathElementObject project;

		public void BeginConstruction<T> () where T : MonoBehaviour, IPathElementObject {
			if (Element.State == DevelopmentState.Undeveloped) {
				project = (IPathElementObject)ObjectPool.Instantiate<T> ();
				SetObject<ConstructionSite> ();
				Element.State = DevelopmentState.UnderConstruction;
			}
		}

		public virtual void SetObject<T> () where T : MonoBehaviour, IPathElementObject {
			SetObject (ObjectPool.Instantiate<T> ());
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
			SetObject (project);
			Element.State = DevelopmentState.Developed;
		}

		void RemoveObject () {
			if (Element.Object != null)
				ObjectPool.Destroy (((MonoBehaviour)Element.Object).transform);
		}

		protected virtual void OnSetObject (IPathElementObject obj) {}
	}
}