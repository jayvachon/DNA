using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationCenter : MBRefs {

	static NotificationCenter instance = null;
	static public NotificationCenter Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (NotificationCenter)) as NotificationCenter;
				if (instance == null) {
					GameObject go = new GameObject ("NotificationCenter");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<NotificationCenter>();
				}
			}
			return instance;
		}
	}

	Notification notification = null;
	Notification Notification {
		get {
			if (notification == null) {
				notification = MyTransform.GetChild (0).GetComponent<Notification> ();
			}
			return notification;
		}
	}

	//List<string> queuedNotifications = new List<string> ();

	Dictionary<string, string> notifications;
	Dictionary<string, string> Notifications {
		get {
			if (notifications == null) {
				notifications = new Dictionary<string, string> ();
				notifications.Add ("laborerRetired", "A Laborer has just retired. Drop it on a clinic to extend its lifespan!");
				notifications.Add ("elderDied", "An Elder has just died. Drop its Remains on the Giving Tree to harvest its years!");
			}
			return notifications;
		}
	}

	public void ShowNotification (string id) {
		/*string content;
		if (Notifications.TryGetValue (id, out content)) {
			ShowCustomNotification (content);
			Notifications.Remove (id);
		}*/
	}

	public void ShowCustomNotification (string content) {
		/*queuedNotifications.Add (content);
		if (queuedNotifications.Count == 1) {
			Notification.SetContent (content);
		}*/
	}

	public void RemoveNotification (string content) {
		/*queuedNotifications.Remove (content);
		if (queuedNotifications.Count > 0) {
			Notification.SetContent (queuedNotifications[0]);
		}*/
	}
}
