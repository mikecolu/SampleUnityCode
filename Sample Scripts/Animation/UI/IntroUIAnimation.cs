using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIAnimation : AUIAnimation {

	private float mYValue = 0f;
	private float  mPosZMin, mPosZMax, mPosYMin, mPosYMax;
	private Transform mTransform;

	private ACommand commandToRun = null;

	private void Awake() {
		mTransform = transform;
		mPosYMin =  mTransform.localPosition.y;
		mPosYMax =  mTransform.localPosition.y + 1.5f;
		mPosZMin =  mTransform.localPosition.z;
		mPosZMax =  mTransform.localPosition.z - 4f;
		// mPosXMin =  mTransform.localPosition.x;
		// mPosXMax =  mTransform.localPosition.x - 0.1f;
	}

	#region Animation
	protected override void AnimateIn()
	{
		iTween.Stop (gameObject);

		// iTween.ValueTo(gameObject, iTween.Hash (
		// 	"from", mYValue,
		// 	"to", 1,
		// 	"time", 1.5f,
		// 	"easetype", iTween.EaseType.linear,
		// 	"onupdatetarget", gameObject,
		// 	"onupdate", "OnAnimateUpdate",
		// 	"oncompletetarget", gameObject,
		// 	"oncomplete", "OnAnimateInComplete",
		// 	"ignoretimescale", true
		// ));

		Debug.Log("animating intro");

		iTween.ValueTo(gameObject, iTween.Hash (
			"from", mYValue,
			"to", 1,
			"time", 4f,
			"easetype", iTween.EaseType.linear,
			"onupdatetarget", gameObject,
			"onupdate", "OnAnimateUpdate",
			"oncompletetarget", gameObject,
			"oncomplete", "OnAnimateInComplete",
			"ignoretimescale", true
		));
	}

	protected override void AnimateOut(ACommand command = null)
	{
		commandToRun = command;

		iTween.Stop(gameObject );

		// iTween.ValueTo(gameObject, iTween.Hash (
		// 	"from", mYValue,
		// 	"to", 1,
		// 	"time", 1.5f,
		// 	"easetype", iTween.EaseType.easeInExpo,
		// 	"onupdatetarget", gameObject,
		// 	"onupdate", "OnAnimateUpdate",
		// 	"oncompletetarget", gameObject,
		// 	"oncomplete", "OnAnimateOutComplete",
		// 	"ignoretimescale", true
		// ));
	}
	

	private void OnAnimateInComplete()
	{
		bIsShown = true;
		GameStateManager.Instance.SwitchToTitle ();
	}

	private void OnAnimateOutComplete()
	{
		bIsShown = false;

		if (commandToRun != null) {
			commandToRun.Execute ();
			Destroy (commandToRun);
			commandToRun = null;
		}
	}

	private void OnAnimateUpdate( float pValue )
	{
		mYValue = pValue;

		mTransform.localPosition = new Vector3(mTransform.localPosition.x, Mathf.Lerp(mPosYMin, mPosYMax, mYValue), Mathf.Lerp(mPosZMin, mPosZMax, mYValue));
	}

	#endregion
}