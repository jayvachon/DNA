using UnityEngine;
using System.Collections;
using System;

public class HTMLRenderer {

	class TagFinder {

		public Tag tag = null;
		public bool insideTag = false;
		bool flagInside = false;

		bool tagging = false;
		bool startTag = true;
		string tagName = "";

		public Tag InputChar (char c) {
			if (flagInside) {
				insideTag = true;
				flagInside = false;
			}
			if (c == '<') {
				insideTag = false;
				tagging = true;
				return null;
			}
			if (tagging) {
				if (c == '>') {
					tagging = false;
					flagInside = true;
					return CreateTag ();
				} else if (c == '/') {
					startTag = false;
				} else {
					tagName += c;
				}
			}
			return null;
		}

		Tag CreateTag () {
			tag = new Tag (tagName, startTag);
			tagging = false;
			startTag = true;
			tagName = "";
			return tag;
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

		string text = "";

		public Content InputChar (char c, bool insideTag, Tag tag) {
			if (insideTag) {
				text += c;
				return null;
			} else {
				return CreateContent (tag);
			}
		}

		Content CreateContent (Tag tag) {
			if (text == "") return null;
			Content c = new Content (text, tag);
			text = "";
			return c;
		}
	}

	class Content {

		public readonly string text;
		public readonly Tag tag;

		public Content (string text, Tag tag) {
			this.text = text;
			this.tag = tag;
		}
	}

	string html;

	public HTMLRenderer (string filePath) {
		this.html = System.IO.File.ReadAllText (filePath);
		DecodeHTML ();
	}

	void DecodeHTML () {
		TagFinder tagFinder = new TagFinder ();
		ContentFinder contentFinder = new ContentFinder ();
		foreach (char c in html) {
			if (c == '\n') continue;
			Tag t = tagFinder.InputChar (c);
			if (t != null) {
				Debug.Log (t.name + ", " + t.start);
			}
			Content content = contentFinder.InputChar (c, tagFinder.insideTag, tagFinder.tag);
			if (content != null) {
				Debug.Log (content.tag.name + ": " + content.text);
			}
		}
	}
}
