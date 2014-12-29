using UnityEngine;
using System.Collections;

public class Elder : InventoryItem {

	// Elders do not perform labor so they do not move around like MovableUnits do
	// Periodically they need to be taken to the hospital
	// Otherwise they stay in houses
	// ---- except maybe they should be able to move?? like, consider their happiness and everything
	// ------ except also this is not a judgement of elderly life, just what it seems to be
	// ------ in america today ??

	// if elders are not taken care of, the buildings they're inhabiting get shut down
	// (houses aren't kept up, hospitals fail)
	
}

/**
 *	Elders need to be picked up by MovableUnits and dropped off at other locations
 *	Each Elder has their own Health and any number of Ailments (or only 1 ailment at a time?)
 *	Health depletes as ailments are not managed, and if it runs out the Elder dies
 *	Ailments indicate which location an Elder must be taken to-
 *		e.g. heart specialist, eye specialist...
 *	Elders must eventually be returned to their homes so that they can make room for
 *		other sick people
 *	They're not explicitely "seen" in the game so there needs to be a way of indicating
 *		the status of Elders in buildings, and there needs to be a way to specify which
 *		Elders a MovableUnit will pick up when they stop by a building.
 *	MovableUnits eventually become Elders
 *	MovableUnits also need to be collecting IceCream and Milk to make MilkShakes
 */