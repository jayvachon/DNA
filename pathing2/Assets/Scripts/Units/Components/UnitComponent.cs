using UnityEngine;
using System.Collections;
using Pathing;

namespace Units {

	public class UnitComponent : MBRefs {

		protected virtual int ParentUnit { get { return 1; } }

		Unit unit = null;
		public Unit Unit {
			get {
				if (unit == null) {
					unit = transform.GetNthParent (ParentUnit).GetScript<Unit> ();
				} 
				return unit;
			}
		}

		MobileUnit mobileUnit = null;
		public MobileUnit MobileUnit {
			get {
				if (mobileUnit == null) {
					mobileUnit = Unit as MobileUnit;
					#if UNITY_EDITOR
					if (mobileUnit == null) {
						Debug.Log (string.Format ("{0} is not type MobileUnit", Unit.Name));
					}
					#endif
				}
				return mobileUnit;
			}
		}

		StaticUnit staticUnit = null;
		public StaticUnit StaticUnit {
			get {
				if (staticUnit == null) {
					staticUnit = Unit as StaticUnit;
					#if UNITY_EDITOR
					if (staticUnit == null) {
						Debug.Log (string.Format ("{0} is not type StaticUnit", Unit.Name));
					}
					#endif
				}
				return staticUnit;
			}
		}

		MobileUnitTransform mobileTransform = null;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = MobileUnit.MobileTransform;
				}
				return mobileTransform;
			}
		}
	}
}