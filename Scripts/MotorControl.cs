using UnityEngine;
using System.Collections;

public class MotorControl : MonoBehaviour {

	public MotorSerial _serial;
	public float motorMsgDelay = 0.3f;

	public Transform[] hangPoints;
	public Transform[] dollHangPoints;

	public float[] startLengths;
	public int[] MotorValues;

	public bool startSendingValues = false;

	// unit: CM
	float rollR = 30.0f;
	float lengthPerRoll = 0.0f;
	int motorValuePerRoll = 150000;

	void Awake () {
		lengthPerRoll = rollR * Mathf.PI;
		MotorValues = new int[hangPoints.Length];
		startLengths = new float[hangPoints.Length];

		for (int i = 0; i < hangPoints.Length; i++) {
			startLengths[i] = Vector3.Distance (hangPoints [i].position, dollHangPoints [i].position);
		}
	}

	void Start () {
		StartCoroutine (SendMotorControls ());
	}
	
	// Update is called once per frame
	void Update () {
	
		// calculate every motor value
		for (int i = 0; i < MotorValues.Length; i++) {
			float dist = Vector3.Distance (hangPoints [i].position, dollHangPoints [i].position);
			float diff = dist - startLengths [i];

			MotorValues [i] = (int)(motorValuePerRoll * diff * -1);
		}


		// debug drawline
		for (int i = 0; i < hangPoints.Length; i++) {
			Debug.DrawLine (hangPoints [i].position, dollHangPoints [i].position, Color.green);
		}
	}

	void OnGUI () {
		if (GUILayout.Button ("All Set To Zero")) {
			ResetAllMotorValues ();
		}
	}

	void ResetAllMotorValues () {
		for (int i = 0; i < hangPoints.Length; i++) {
			_serial.MotorMoveToValue (i, 0);
		}
	}

	IEnumerator SendMotorControls () {

		while (true) {

			if (startSendingValues) {
				for (int i = 0; i < hangPoints.Length; i++) {
					_serial.MotorMoveToValue (i, MotorValues [i]);
					yield return new WaitForSeconds (0.02f);
				}
			}

			yield return new WaitForSeconds ( motorMsgDelay );
		}
	}
}
