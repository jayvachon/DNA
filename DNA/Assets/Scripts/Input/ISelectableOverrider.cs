using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DNA.InputSystem {
	
	public interface ISelectableOverrider {
		PointerEventData.InputButton OverrideButton { get; }
		void OnOverrideSelect (ISelectable overridenSelectable);
	}
}