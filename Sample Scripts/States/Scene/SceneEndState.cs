using UnityEngine;
using System.Collections;

public class SceneEndState : StateMachineBehaviour
{
	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: End Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.End);
		}
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: End Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.End);
		}
	}
}
