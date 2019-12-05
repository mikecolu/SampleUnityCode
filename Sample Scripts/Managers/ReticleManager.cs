using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TargetDist{
	public RaycastHit hit;
	public float dist;
}

public class ReticleManager : MonoInstance<ReticleManager>
{
	#region Variables and Properties
	[SerializeField] private Reticle m_reticle = null;
	[SerializeField] [Range( 0, 1 )] private float m_fAccuracy = 0.5f;
	[SerializeField] private Transform m_playerHead;
	[SerializeField] private bool m_bEnableDebug = false;

	private int m_iLayerMask = 0;

	private bool m_bHasTarget = false;
	private Vector3 m_vTargetCoord = Vector3.zero;
	private Vector3 m_vAssistedTargetCoord = Vector3.zero;

	public bool HasTarget{ get{ return m_bHasTarget; } }
	public Vector3 TargetCoord{ get{ return m_vTargetCoord; } }
	public Vector3 AssistedTargetCoord{ get{ return m_vAssistedTargetCoord; } }

	private Transform m_reticleTransform;
	#endregion

	public static System.Action<GameObject> aAcquireNewTarget;
	public static System.Action<bool> aHighlightTarget;
	public static System.Action<bool> aTargetCustomer;

	protected override void Awake()
	{
		base.Awake();

		m_iLayerMask = 1;
		m_iLayerMask |= ( 1 << 8 );
		m_iLayerMask |= ( 1 << 9 );
		m_iLayerMask |= ( 1 << 10 );

		Debug.Assert( m_reticle != null, "Reticle object is null" );		
	}

	private void Start() {
		if (m_reticle != null) {
			m_reticleTransform = m_reticle.transform;
		}
	}

	private void Update()
	{
		// Reset values.
		m_bHasTarget = false;

		if(Pvr_UnitySDKManager.SDK.HeadPose == null)
		{
			return;
		}

		Vector3 vPlayerPos = m_playerHead.position;
		Vector3 vPlayerSightPoint = vPlayerPos;
		Vector3 vPlayerSightDir = Pvr_UnitySDKManager.SDK.HeadPose.Orientation * Vector3.forward;
		float fTargetDist = Constants.RETICLE_TARGET_DIST_DEFAULT;

		// Player sight(eye) straight.
		RaycastHit[] initialHits = Physics.RaycastAll( vPlayerSightPoint, vPlayerSightDir, Constants.RETICLE_CAST_DIST, m_iLayerMask );

		#if UNITY_EDITOR
		if( m_bEnableDebug ) { Debug.DrawRay( vPlayerSightPoint, vPlayerSightDir * Constants.RETICLE_CAST_DIST, Color.red ); }
		#endif

		List<TargetDist> targets = new List<TargetDist>(initialHits.Length);

		for( int i = 0; i < initialHits.Length; i++ ) {

			// Debug.Log ("hit: " + initialHits[i].transform.name);

			TargetDist td = new TargetDist();
			td.hit = initialHits[i];
			td.dist = Vector3.Distance( td.hit.transform.position, vPlayerPos );

			bool bAdded = false;
				
			for( int j = 0; j < targets.Count; j++ ) {
				if( td.dist < targets[j].dist ) {
					targets.Insert( j, td );
					bAdded = true;
					break;
				}
			}

			if( !bAdded ) {
				targets.Add( td );
			}
		}

		for( int i = 0; i < targets.Count; i++ )
		{
			RaycastHit hit = targets[i].hit;

			if( hit.distance < fTargetDist ) {
				fTargetDist = hit.distance;
			}

			// If the object is targettable.
			if ( hit.transform.CompareTag( Constants.TAG_RETICLETARGET ) || hit.transform.CompareTag( Constants.TAG_CUSTOMER ) || hit.transform.CompareTag( Constants.TAG_CONE ))
			{
				
				Vector3 vTargetHitDir = ( hit.point - vPlayerSightPoint ).normalized;
				fTargetDist = Vector3.Distance( vPlayerSightPoint, hit.transform.position );

				// Sight to target cast. Check walls.
				Ray ray = new Ray( vPlayerSightPoint, ( hit.transform.position - vPlayerSightPoint ) );
				RaycastHit[] blockHits = Physics.RaycastAll( ray, fTargetDist, m_iLayerMask );

				bool bHasWall = false;

				for( int j = 0; j < blockHits.Length; j++ ) {
					if( blockHits[j].transform.CompareTag( Constants.TAG_WALL ) ) {

						if( blockHits[j].distance < fTargetDist ) {
							fTargetDist = blockHits[j].distance;
						}

						bHasWall = true;
						break;
					}
				}

				if( !bHasWall ) {
					Vector3 vTargetBodyDir = (hit.transform.position - vPlayerSightPoint ).normalized;
					if (aAcquireNewTarget != null) {
						aAcquireNewTarget(hit.transform.gameObject);

						if (aHighlightTarget != null) {
							aHighlightTarget (true);
						}
						if (aTargetCustomer != null) {
							aTargetCustomer (true);
						}
						
					}

					#if UNITY_EDITOR
					if( m_bEnableDebug ) { 
						Debug.DrawRay( vPlayerSightPoint, vTargetBodyDir * fTargetDist, Color.green );
						Debug.DrawRay( vPlayerSightPoint, vTargetHitDir * fTargetDist, Color.yellow );
					}
					#endif

					// Distance of center to hit point.
					float dist = Vector3.Distance( vTargetBodyDir, vTargetHitDir ) * (fTargetDist);
					// In percentage in relation to target radius.
					dist = dist / Constants.RETICLE_TARGET_RADIUS;

					// Eased.
					float t = EaseSineIn( dist );
					// Accuracy adjustment.
					t = Mathf.Lerp( 1, t, m_fAccuracy );

					Vector3 vTargetFinal = vTargetHitDir;

					m_vAssistedTargetCoord = vPlayerSightPoint + vTargetFinal * (fTargetDist);
					m_reticle.transform.position = m_vAssistedTargetCoord;
					m_bHasTarget = true;
					break;
				}
			}
		}

		m_vTargetCoord = vPlayerSightPoint + vPlayerSightDir * fTargetDist;

		if( !m_bHasTarget ) {
			m_vAssistedTargetCoord = m_vTargetCoord;

			m_reticle.transform.position = m_vTargetCoord;

			if (aAcquireNewTarget != null) {
				aAcquireNewTarget(null);
			}
			if (aHighlightTarget != null) {
					aHighlightTarget (false);
			}
			if (aTargetCustomer != null) {
					aTargetCustomer (false);
			}
		}



		if( m_reticle != null ) {
			m_reticle.SetHasTarget( m_bHasTarget );

			// Resize reticle depending on distance to player

			Vector3 retPos = m_reticleTransform.position;
			float newScale = Constants.RETICLE_SIZE * Vector3.Distance (vPlayerSightPoint, retPos);
			m_reticleTransform.localScale = Vector3.one * newScale;
		}
	}

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	#region Easing
	private float EaseSineIn( float p_value ) {
		return -1 * Mathf.Cos(p_value * (Mathf.PI/2)) + 1;
	}

	private float EaseQuadIn( float p_value ) {
		return 1 * p_value * p_value;
	}
	#endregion
}
