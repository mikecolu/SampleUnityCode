using UnityEngine;
using System.Collections;

public class SceneIntroState : StateMachineBehaviour
{
	private float m_introDelay = 2f;

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Intro Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.Intro);
		}

		// SceneController.Instance.RevealScene();
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Intro Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.Intro);
		}

		m_introDelay = 2.0f;
	}
}
