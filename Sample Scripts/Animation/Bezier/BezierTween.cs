using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTween : MonoBehaviour
{
	[SerializeField] private bool m_bDebug = false;

	private BezierPath m_bezier = null;
	private Transform m_tTweenObj = null;
	private Coroutine m_coroutine = null;
	private Action m_callback = null;
	protected bool m_bIsTweening = false;

	#region Start/Stop Animation
	public void Run( Transform p_tTweenObj, List<Vector3> p_controlPoints, float p_duration, Action p_callback )
	{
		Stop();

		StartAnimation( p_tTweenObj, p_controlPoints, p_duration, p_callback );
	}

	protected void StartAnimation( Transform p_tTweenObj, List<Vector3> p_controlPoints, float p_duration, Action p_callback )
	{
		m_bezier = new BezierPath();
		m_bezier.SetControlPoints( p_controlPoints );

		m_tTweenObj = p_tTweenObj;

		m_callback = p_callback;

		m_coroutine = StartCoroutine( TweenCoroutine( p_duration ) );		

		m_bIsTweening = true;
	}

	public void Stop(){
		if( m_coroutine != null ) {
			StopCoroutine( m_coroutine );			
		}

		ResetValues();
	}
	#endregion

	#region Tweening
	private IEnumerator TweenCoroutine( float p_duration )
	{
		float fStartTime = Time.time;
		float t = 0;
		while( t <= 1 )
		{
			t = ( Time.time - fStartTime ) / p_duration;

			Vector3 point = m_bezier.CalculateBezierPoint( 0, t );

			MoveObject( point );

			yield return null;
		}

		if( m_callback != null ) {
			m_callback();
		}

		ResetValues();
	}

	protected virtual void MoveObject( Vector3 p_position )
	{
		if( m_tTweenObj != null ) {
			m_tTweenObj.position = p_position;
		}		
	}
	#endregion

	#region Reset
	protected virtual void ResetValues()
	{
		m_bezier = null;
		m_tTweenObj = null;
		m_coroutine = null;
		m_callback = null;
		m_bIsTweening = false;
	}
	#endregion

	#region Gizmos
	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if( !m_bDebug ) {
			return;
		}

		if( !m_bIsTweening ) {
		}

		if( m_bezier == null ) {
			return;
		}
			
		for( int i = 0; i < 4; i++ ) {
			Gizmos.DrawWireSphere( m_bezier.GetControlPoints()[i], 0.25f );
		}

		List<Vector3> m_drawingPoints = m_bezier.GetDrawingPoints2();

		Gizmos.DrawLine( m_bezier.GetControlPoints()[0], m_bezier.GetControlPoints()[1] );

		Gizmos.DrawLine( m_bezier.GetControlPoints()[2], m_bezier.GetControlPoints()[3] );

		for( int i = 0; i < m_drawingPoints.Count - 1; i++ ) {
			Gizmos.DrawLine( m_drawingPoints[i], m_drawingPoints[i + 1] );
		}

		if( m_tTweenObj != null ) {
			Gizmos.DrawLine( m_tTweenObj.position, m_drawingPoints[0] );
		}
	}
	#endif
	#endregion
}
