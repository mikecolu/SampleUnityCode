using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTossTween : BezierTween {

	// Preferably 0-0.5f
	private const float CTRL_LINEAR_PERCENTAGE = 0.3f;
	private const float CTRL_HEIGHT_PERCENTAGE = 0.075f;
	private const float CTRL_HEIGHT_NOISE = 0.05f;
	private const float SECONDSPERMAGNITUDE = 0.075f;
	private const float SECONDSPERMAGNITUDE_NOISE = 0.00f;

	Rigidbody m_rigidbody = null;

	#region Start/Stop Animation
	public void Run( Transform p_tTweenObj, Vector3 p_startPosition, Vector3 p_endPosition, Action p_callback )
	{
		float distance = Vector3.Distance( p_startPosition, p_endPosition );
		float fHeightNoise = UnityEngine.Random.Range( -CTRL_HEIGHT_NOISE, CTRL_HEIGHT_NOISE );
		float height = distance * ( CTRL_HEIGHT_PERCENTAGE + fHeightNoise );
		Vector3 vDir = p_endPosition - p_startPosition;

		Vector3 ctrl1 = vDir * CTRL_LINEAR_PERCENTAGE + p_startPosition;
		Vector3 ctrl2 = vDir * ( 1f - CTRL_LINEAR_PERCENTAGE ) + p_startPosition;

		List<Vector3> controlPoints = new List<Vector3>( new Vector3[]{
			p_startPosition,
			ctrl1 + Vector3.up * height,
			ctrl2 + Vector3.up * height,
			p_endPosition
		} );

		Stop();

		m_rigidbody = p_tTweenObj.GetComponent<Rigidbody>();

		float fVelNoise = UnityEngine.Random.Range( -SECONDSPERMAGNITUDE_NOISE, SECONDSPERMAGNITUDE_NOISE );

		StartAnimation( p_tTweenObj, controlPoints, ( SECONDSPERMAGNITUDE + fVelNoise ) * distance, p_callback );
	}
	#endregion

	#region Tweening
	protected override void MoveObject( Vector3 p_position )
	{
		if( m_rigidbody != null ) {
			m_rigidbody.MovePosition( p_position );
		}		
	}
	#endregion

	#region Reset
	protected override void ResetValues()
	{
		base.ResetValues();

		m_rigidbody = null;
	}
	#endregion
}
