#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameObjectMethodDebugger : EditorWindow {

	DebuggableComponent[] components;
	Vector2 scrollPos;

	[MenuItem ("Window/Game Object Method Debugger")]
	static void Init () {
		GetWindow<GameObjectMethodDebugger> ().Show ();
	}

	void GetMethodsFromSelection (GameObject selected) {

		components = new DebuggableComponent[0];
		Component[] goComponent = Selection.activeGameObject.GetComponents<MonoBehaviour> ();

		foreach (Component c in goComponent) {
			MethodInfo[] info = c.GetType ().GetMethods (BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			info = Array.FindAll (info, x => Array.Find (x.GetCustomAttributes (true), a => a.ToString() == "DebuggableMethod") != null);
			if (info.Length > 0) {
				components = components.Concat (new [] { new DebuggableComponent (c, info) }).ToArray ();
			}
		}
	}

	void OnGUI () {
		if (Selection.activeGameObject == null) {
			EditorGUILayout.HelpBox ("No game object selected.", MessageType.Info);
			return;
		}
		if (components == null || components.Length == 0) {
			EditorGUILayout.HelpBox ("No methods have been marked debuggable. Add the [DebuggableMethod] attribute to a method in the selected game object's class.", MessageType.Info);
			return;
		}

		scrollPos = GUILayout.BeginScrollView (scrollPos, false, false, GUILayout.Width (position.width), GUILayout.Height (position.height));

		foreach (DebuggableComponent c in components) {
			if (c == null || c.Component == null)
				continue;
			GUILayout.Label (c.Component.name);
			foreach (MethodInfo m in c.Methods) {
				
				foreach (ParameterInfo p in c.Parameters[m]) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (p.Name);
					System.Type paramType = p.ParameterType;
					if (paramType == typeof (int)) {
						c.SetValue<int> (p, EditorGUILayout.IntField ((int)c.GetValue<int> (p)));
					} else if (paramType == typeof (bool)) {
						c.SetValue<bool> (p, EditorGUILayout.Toggle ((bool)c.GetValue<bool> (p)));
					} else if (paramType == typeof (float)) {
						c.SetValue<float> (p, EditorGUILayout.FloatField ((float)c.GetValue<float> (p)));
					} else if (paramType == typeof (double)) {
						c.SetValue<double> (p, EditorGUILayout.DoubleField ((double)c.GetValue<double> (p)));
					} else if (paramType == typeof (string)) {
						c.SetValue<string> (p, EditorGUILayout.TextField ((string)c.GetValue<string> (p)));
					} else if (paramType == typeof (Color)) {
						c.SetValue<Color> (p, EditorGUILayout.ColorField ((Color)c.GetValue<Color> (p)));
					} else if (paramType == typeof (Bounds)) {
						c.SetValue<Bounds> (p, EditorGUILayout.BoundsField ((Bounds)c.GetValue<Bounds> (p)));
					} else if (paramType == typeof (long)) {
						c.SetValue<long> (p, EditorGUILayout.LongField ((long)c.GetValue<long> (p)));
					} else if (paramType == typeof (Rect)) {
						c.SetValue<Rect> (p, EditorGUILayout.RectField ((Rect)c.GetValue<Rect> (p)));
					} else if (paramType == typeof (Vector2)) {
						c.SetValue<Vector2> (p, EditorGUILayout.Vector2Field (p.Name, (Vector2)c.GetValue<Vector2> (p)));
					} else if (paramType == typeof (Vector3)) {
						c.SetValue<Vector3> (p, EditorGUILayout.Vector3Field (p.Name, (Vector3)c.GetValue<Vector3> (p)));
					} else if (paramType == typeof (Vector4)) {
						c.SetValue<Vector4> (p, EditorGUILayout.Vector4Field (p.Name, (Vector4)c.GetValue<Vector4> (p)));						
					} else {
						GUILayout.Label ("The type '" + p.ParameterType + "' is unsupported. Be sure it has a default value, otherwise the method will not run.");
					}
					GUILayout.EndHorizontal ();
				}

				if (GUILayout.Button (m.Name)) {
					try {
						m.Invoke (c.Component, c.GetParameters (m));
					} catch {
						throw new System.Exception ("Could not invoke because the method has unsupported parameter types without default values.");
					}
				}
			}
		}
		GUILayout.EndScrollView ();
	}

	void OnSelectionChange () {
		SetSelection ();
	}

	void OnFocus () {
		SetSelection ();
	}

	void SetSelection () {
		if (Selection.activeGameObject == null) {
			components = null;
			return;
		}
		GetMethodsFromSelection (Selection.activeGameObject);
		Repaint ();
	}

	class DebuggableComponent {

		public readonly Component Component;
		public readonly MethodInfo[] Methods;
		public readonly Dictionary<MethodInfo, ParameterInfo[]> Parameters = new Dictionary<MethodInfo, ParameterInfo[]> ();

		Dictionary<ParameterInfo, object> values = new Dictionary<ParameterInfo, object> ();

		System.Type[] supportedTypes = new System.Type[] {
			typeof (int), typeof (float), typeof (bool), 
			typeof (double), typeof (string), typeof (Color), 
			typeof (Bounds), typeof (long), typeof (Rect), 
			typeof (Vector2), typeof (Vector3), typeof (Vector4)
		};

		public DebuggableComponent (Component component, MethodInfo[] methods) {
			Component = component;
			Methods = methods;
			foreach (MethodInfo method in Methods) {
				Parameters.Add (method, method.GetParameters ());
			}
		}

		public void SetValue<T> (ParameterInfo p, T val) {
			if (values.ContainsKey (p)) {
				values[p] = val;
			} else {
				values.Add (p, val);
			}
		}

		public object GetValue<T> (ParameterInfo p) {
			object i;
			if (values.TryGetValue (p, out i)) {
				return i;
			}
			values.Add (p, default (T));
			return values[p];
		}

		public object[] GetParameters (MethodInfo m) {
			ParameterInfo[] parameters = Parameters[m];
			object[] paramVals = new object[parameters.Length];
			for (int i = 0; i < parameters.Length; i ++) {
				ParameterInfo p = parameters[i];
				object val = paramVals[i];
				if (supportedTypes.Contains (p.ParameterType)) {
					val = GetValue<object> (p);
				} else {
					val = p.DefaultValue;
				}
				paramVals[i] = val;
			}
			return paramVals;
		}
	}
}
#endif