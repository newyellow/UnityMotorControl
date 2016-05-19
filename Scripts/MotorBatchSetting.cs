using UnityEngine;
using System.Collections;

public class MotorBatchSetting : MonoBehaviour {

	public MotorSerial _serial;
	public int motorNums = 8;

	string commandToExecute = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUILayout.BeginArea (new Rect (0.0f, 0.0f, 500.0f, 500.0f));

		commandToExecute = GUILayout.TextField (commandToExecute);
		if (GUILayout.Button ("Run")) {
			BatchCommand (commandToExecute);
			commandToExecute = "";
		}

		GUILayout.EndArea ();
	}

	void BatchCommand ( string command ) {

		// batch ask
		if (command [0] == '?') {
			for (int i = 0; i < motorNums; i++) {
				string askCommand = "?t" + i + command.Substring (1);
				_serial.AddCommand (askCommand);
			}
		}

		// batch execute
		else {
			for (int i = 0; i < motorNums; i++) {
				string tiCommand = "t" + i + command;
				_serial.AddCommand (tiCommand);
			}
		}
	}
}
