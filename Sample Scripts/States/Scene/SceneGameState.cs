using UnityEngine;
using System.Collections;

public class SceneGameState : StateMachineBehaviour
{
	private float m_gameStartDelay = 1.0f;

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Game Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.Game);
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
		Debug.Log( "Scene: Game Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.Game);
		}

		m_gameStartDelay = 2.0f;
	}
}
