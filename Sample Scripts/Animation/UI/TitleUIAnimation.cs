using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIAnimation : AUIAnimation {

	private float mYValue = 0f;
	private float mYMax, mYMin, mXMax, mXMin, mPosMin, mPosMax, mYMax2, mYMin2;
	private Transform mTransform;

	private ACommand commandToRun = null;

	private void Awake() {
		mTransform = transform;
		mYMax = mTransform.localRotation.y;
		mYMin = mTransform.localRotation.y - 180;
		mYMax2 = mTransform.localRotation.y - 60;
		// mYMin = mTransform.localRotation.y + 180;

		mXMax = mTransform.localRotation.x + 60;
		mXMin = mTransform.localRotation.x;
		mPosMin =  mTransform.localPosition.z;
		mPosMax =  mTransform.localPosition.z - 0.35f;
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
	}

	protected override void AnimateOut(ACommand command = null)
	{
		commandToRun = command;

		iTween.Stop(gameObject );

		iTween.ValueTo(gameObject, iTween.Hash (
			"from", mYValue,
			"to", 1,
			"time", 0.25f,
			"easetype", iTween.EaseType.linear,
			"onupdatetarget", gameObject,
			"onupdate", "OnLiftAnimateUpdate",
			"oncompletetarget", gameObject,
			"oncomplete", "OnFlipAnimation",
			"ignoretimescale", true
		));
	}

	private void OnFlipAnimation()//ACommand command = null)
	{
		// commandToRun = command;

		// iTween.Stop(gameObject );

		iTween.ValueTo(gameObject, iTween.Hash (
			"from", mYValue,
			"to", 0,
			"time", 0.75f,
			"easetype", iTween.EaseType.linear,
			"onupdatetarget", gameObject,
			"onupdate", "OnAnimateUpdate",
			"oncompletetarget", gameObject,
			"oncomplete", "OnDropAnimateOut",
			"ignoretimescale", true
		));
	}

	private void OnDropAnimateOut()
	{
		iTween.ValueTo(gameObject, iTween.Hash (
			"from", mYValue,
			"to", 1,
			"time", 0.25f,
			"easetype", iTween.EaseType.linear,
			"onupdatetarget", gameObject,
			"onupdate", "OnDropAnimateUpdate",
			"oncompletetarget", gameObject,
			"oncomplete", "OnAnimateOutComplete",
			"ignoretimescale", true
		));
	}

	

	private void OnAnimateInComplete()
	{
		bIsShown = true;
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

	private void OnLiftAnimateUpdate( float pValue )
	{
		mYValue = pValue;

		// mTransform.localRotation = Quaternion.Euler(Mathf.Lerp(mXMin, mXMax, mYValue), mTransform.localRotation.y, mTransform.localRotation.z);
		mTransform.localPosition = new Vector3(mTransform.localPosition.x, mTransform.localPosition.y, Mathf.Lerp(mPosMin, mPosMax, mYValue));
	}

	private void OnAnimateUpdate( float pValue )
	{
		mYValue = pValue;

		mTransform.localRotation = Quaternion.Euler(mTransform.localRotation.x, Mathf.Lerp(mYMin, 22.5f, mYValue), mTransform.localRotation.z);//Mathf.Lerp(mXMax, -60, mYValue)
		// new Vector3 (
		// 	mTransform.localRotation.x,
		// 	Mathf.Lerp(mYMin, mYMax, mYValue),
		// 	mTransform.localRotation.z
		// );
	}

	private void OnDropAnimateUpdate( float pValue )
	{
		mYValue = pValue;

		// mTransform.localRotation = Quaternion.Euler(Mathf.Lerp(-mXMax, mXMin, mYValue), mYMin, mTransform.localRotation.z);
		mTransform.localPosition = new Vector3(mTransform.localPosition.x, mTransform.localPosition.y, Mathf.Lerp(mPosMax, mPosMin, mYValue));
	}

	#endregion
}
