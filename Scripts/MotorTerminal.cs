using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;

public class MotorTerminal : MonoBehaviour {

	// mac need PL2303 driver installed
	// and named like: /dev/tty.usbserial
	public string portStr = "COM1";
	SerialPort serial;

	int maxLineOfDisplay = 30;
	List<string> messages;

	List<string> commandStack;

	string nextCommand = "";

	public bool executeCommandManually = false;

	// Use this for initialization
	void Start () {

		messages = new List<string> ();
		commandStack = new List<string> ();

		serial = new SerialPort (portStr, 9600);
		serial.ReadTimeout = 10;
		serial.Open ();


		if( !executeCommandManually )
			StartCoroutine (commandRunner ());

	}

	// Update is called once per frame
	void Update () {
		
		string log = serial.ReadLine ();

		messages.Add (log);

		if (messages.Count > maxLineOfDisplay)
			messages.RemoveAt (0);

	}

	void OnGUI () {
		GUILayout.BeginArea (new Rect (50.0f, 50.0f, 500.0f, 500.0f));

		GUILayout.Label ("Serial Status: " + serial.IsOpen);

		nextCommand = GUILayout.TextField (nextCommand);

		if (GUILayout.Button ("Send Command")) {
			AddCommand (nextCommand);
			nextCommand = "";
		}

		if (executeCommandManually) {
			GUILayout.Space (5);
			if (GUILayout.Button ("Send Command"))
				StartCoroutine ("executeCommands");
		}

		GUILayout.Label ("Command Count: " + commandStack.Count);

		for (int i = 0; i < messages.Count; i++) {
			GUILayout.Label (messages [i]);
		}

		GUILayout.EndArea ();
	}

	IEnumerator commandRunner () {

		while (true) {
			
			if (commandStack.Count > 0) {
				serial.WriteLine (commandStack [0]);
				commandStack.RemoveAt (0);
			}

			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator executeCommands () {

		while (true) {
			if (commandStack.Count > 0) {
				serial.WriteLine (commandStack [0]);
				commandStack.RemoveAt (0);

				yield return new WaitForSeconds (0.1f);
			} else
				break;
				
		}
	}

	void AddCommand( string command ) {
		commandStack.Add (command);
	}

	void OnApplicationExit () {
		serial.Close ();
	}
}
