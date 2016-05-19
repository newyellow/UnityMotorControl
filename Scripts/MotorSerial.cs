using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;

public class MotorSerial : MonoBehaviour {

	// mac need PL2303 driver installed
	// and named like: /dev/tty.usbserial
	public string portStr = "COM1";
	SerialPort serial;

	List<string> messages;
	List<string> commandStack;

	string nextCommand = "";

	public bool displayLog = false;
	public int maxLineOfDisplay = 20;

	public bool startSending = false;
	public float sendingDelay = 0.05f;

	public GUIStyle displayStyle;

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

		string log = serial.ReadLine ();

		messages.Add (log);

		if (messages.Count > maxLineOfDisplay)
			messages.RemoveAt (0);

	}

	void OnGUI () {

		if (displayLog) {
			GUILayout.BeginArea (new Rect (50.0f, 50.0f, 500.0f, 500.0f));

			GUILayout.Label ("Serial Status: " + serial.IsOpen, displayStyle);

			GUILayout.Label ("Command Count: " + commandStack.Count, displayStyle);

			for (int i = 0; i < messages.Count; i++) {
				GUILayout.Label (messages [i], displayStyle);
			}

			GUILayout.EndArea ();
		}
	}

	IEnumerator commandRunner () {

		while (true) {

			if (commandStack.Count > 0) {
				serial.WriteLine (commandStack [0]);
				commandStack.RemoveAt (0);
			}

			yield return new WaitForSeconds (sendingDelay);
		}
	}

	public void AddCommand( string command ) {

		if( startSending )
			commandStack.Add (command);
	}

	public void MotorMoveToValue ( int motorIndex, int toValue ) {
		string command = "t" + motorIndex + "ima " + toValue;
		AddCommand (command);
	}

	void OnApplicationExit () {
		serial.Close ();
	}
}
