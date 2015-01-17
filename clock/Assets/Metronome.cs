using UnityEngine;
using System;
using System.Collections;

public class Metronome : MonoBehaviour 
{
	public struct Timing
	{
		public float beats;
		public int measures;
		public float totalBeats;
	}
	
	public class Settings
	{
		float bpm;
		public float BPM
		{
			get{ return bpm; }
			set
			{
				if( value <= 0 ) return;
				bpm = value;
				beatTime = 60f/bpm;
			}
		}
		
		float beatTime;
		
		int beatsPerMeasure;
		
		public Settings( float bpm = 120f, int beatsPerMeasure = 4 )
		{
			this.BPM = bpm;
			this.beatsPerMeasure = beatsPerMeasure;
		}
		
		public Timing GetTimingData( float realTime )
		{
			Timing t = new Timing();
			t.totalBeats = realTime / beatTime;
			t.measures = (int)(t.totalBeats/beatsPerMeasure);
			t.beats = t.totalBeats % beatsPerMeasure + 1;
			return t;
		}
		
		public string PrettyString()
		{
			return string.Format( "{0}bpm ({1}/4)", bpm, beatsPerMeasure );	
		}
	}
	
	static float debugYPos =  10f;
	float myDebugYPos;
	
	Settings settings;
	Timing currentTiming;
	bool playing = false;
	float timeSinceStart;
	
	public static Metronome Create( Settings settings, bool playImmediately = true )
	{
		GameObject go = new GameObject( "Metro: " + settings.PrettyString(), typeof(Metronome) );
		Metronome metro = go.GetComponent<Metronome>();
		metro.settings = settings;
		
		if( playImmediately ) metro.Play();
		
		metro.myDebugYPos = debugYPos;
		debugYPos += 20f;
		
		return metro;
	}
	
	public void Play( bool reset = true )
	{
		if( reset ) timeSinceStart = 0f;
		playing = true;
	}
	public void Stop(){playing = false;}
	
	void Update()
	{
		if( playing )
		{
			currentTiming = settings.GetTimingData( timeSinceStart );
			timeSinceStart += Time.deltaTime;
		}
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect(10,myDebugYPos,500,30), 
			string.Format( "{0}: {1}:{2:f2} (total beats: {3:f3})", 
			name, currentTiming.measures, currentTiming.beats, currentTiming.totalBeats ) );
	}
}