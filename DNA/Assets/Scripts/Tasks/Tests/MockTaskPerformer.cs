using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.Units;
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
	
	public Inventory Inventory { get; private set; }

	public MockTaskAcceptor taskAcceptor;
	public MockTaskAcceptor2 taskAcceptor2;

	void Awake () {
		
		InitInventory ();

		//TestBinding (this, taskAcceptor, taskAcceptor2);
		//TestMatching (this, taskAcceptor);
		//TestGenerateUnit (new GenerateUnitTest<Laborer> ());

		//TestGenerate<MilkshakeHolder> (new GenerateItemTest<MilkshakeHolder> ());
		//TestConsume<CoffeeHolder> (new ConsumeItemTest<CoffeeHolder> ());
		//TestAutoStart (new AutoStartTest ());
		//TestRepeat (new RepeatTest ());
		//TestEnabled ();

		/*TestCollect<YearGroup> (
			new CollectItemTest<YearGroup> (),
			taskAcceptor.AcceptableTasks[typeof (AcceptCollectItemTest<YearGroup>)] as AcceptCollectItemTest<YearGroup>);*/

		/*TestDeliver<YearGroup> (
			new DeliverItemTest<YearGroup> (),
			taskAcceptor.AcceptableTasks[typeof (AcceptDeliverItemTest<YearGroup>)] as AcceptDeliverItemTest<YearGroup>);*/
	}

	void InitInventory () {
		Inventory = new Inventory (this);
		Inventory.Add (new MilkshakeGroup (5, 5));
		Inventory.Add (new CoffeeGroup (5, 5));
		Inventory.Add (new YearGroup (0, 5));
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
		repeat.onEnd += (PerformerTask task) => {
			Co2.WaitForFixedUpdate (() => {
				if (repeat.Performing) 
					Debug.Log ("Repeat test succeeded :)");
				else
					Debug.Log ("Repeat test failed :(");
				repeat.Stop ();
			});
		};

		if (!repeat.Settings.Repeat)
			throw new System.Exception ("The task '" + repeat.GetType () + "' will not repeat because its data model has Repeat set to false");

		repeat.Start ();
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

	public void TestGenerate<T> (GenerateItem<T> gen) where T : ItemGroup {
		gen.onComplete += (PerformerTask task) => Debug.Log ("Generate Item test succeeded :)");
		PerformableTasks.Add (gen);
		gen.Start ();
		if (!gen.Performing)
			Debug.Log ("Generate Item test failed because the task did not start");
	}

	public void TestConsume<T> (ConsumeItem<T> cons) where T : ItemGroup {
		cons.onComplete += (PerformerTask task) => Debug.Log ("Consume Item test succeeded :)");
		PerformableTasks.Add (cons);
		cons.Start ();
		if (!cons.Performing)
			Debug.Log ("Consume Item test failed because the task did not start");
	}

	public void TestDeliver<T> (DeliverItem<T> deliver, AcceptDeliverItem<T> acceptDeliver) where T : ItemGroup {

		T group = Inventory.Get<T> ();
		group.Set (5);
		PerformableTasks.Add (deliver);

		// Make sure the task doesn't start if the acceptor's inventory is full
		taskAcceptor.FillGroup<T> ();
		deliver.Start (acceptDeliver);
		if (deliver.Performing) {
			Debug.Log ("Deliver Item test failed because the task started but acceptor's inventory is full");
			return;
		}

		taskAcceptor.ClearGroup<T> ();
		deliver.onComplete += (PerformerTask task) => {
			taskAcceptor.ClearGroup<T> ();
			deliver.Start (acceptDeliver);

			// Make sure the task doesn't start if the performer's inventory is empty
			if (deliver.Performing)
				Debug.Log ("Deliver Item test failed bacause the task started but performer's inventory is empty");
			else
				Debug.Log ("Deliver Item test succeeded :)");
		};

		deliver.Start (acceptDeliver);
		if (!deliver.Performing)
			Debug.Log ("Deliver Item test failed because the task did not start");
	}

	public void TestCollect<T> (CollectItem<T> collect, AcceptCollectItem<T> acceptCollect) where T : ItemGroup {
		
		T group = Inventory.Get<T> ();
		group.Clear ();
		PerformableTasks.Add (collect);

		// Make sure task doesn't start if the acceptor's inventory is empty
		taskAcceptor.ClearGroup<T> ();
		collect.Start (acceptCollect);
		if (collect.Performing) {
			Debug.Log ("Collect Item test failed because the task started but acceptor's inventory is empty");
			return;
		}

		taskAcceptor.FillGroup<T> ();
		collect.onComplete += (PerformerTask task) => {
			taskAcceptor.FillGroup<T> ();
			collect.Start (acceptCollect);

			// Make sure the task doesn't start if the performer's inventory is full
			if (collect.Performing)
				Debug.Log ("Collect Item test failed because the task started but performer's inventory is full");
			else
				Debug.Log ("Collect Item test succeeded :)");
		};

		collect.Start (acceptCollect);
		if (!collect.Performing)
			Debug.Log ("Collect Item test failed because the task did not start");
	}

	public void TestGenerateUnit<T> (GenerateUnit<T> gen) where T : Unit {
		PerformableTasks.Add (gen);
		gen.Start ();
	}

	public void TestMatching (ITaskPerformer performer, ITaskAcceptor acceptor) {
		List<PerformerTask> matches = TaskMatcher.GetEnabled (performer, acceptor);
		foreach (PerformerTask m in matches)
			Debug.Log (m);
	}

	public void TestBinding (ITaskPerformer performer, ITaskAcceptor acceptor, ITaskAcceptor acceptorPair) {
		
		/*MatchResult match = TaskMatcher.GetPerformable (performer, acceptor, acceptorPair);
		if (match != null) {
			Debug.Log (match.Match);
			if (!match.NeedsPair) {
				Debug.Log ("starting");
				match.Match.onComplete += (PerformerTask task) => { Debug.Log ("finished"); };
				match.Start ();
			} else {
				Debug.Log ("Needs pair of type " + match.PairType);
				// TODO: Try to find a pair in the world
				// if a pair was found, path to it
				// else, perform the block below this one
			}
		} else {
			match = TaskMatcher.GetPerformable (performer, acceptorPair, acceptor);
			if (match == null) {
				// Stop moving
				Debug.Log ("stop moving");
			} else {
				// Move to the acceptor pair
				Debug.Log ("move to other point on path");
			}
		}*/
	}
}
