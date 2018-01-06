using UnityEngine;
using System.Collections;

// This defines the keyboard and handler control for the FPS charactor
public class FPSControl : MonoBehaviour {

    CharacterController cc;
	GameObject fpsController;
    GameObject miller;
	// Use this for initialization
	void Start () {
        cc = GameObject.Find("FPSController").GetComponent<CharacterController>();
        miller = GameObject.Find("Miller");
		fpsController = GameObject.Find ("FPSController");
		//fpsController.transform.localRotation = Quaternion.identity;
        GameObject.Find("FirstPersonCharacter").GetComponent<Camera>().enabled = true;

#if UNITY_ANDROID
        //GameObject.Find("FirstPersonCharacter").GetComponent<Camera>().enabled = false;
        //GameObject.Find("Cardboard_Camera").GetComponent<Camera>().enabled = true;
        Input.gyro.enabled = true;
		AttachGyro();
#endif
	}

	Quaternion temp = new Quaternion();
	float yaw = 0.0f, pitch = 0.0f, roll = 0.0f;
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		yaw = transform.rotation.eulerAngles.y;
		#endif

		// Corresponding direction movement with key pressed (PC+joystick)
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);
        float speedunit = 3.0f;
		Quaternion dir = Quaternion.Euler (0.0f, yaw, 0.0f);
		if (Input.GetKey(KeyCode.UpArrow)) move += speedunit * (dir * new Vector3(0.0f, 0.0f, 1.0f));
		if (Input.GetKey(KeyCode.DownArrow)) move += speedunit * (dir * new Vector3(0.0f, 0.0f, -1.0f));
		//move += speedunit * (dir * new Vector3(Input.GetAxis("Left TriggerX"), 0.0f, 0.0f));
		if (Input.GetKey(KeyCode.LeftArrow)) move += speedunit * (dir * new Vector3(-1.0f, 0.0f, 0.0f));
		if (Input.GetKey(KeyCode.RightArrow)) move += speedunit * (dir * new Vector3(1.0f, 0.0f, 0.0f));
		//move += speedunit * (dir * new Vector3(0.0f, 0.0f, -Input.GetAxis("Left TriggerY")));
		cc.SimpleMove(move);

#if UNITY_ANDROID
		if (!gyroEnabled)
			return;
		transform.rotation = Quaternion.Slerp(transform.rotation,
			cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
#else
		// Corresponding direction rotation with key pressed
		if (Input.GetKey (KeyCode.A)) yaw -= Time.deltaTime * 40;
		if (Input.GetKey (KeyCode.D))  yaw += Time.deltaTime * 40;
		//yaw += Time.deltaTime * 20.0f * Input.GetAxis ("Right TriggerX");
		fpsController.transform.localRotation = Quaternion.Euler (0.0f, yaw, 0.0f);

		if (Input.GetKey (KeyCode.W)) pitch -= Time.deltaTime * 30.0f;
		if (Input.GetKey (KeyCode.S)) pitch += Time.deltaTime * 30.0f;
		//pitch += Time.deltaTime * 30.0f * Input.GetAxis ("Right TriggerY");
		if (pitch > 65.0f) pitch = 65.0f;
		if (pitch < -65.0f) pitch = -65.0f;
		fpsController.transform.localRotation *= Quaternion.Euler (pitch, 0.0f, 0.0f);
#endif
	}

	/*
	void OnGUI(){
		GUI.TextArea(new Rect(0, 20, 200, 20), "Current Yaw : " + yaw);
		GUI.TextArea(new Rect(0, 40, 200, 20), "Current Pitch : " + pitch);
		GUI.TextArea(new Rect(0, 60, 200, 20), "Current Roll : " + roll);
	}
	*/



	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;
	private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
	private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);
	private Quaternion cameraBase = Quaternion.identity;
	private Quaternion calibration = Quaternion.identity;
	private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix = Quaternion.identity;
	private Quaternion referanceRotation = Quaternion.identity;
	private bool debug = true;

	/// <summary>
	/// Attaches gyro controller to the transform.
	/// </summary>
	private void AttachGyro()
	{
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}
	/// <summary>
	/// Detaches gyro controller from the transform
	/// </summary>
	private void DetachGyro()
	{
		gyroEnabled = false;
	}

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}
	/// <summary>
	/// Update the camera base rotation.
	/// </summary>
	/// <param name='onlyHorizontal'>
	/// Only y rotation.
	/// </param>
	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}
	/// <summary>
	/// Gets the rot fix for different orientations.
	/// </summary>
	/// <returns>
	/// The rot fix.
	/// </returns>
	private Quaternion GetRotFix()
	{
		return Quaternion.identity;
	}
	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}
	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
	}
}
