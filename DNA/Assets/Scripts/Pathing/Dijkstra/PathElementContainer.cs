using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA {

	public class PathElementContainer : MBRefs {

		public virtual PathElement Element { get; protected set; }

		IPathElementObject project;

		public void BeginConstruction<T> () where T : MonoBehaviour, IPathElementObject {
			project = (IPathElementObject)ObjectPool.Instantiate<T> ();
			SetObject (ObjectPool.Instantiate<ConstructionSite> ());
			Element.State = DevelopmentState.UnderConstruction;
		}

		protected void SetObject (IPathElementObject obj) {
			Element.Object = obj;
			Transform objTransform = ((MonoBehaviour)obj).transform;
			objTransform.SetParent (MyTransform);
			objTransform.localPosition = Vector3.zero;
			objTransform.rotation = MyTransform.rotation;
			objTransform.localScale = MyTransform.localScale;
		}

		protected void EndConstruction () {
			ObjectPool.Destroy<ConstructionSite> (((MonoBehaviour)Element.Object).transform);
			SetObject (project);
			Element.State = DevelopmentState.Developed;
		}
	}
}