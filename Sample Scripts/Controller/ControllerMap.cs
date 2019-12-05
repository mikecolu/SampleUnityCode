#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerScheme {
	CONTROLLER,
	REMOTE
}

namespace DairyScoop
{

	public class ControllerMap : MonoBehaviour {

		public static Action aUpButtonPressed;
		public static Action aDownButtonPressed;
		public static Action aRightButtonPressed;
		public static Action aLeftButtonPressed;

		public static Action aMenuButtonDown;
		public static Action aMenuButtonPressed;
		public static Action aMenuButtonUp;

		public static Action aBackButtonDown;
		public static Action aBackButtonPressed;
		public static Action aBackButtonUp;

		public static Action aAButtonDown;
		public static Action aAButtonPressed;
		public static Action aAButtonUp;

		public static Action aBButtonDown;
		public static Action aBButtonPressed;
		public static Action aBButtonUp;

		public static Action aXButtonDown;
		public static Action aXButtonPressed;
		public static Action aXButtonUp;

		public static Action aYButtonDown;
		public static Action aYButtonPressed;
		public static Action aYButtonUp;

		public static Action aLButtonDown;
		public static Action aLButtonPressed;
		public static Action aLButtonUp;

		public static Action aRButtonDown;
		public static Action aRButtonPressed;
		public static Action aRButtonUp;

		public static Action aRightFrontBlock;
		public static Action aRightSideBlock;
		public static Action aRightCross;
		public static Action aRightHook;
		public static Action aRightUppercut;
		public static Action aLeftFrontBlock;
		public static Action aLeftSideBlock;
		public static Action aLeftJab;
		public static Action aLeftHook;
		public static Action aLeftUppercut;


		public static Action<Vector3, Vector3, Vector3> aAccChanged;
		public static Action<Vector3, Vector3, Vector3> aGyrChanged;

		// Pico VR Head
		[SerializeField]
		private Transform mHead;
		[SerializeField]
		private Transform mFlickTracker;
		[SerializeField]
		private Transform mRemoteTransform;
		[SerializeField]
		private Transform mPlayerRoot;

		private Vector3 currFlickPos, prevFlickPos, flickHeading;
		public static float flickDirection, flickSpeed;

		private float m_currRightPunchAngularVel, m_topRightPunchAngularVel = 0, m_currRightPunchVel, m_topRightPunchVel = 0;
		private float m_currLeftPunchAngularVel, m_topLeftPunchAngularVel = 0, m_currLeftPunchVel, m_topLeftPunchVel = 0;
		private bool m_bDidPlayerPunch = true, m_bCanPlayerPunch = true;

		[SerializeField]
		private Transform m_rightHandTransform;
		[SerializeField]
		private Transform m_leftHandTransform;

		private bool trackingSwipe = false;
		private bool checkSwipe = false;
		bool bcanSwitch = true;
		private IEnumerator delayCoroutine;

		public static Action<Vector3, Vector3, Vector3> aHeadRotChanged;
		// Head rotation
		public static Vector3 currentHeadRot = Vector3.zero, prevHeadRot = Vector3.zero, deltaHeadRot = Vector3.zero;

		// Pico VR controller scheme
		private static ControllerScheme mControllerScheme = ControllerScheme.REMOTE;

		private Vector3 currentAcc = Vector3.zero, prevAcc = Vector3.zero, deltaAcc = Vector3.zero;
		private Vector3 currentGyr = Vector3.zero, prevGyr = Vector3.zero, deltaGyr = Vector3.zero; 
		private const float THRESHOLD = 0.45f;


