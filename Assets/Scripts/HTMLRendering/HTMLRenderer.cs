using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HTMLRenderer {

	class TagFinder {

		string name = "";
		bool startTag = true;
		bool recording = false;
		bool flagEndRecording = false;
		public bool Recording {
			get { return recording; }
		}

		public Tag InputChar (char c) {
			if (flagEndRecording) {
				flagEndRecording = false;
				recording = false;
			}
			switch (c) {
				case '<': OpenTag (); break;
				case '/': FlagEndTag (); break;
				case '>': return CloseTag ();
				default: AppendName (c); break;
			}
			return null;
		}

		void OpenTag () {
			recording = true;
		}

		void FlagEndTag () {
			if (recording) {
				startTag = false;
			}
		}

		Tag CloseTag () {
			Tag t = new Tag (name, startTag);
			Reset ();
			return t;
		}

		void AppendName (char c) {
			if (recording) {
				name += c;
			}
		}

		void Reset () {
			name = "";
			startTag = true;
			flagEndRecording = true;
		}
	}

	class TagHandler {

		List<Tag> tags = new List<Tag> ();
		public List<Tag> Tags {
			get { return tags; }
		}

		Tag lastStartTag;
		public Tag LastStartTag {
			get { return lastStartTag; }
		}

		Tag lastEndTag;
		public Tag LastEndTag {
			get { return lastEndTag; }
		}

		public void InputTag (Tag t) {
			if (t == null) return;
			if (t.start) {
				lastStartTag = t;
				tags.Add (t);
			} else {
				for (int i = 0; i < tags.Count; i ++) {
					if (tags[i].name == t.name) {
						lastEndTag = tags[i];
						tags.Remove (tags[i]);
					}
				}
			}
		}
	}

	class Tag {

		public readonly string name; // what's inside the brackets
		public readonly bool start;  // whether it's a start tag (true) or end tag (false)
		public readonly bool single; // such as img

		public Tag (string name, bool start, bool single=false) {
			this.name = name;
			this.start = start;
			this.single = single;
		}
	}

	class ContentFinder {

		bool recording = false;
		string text = "";
		Tag currentTag = null;

		public Content InputChar (List<Tag> tagsList, Tag lastStartTag, Tag lastEndTag, char c) {
			
			/*if (lastEndTag != null) {
				if (lastEndTag.name == currentTag.name) {
					Debug.Log (text);
					text = "";
					currentTag = null;
				}
			}

			if (lastStartTag != null) {
				if (currentTag == null) {
					currentTag = lastStartTag;
				}
			}

			if (currentTag != null) {
				text += c;
			}*/

			return null;
		}
	}

	class Content {

		public readonly string text;
		public readonly List<Tag> tags = new List<Tag>();

		public Content (string text, List<Tag> tags) {
			this.text = text;
			this.tags = tags;
			Debug.Log (text);
		}
	}

	string html;

	public HTMLRenderer (string filePath) {
		this.html = System.IO.File.ReadAllText (filePath);
		DecodeHTML ();
	}

	void DecodeHTML () {
		TagFinder tagFinder = new TagFinder ();
		TagHandler tagHandler = new TagHandler ();
		ContentFinder contentFinder = new ContentFinder ();
		foreach (char c in html) {
			if (c == '\n') continue;
			Tag t = tagFinder.InputChar (c);
			tagHandler.InputTag (t);
			if (tagFinder.Recording) continue;
			/*Debug.Log ("=================");
			if (tagHandler.LastStartTag != null) {
				Debug.Log ("start: " + tagHandler.LastStartTag.name);
			}
			if (tagHandler.LastEndTag != null) {
				Debug.Log ("end: " + tagHandler.LastEndTag.name);
			}*/
			contentFinder.InputChar (tagHandler.Tags, tagHandler.LastStartTag, tagHandler.LastEndTag, c);
		}
	}
}
