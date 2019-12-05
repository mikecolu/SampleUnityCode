using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTutorialState : StateMachineBehaviour {

	private float m_gameStartDelay = 1.0f;

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Tutorial Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.Tutorial);
		}
	}

	public override void OnStateUpdate( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
        if(m_gameStartDelay > 0.1f)
		{
			m_gameStartDelay -= Time.deltaTime;
		}
		else if( m_gameStartDelay != 0.0f )
		{
			SceneController.Instance.RevealScene();
			m_gameStartDelay = 0.0f;
		}
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Tutorial Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.Tutorial);
		}

		m_gameStartDelay = 2.0f;
	}
}