		// Update is called once per frame
		void Update () {

			if((OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude < Constants.PUNCH_VELOCITY_RESET_THRESHOLD 
			&& OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude < Constants.PUNCH_ANGULAR_VELOCITY_RESET_THRESHOLD)
			&& (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude < Constants.PUNCH_VELOCITY_RESET_THRESHOLD 
			&& OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude < Constants.PUNCH_ANGULAR_VELOCITY_RESET_THRESHOLD))
			{
				// Debug.Log("m_bDidPlayerPunch: " + m_bDidPlayerPunch);
				if(m_bDidPlayerPunch)
				{
					Debug.Log("m_topPunchVel: " + m_topLeftPunchVel);
					Debug.Log("m_topPunchAngularVel: " + m_topLeftPunchAngularVel);
					ResetPunchVariables();
				}
			}
			else if(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude >= Constants.PUNCH_VELOCITY_THRESHOLD 
				|| OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude >= Constants.PUNCH_ANGULAR_VELOCITY_THRESHOLD)
			{
				m_bDidPlayerPunch = true;
				m_bCanPlayerPunch = false;
				Debug.Log("Punching Left");
				GetLeftPunchVelocity();
				GetLeftPunchAngularVelocity();
				
			}
			else if(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude >= Constants.PUNCH_ANGULAR_VELOCITY_THRESHOLD
				|| OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude	>= Constants.PUNCH_ANGULAR_VELOCITY_THRESHOLD)
			{
				m_bDidPlayerPunch = true;
				m_bCanPlayerPunch = false;
				Debug.Log("Punching Right");
				GetRightPunchVelocity();
				GetRightPunchAngularVelocity();
			}

		#region ControllerButtons	

			if (mHead != null) {
				// save previous frame's head rotation reading and calculate delta rotation from previous and current frames
				prevHeadRot = currentHeadRot;
				currentHeadRot = mHead.transform.eulerAngles;
				currentHeadRot = new Vector3 (
					(currentHeadRot.x > 180) ? currentHeadRot.x - 360 : currentHeadRot.x,
					(currentHeadRot.y > 180) ? currentHeadRot.y - 360 : currentHeadRot.y,
					(currentHeadRot.z > 180) ? currentHeadRot.z - 360 : currentHeadRot.z
				);
				deltaHeadRot = currentHeadRot - prevHeadRot;
				if (deltaHeadRot != Vector3.zero) {
					if (aHeadRotChanged != null) {
						aHeadRotChanged (prevHeadRot, currentHeadRot, deltaHeadRot);
					}
				}
			}
			
			#if ANDROID_DEVICE && PicoBuild
			if(IsRemoteConnected())
			{
				m_rightHandTransform.rotation = Pvr_UnitySDKAPI.Controller.UPvr_GetControllerQUA(0);
				m_leftHandTransform.rotation = Pvr_UnitySDKAPI.Controller.UPvr_GetControllerQUA(1);
			}
			#elif OculusBuild
				m_rightHandTransform.rotation = OVRInput.GetLocalControllerRotation (OVRInput.Controller.RTouch);
				m_rightHandTransform.position = OVRInput.GetLocalControllerPosition (OVRInput.Controller.RTouch);
				m_leftHandTransform.rotation = OVRInput.GetLocalControllerRotation (OVRInput.Controller.LTouch);
				m_leftHandTransform.position = OVRInput.GetLocalControllerPosition (OVRInput.Controller.LTouch);

			#endif

			prevAcc = currentAcc;
			currentAcc = Pvr_UnitySDKAPI.Controller.Upvr_GetAcceleration ();

			deltaAcc = currentAcc - prevAcc;

			if (deltaAcc != Vector3.zero) {
				if (aAccChanged != null) {
					aAccChanged (prevAcc, currentAcc, deltaAcc);
				}
			}

		#if PicoBuild

			if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(0) == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft 
			|| Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(0) == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight
			|| Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(1) == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft 
			|| Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(1) == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight)
			{
				if (aXButtonDown != null)
				{
					aXButtonDown ();	
				}
					
			}
		
