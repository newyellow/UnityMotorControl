using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;

public class MotorCommandLine : MonoBehaviour {

	// mac need PL2303 driver installed
	// and named like: /dev/tty.usbserial
	public string portStr = "COM1";
	SerialPort serial;

	int maxLineOfDisplay = 30;
	List<string> messages;

	List<string> commandStack;

	float value1 = 0.0f;
	float value2 = 0.0f;
	float value3 = 0.0f;

	// Use this for initialization
	void Start () {
	
		messages = new List<string> ();
		commandStack = new List<string> ();

		serial = new SerialPort (portStr, 9600);
		serial.ReadTimeout = 10;
		serial.Open ();


		StartCoroutine (commandRunner ());

	}
	
	// Update is called once per frame
	void Update () {
		if (serial.IsOpen) {
		
			if (Input.GetKeyDown (KeyCode.Q))
				AddCommand ("ma 150000");

			if (Input.GetKeyDown (KeyCode.A))
				AddCommand ("ma 0");

			if (Input.GetKeyDown (KeyCode.W))
				AddCommand ("t1ma 150000");

			if (Input.GetKeyDown (KeyCode.S))
				AddCommand ("t1ma 0");

			if (Input.GetKeyDown (KeyCode.E))
				AddCommand ("t2ma 150000");

			if (Input.GetKeyDown (KeyCode.D))
				AddCommand ("t2ma 0");
				
		}
		else
			Debug.Log ("ERROR");

		string log = serial.ReadLine ();
		messages.Add (log);
		if (messages.Count > maxLineOfDisplay)
			messages.RemoveAt (0);

	}

	void OnGUI () {
		GUILayout.BeginArea (new Rect (50.0f, 50.0f, 500.0f, 500.0f));
		GUILayout.Label ("Command Count: " + commandStack.Count);

		value1 = GUILayout.HorizontalSlider (value1, 0.0f, 150000.0f);
		value2 = GUILayout.HorizontalSlider (value2, 0.0f, 150000.0f);
		value3 = GUILayout.HorizontalSlider (value3, 0.0f, 150000.0f);

		for (int i = 0; i < messages.Count; i++) {
			GUILayout.Label (messages [i]);
		}

		GUILayout.EndArea ();
	}

	IEnumerator commandRunner () {
		int index = 0;

		while (true) {

			if (commandStack.Count > 0) {
				serial.WriteLine (commandStack [0]);
				commandStack.RemoveAt (0);
			}

			index = (index + 1) % 3;

			if (index == 0)
				AddCommand ("ma " + (int)(value1));
			else if (index == 1)
				AddCommand ("t1ma " + (int)(value2));
			else if (index == 2)
				AddCommand ("t2ma " + (int)(value3));
			
			yield return new WaitForSeconds (0.15f);
		}
	}

	void AddCommand( string command ) {
		commandStack.Add (command);
	}

	void OnApplicationExit () {
		serial.Close ();
	}
}
