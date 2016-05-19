using UnityEngine;
using System.Collections;

public class MotorAdjuster : MonoBehaviour {

	public MotorSerial _serial;

	public int MotorIndex = 0;
	public int[] motorValues;
	public int speed = 10000;

	// Use this for initialization
	void Start () {
		StartCoroutine (SendValue());
	}
	
	// Update is called once per frame
	void Update () {
	
		for (int i = 0; i < motorValues.Length; i++) {
			if (Input.GetKeyDown ((i+1).ToString()))
				MotorIndex = i;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			motorValues[MotorIndex] += (int)(speed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			motorValues[MotorIndex] -= (int)(speed * Time.deltaTime);
		}
	}

	void OnGUI () {
		GUILayout.Label ("MOTOR INDEX:" + MotorIndex);
		GUILayout.Label ("VALUE: " + motorValues [MotorIndex]);
	}

	IEnumerator SendValue () {
		while (true) {

			_serial.MotorMoveToValue (MotorIndex, motorValues [MotorIndex]);

			yield return new WaitForSeconds (5.0f);
		}
	}
}