			/*
		 * Touchpad
		 */
			if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown (0,Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) || Input.GetKeyDown(KeyCode.Z)) {
				if (aAButtonDown != null)
					aAButtonDown ();
			}
			else if(Pvr_UnitySDKAPI.Controller.UPvr_GetKey (0,Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) || Input.GetKey(KeyCode.Z)) {
				if (aAButtonPressed != null)
					aAButtonPressed ();
			}
			else if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp (0,Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) || Input.GetKeyUp(KeyCode.Z)) {
				if (aAButtonUp != null)
					aAButtonUp ();
				Pvr_UnitySDKManager.SDK.newPicovrTriggered = true;
			}

				/*
		 * Home Button
		 */
			if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyLongPressed (0,Pvr_UnitySDKAPI.Pvr_KeyCode.HOME) || Input.GetKeyDown(KeyCode.M)) {
				if (aLButtonPressed != null)
					aLButtonPressed ();
			}

			/*
		 * App Button
		 */
			if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown (0,Pvr_UnitySDKAPI.Pvr_KeyCode.APP)  || Input.GetKeyDown(KeyCode.Space)) {
				if (aBackButtonDown != null)
					aBackButtonDown ();
			}
			else if(Pvr_UnitySDKAPI.Controller.UPvr_GetKey (0,Pvr_UnitySDKAPI.Pvr_KeyCode.APP)) {
				if (aBackButtonPressed != null)
					aBackButtonPressed ();
			}
			else if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp (0,Pvr_UnitySDKAPI.Pvr_KeyCode.APP)) {
				if (aBackButtonUp != null)
					aBackButtonUp ();
			}

		#elif OculusBuild

			if (bcanSwitch == true) {

				Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
				Vector2 secondaryTouchpad = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

				if (secondaryTouchpad.x < 0 || secondaryTouchpad.x > 0 || primaryTouchpad.x < 0 || primaryTouchpad.x > 0){

					delayCoroutine = DelaySwitch (0.5f);

					if (aXButtonDown != null)
					{
						aXButtonDown ();	
					}

					StopCoroutine (delayCoroutine);
					StartCoroutine (delayCoroutine);
					bcanSwitch = false;	
				}
			}

		#endif

			/*
			 * Controller Mapping
			 * - Mapping is based on Pico Neo gamepad
			 */

			/*
			 * Up/Down Buttons
			 */
			if(Input.GetAxis("Vertical") == 1) {
				if (aUpButtonPressed != null)
					aUpButtonPressed ();
			}
			if(Input.GetAxis("Vertical") == -1) {
				if (aDownButtonPressed != null)
					aDownButtonPressed ();
			}

			/*
			 * Left/Right Buttons
			 */
			if(Input.GetAxis("Horizontal") == 1) {
				if (aRightButtonPressed != null)
					aRightButtonPressed ();
			}
			if(Input.GetAxis("Horizontal") == -1) {
				if (aLeftButtonPressed != null)
					aLeftButtonPressed ();
			}

			/*
			 * Menu Button
			 */
			if(Input.GetKeyDown(KeyCode.Menu)) {
				if (aMenuButtonDown != null)
					aMenuButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Menu)) {
				if (aMenuButtonPressed != null)
					aMenuButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Menu)) {
				if (aMenuButtonUp != null)
					aMenuButtonUp ();
			}

			/*
			 * Back Button
			 */
			if(Input.GetKeyDown(KeyCode.Escape)) {
				if (aBackButtonDown != null)
					aBackButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Escape)) {
				if (aBackButtonPressed != null)
					aBackButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Escape)) {
				if (aBackButtonUp != null)
					aBackButtonUp ();
			}

		#endregion

		#region ControllerPosition

			#if PicoBuild
				Debug.Log("----------------------------------------------------------------------------------");
				Debug.Log("Acceleration: " + Pvr_UnitySDKAPI.Controller.Upvr_GetAcceleration());
				Debug.Log("Angular Velocity Right: " + Pvr_UnitySDKAPI.Controller.Upvr_GetAngularVelocity(0));
				Debug.Log("Angular Velocity Left: " + Pvr_UnitySDKAPI.Controller.Upvr_GetAngularVelocity(1));

			#region RightHandPos

			//Front Block Right
			if((GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Front) == 0
			&& (GetReferencePointDirection(m_rightHandTransform.forward) & AxisDirection.Right) > 0)
			{
				// Debug.Log("Right Front Block");
				if(aRightFrontBlock != null)
				{
					aRightFrontBlock();
				}
			}

			//Hook Block Right
			if((GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(m_rightHandTransform.forward) & AxisDirection.Back) > 0)
			{
				// Debug.Log("Right Side Block");
				if(aRightSideBlock != null)
				{
					aRightSideBlock();
				}
			}

			//Cross Right
			if((GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Front) > 0
			&& (GetReferencePointDirection(m_rightHandTransform.forward) & AxisDirection.Left) > 0)
			{
				// Debug.Log("Cross Right");
				if(aRightCross != null)
				{
					aRightCross();
				}
			}

			//Uppercut Right
			if((GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Front) > 0
			&& (GetReferencePointDirection(m_rightHandTransform.forward) & AxisDirection.Right) > 0)
			{
				// Debug.Log("Uppercut Right");
				if(aRightUppercut != null)
				{
					aRightUppercut();
				}
			}

			//Hook Right
			if((GetReferencePointDirection(-m_rightHandTransform.up) & AxisDirection.Left) > 0
			&& (GetReferencePointDirection(m_rightHandTransform.forward) & (AxisDirection.Up | AxisDirection.Back)) > 0)
			{
				// Debug.Log("Hook Right");
				if(aRightHook != null)
				{
					aRightHook();
				}
			}

			#endregion

			#region LeftHandPos

			//Front Block Left
			if((GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Front) == 0
			&& (GetReferencePointDirection(m_leftHandTransform.forward) & AxisDirection.Left) > 0)
			{
				// Debug.Log("Left Front Block");
				if(aLeftFrontBlock != null)
				{
					aLeftFrontBlock();
				}
			}

			//Hook Block Left
			if((GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(m_leftHandTransform.forward) & AxisDirection.Back) > 0)
			{
				// Debug.Log("Left Side Block");
				if(aLeftSideBlock != null)
				{
					aLeftSideBlock();
				}
			}

			//Jab Left
			if((GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Front) > 0
			&& (GetReferencePointDirection(m_leftHandTransform.forward) & AxisDirection.Right) > 0)
			{
				// Debug.Log("Jab Left");
				if(aLeftJab != null)
				{
					aLeftJab();
				}
			}

			//Uppercut Left
			if((GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Up) > 0
			&& (GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Front) > 0
			&& (GetReferencePointDirection(m_leftHandTransform.forward) & AxisDirection.Left) > 0)
			{
				// Debug.Log("Uppercut Left");
				if(aLeftUppercut != null)
				{
					aLeftUppercut();
				}
			}

			//Hook Left
			if((GetReferencePointDirection(-m_leftHandTransform.up) & AxisDirection.Right) > 0
			&& (GetReferencePointDirection(m_leftHandTransform.forward) & (AxisDirection.Up | AxisDirection.Back)) > 0)
			{
				// Debug.Log("Hook Left");
				if(aLeftHook != null)
				{
					aLeftHook();
				}
			}

			#endregion

		#endregion

			/*
			 * A Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button0)){// || Input.GetKeyDown(KeyCode.Z)){// || Input.GetButtonDown("Fire1")
				if (aAButtonDown != null)
					// aAButtonDown ();
					aRButtonDown();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button0)){// || Input.GetButton("Fire1") || Input.GetKey(KeyCode.Z)) {;
				if (aAButtonPressed != null)
					aAButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button0)){// || Input.GetKeyUp(KeyCode.Z)) {//|| Input.GetButtonUp("Fire1") ) {
				if (aAButtonUp != null)
					aAButtonUp ();
			}

			/*
			 * B Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.X)) {
				if (aBButtonDown != null)
					aBButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.X)) {
				if (aBButtonPressed != null)
					aBButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.X)) {
				if (aBButtonUp != null)
					aBButtonUp ();
			}

			/*
			 * X Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.C)) {
				if (aXButtonDown != null)
					aXButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.C)) {
				if (aXButtonPressed != null)
					aXButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.C)) {
				if (aXButtonUp != null)
					aXButtonUp ();
			}

			/*
			 * Y Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button3)) {
				if (aYButtonDown != null)
					aYButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button3)) {
				if (aYButtonPressed != null)
					aYButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button3)) {
				if (aYButtonUp != null)
					aYButtonUp ();
			}

			/*
			 * L Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button4)) {
				if (aLButtonDown != null)
					aLButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button4)) {
				if (aLButtonPressed != null)
					aLButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button4)) {
				if (aLButtonUp != null)
					aLButtonUp ();
			}

			/*
			 * R Button
			 */
			if(Input.GetKeyDown(KeyCode.Joystick1Button5)) {
				if (aRButtonDown != null)
					aRButtonDown ();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button5)) {
				if (aRButtonPressed != null)
					aRButtonPressed ();
			}
			else if(Input.GetKeyUp(KeyCode.Joystick1Button5)) {
				if (aRButtonUp != null)
					aRButtonUp ();
			}
		}

		private void ComputeFlickDirection()
		{
			prevFlickPos    = currFlickPos;
			currFlickPos    = mFlickTracker.position;
			flickHeading    = currFlickPos - prevFlickPos;
			flickDirection  = Vector3.Dot(flickHeading, mRemoteTransform.up);
			flickDirection += Mathf.Abs(Vector3.Dot(flickHeading, mRemoteTransform.right)) * 0.7f;
			flickSpeed      = flickHeading.sqrMagnitude * flickDirection * flickDirection;
		}

		private void GetRightPunchVelocity()
		{
			// m_bDidPlayerPunch = true;

			m_currRightPunchVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;

			if(m_currRightPunchVel > m_topRightPunchVel)
			{
				m_topRightPunchVel = m_currRightPunchVel;
			}
			
		}

		private void GetRightPunchAngularVelocity()
		{
			// m_bDidPlayerPunch = true;

			m_currRightPunchAngularVel = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude;

			if(m_currRightPunchAngularVel > m_topRightPunchAngularVel)
			{
				m_topRightPunchAngularVel = m_currRightPunchAngularVel;
			}
			
		}

		private void GetLeftPunchVelocity()
		{
			// m_bDidPlayerPunch = true;

			m_currLeftPunchVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude;

			if(m_currLeftPunchVel > m_topLeftPunchVel)
			{
				m_topLeftPunchVel = m_currLeftPunchVel;
			}
			
		}

		private void GetLeftPunchAngularVelocity()
		{
			// m_bDidPlayerPunch = true;

			m_currLeftPunchAngularVel = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude;

			if(m_currLeftPunchAngularVel > m_topLeftPunchAngularVel)
			{
				m_topLeftPunchAngularVel = m_currLeftPunchAngularVel;
			}
			
		}

		private void ResetPunchVariables()
		{
			m_bDidPlayerPunch = false;
			m_bCanPlayerPunch = true;
			m_topRightPunchVel = 0.0f;
			m_topRightPunchAngularVel = 0.0f;
			m_topLeftPunchVel = 0.0f;
			m_topLeftPunchAngularVel = 0.0f;

			Debug.Log("Reset Punch");
		}

		private bool IsRemoteConnected()
		{
			#if ANDROID_DEVICE
			if(ControllerMap.GetControllerScheme () != ControllerScheme.REMOTE){ return false; }
			return 	  Pvr_ControllerManager.controllerlink.IsServiceExisted()
			&& Pvr_ControllerManager.controllerlink.controllerConnected;
			#else
			return false;
			#endif
		}

		public static ControllerScheme GetControllerScheme() {
			return mControllerScheme;
		}

		public AxisDirection GetReferencePointDirection(Vector3 p_referencePoint)
		{
			p_referencePoint = p_referencePoint.normalized;
			float pointerDotRight = Vector3.Dot( p_referencePoint, mPlayerRoot.right   );
			float pointerDotUp    = Vector3.Dot( p_referencePoint, mPlayerRoot.up      );
			float pointerDotFront = Vector3.Dot( p_referencePoint, mPlayerRoot.forward );

			AxisDirection orientation = AxisDirection.None;

			if( pointerDotRight >= THRESHOLD )
			{
				orientation |= AxisDirection.Right;
			}
			else if( pointerDotRight <= -THRESHOLD )
			{
				orientation |= AxisDirection.Left;
			}

			if( pointerDotUp >= THRESHOLD )
			{
				orientation |= AxisDirection.Up;
			}
			else if( pointerDotUp <= -THRESHOLD )
			{
				orientation |= AxisDirection.Down;
			}

			if( pointerDotFront >= THRESHOLD )
			{
				orientation |= AxisDirection.Front;
			}
			else if( pointerDotFront <= -THRESHOLD )
			{
				orientation |= AxisDirection.Back;
			}
			
			return orientation;
		}

		IEnumerator DelaySwitch(float p_delayTime) {

			yield return new WaitForSeconds (p_delayTime);
			bcanSwitch = true;

		}

	}

}
