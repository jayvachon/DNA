﻿using UnityEngine;
using System.Collections;

namespace DNA.Paths {

	public enum DevelopmentState { 
		Undeveloped, 
		UnderConstruction, 
		Developed 
	}

	public class PathElement {

		DevelopmentState state = DevelopmentState.Undeveloped;
		public DevelopmentState State {
			get { return state; }
			set { state = value; }
		}

		public IPathElementObject Object { get; set; }

		static int version = 0;
		int myVersion = 0;

		public bool UpToDate { get { return myVersion == version; } }

		protected void UpdateVersion () {
			version ++;
		}

		public void SetUpToDate () {
			myVersion = version;
		}
	}
}