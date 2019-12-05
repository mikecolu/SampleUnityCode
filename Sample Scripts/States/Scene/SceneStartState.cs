using UnityEngine;
using System.Collections;

public class SceneStartState : StateMachineBehaviour
{
	private float m_introDelay = 2f;

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Start Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.Start);
		}
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Start Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.Start);
		}

		m_introDelay = 2.0f;
	}
}
