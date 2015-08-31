using UnityEngine;
using System.Collections;
using GameInventory;
using DNA.Tasks;

public class MockTaskPerformer : MonoBehaviour, ITaskPerformer, IInventoryHolder {

	PerformableTasks performableTasks = null;
	public PerformableTasks PerformableTasks { 
		get {
			if (performableTasks == null) {
				performableTasks = new PerformableTasks (this);
			}
			return performableTasks;
		}
	}
	
	RepeatTest repeat = new RepeatTest ();

	public Inventory Inventory { get; private set; }

	void Awake () {
		
		InitInventory ();		

		//TestGenerate<MilkshakeHolder> (new GenerateItemTest<MilkshakeHolder> ());
		TestConsume<CoffeeHolder> (new ConsumeItemTest<CoffeeHolder> ());
		//TestAutoStart (new AutoStartTest ());
		//TestRepeat (new RepeatTest ());
		//TestEnabled ();
	}

	void InitInventory () {
		Inventory = new Inventory (this);
		Inventory.Add (new MilkshakeHolder (5, 0));
		Inventory.Add (new CoffeeHolder (5, 5));
	}

	public void TestAutoStart (PerformerTask autoStart) {

		PerformableTasks.Add (autoStart);
		
		if (!autoStart.Settings.AutoStart)
			throw new System.Exception ("The task '" + autoStart.GetType () + "' will not auto start because its data model has AutoStart set to false");

		if (autoStart.Settings.Duration == 0)
			throw new System.Exception ("The test '" + autoStart.GetType () + "' will fail because its duratio is 0. Set it to something above 0");

		if (autoStart.Performing) 
			Debug.Log ("Auto Start test succeeded :)");
		else
			Debug.Log ("Auto Start test failed :(");
	}

	public void TestRepeat (PerformerTask repeat) {

		PerformableTasks.Add (repeat);
		repeat.onEnd += OnEndRepeat;

		if (!repeat.Settings.Repeat)
			throw new System.Exception ("The task '" + repeat.GetType () + "' will not repeat because its data model has Repeat set to false");

		repeat.Start ();
	}

	void OnEndRepeat () {
		Coroutine.WaitForFixedUpdate (() => {
			if (repeat.Performing) 
				Debug.Log ("Repeat test succeeded :)");
			else
				Debug.Log ("Repeat test failed :(");
			repeat.onEnd -= OnEndRepeat;
			repeat.Stop ();
		});
	}

	public void TestEnabled (PerformerTask task=null) {

		if (task != null) {
			task.Start ();
			if ((task.Enabled && task.Performing) || (!task.Enabled && !task.Performing))
				Debug.Log ("Enabled test succeeded :)");
			else
				Debug.Log ("Enabled test failed because enabled is " + task.Enabled + " but performing is " + task.Performing);
			return;
		}

		EnabledTest enabled = new EnabledTest ();
		enabled.enabled = false;
		bool failed = false;

		enabled.Start ();
		if (enabled.Performing) {
			Debug.Log ("Enabled test failed because the task was started but the task is disabled");
			failed = true;
		}

		enabled.enabled = true;
		enabled.Start ();
		if (!enabled.Performing) {
			Debug.Log ("Enabled test failed because the task is enabled but didn't start");
			failed = true;
		}

		if (!failed)
			Debug.Log ("Enabled test succeeded :)");
	}

	public void TestGenerate<T> (GenerateItem<T> gen) where T : ItemHolder {
		Inventory.Get<T> ().HolderFilled += () => Debug.Log ("Generate Item test succeeded :)");
		PerformableTasks.Add (gen);
		gen.Start ();
		if (!gen.Performing)
			Debug.Log ("Generate Item test failed because the task did not start");
	}

	public void TestConsume<T> (ConsumeItem<T> cons) where T : ItemHolder {
		Inventory.Get<T> ().HolderEmptied += () => Debug.Log ("Consume Item test succeeded :)");
		PerformableTasks.Add (cons);
		cons.Start ();
		if (!cons.Performing)
			Debug.Log ("Consume Item test failed because the task did not start");
	}
}
