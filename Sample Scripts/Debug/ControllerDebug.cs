using UnityEngine;
using UnityEngine.UI;

public class ControllerDebug : MonoBehaviour
{
	[SerializeField]
	private Text rotText;
	[SerializeField]
	private Text accText;
	[SerializeField]
	private Text gyroText;
	[SerializeField]
	private GameObject controller;

	void Update() {
		rotText.text = "rot: " + controller.transform.eulerAngles;
		// accText.text = "acc: " + PicoVRManager.SDK.GetBoxSensorAcc();
		// gyroText.text = "gyr: " + PicoVRManager.SDK.GetBoxSensorGyr();
	}
}