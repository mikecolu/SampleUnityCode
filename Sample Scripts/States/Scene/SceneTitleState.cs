using UnityEngine;
using System.Collections;

public class SceneTitleState : StateMachineBehaviour
{
	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Title Enter" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].InitializeManager (SceneType.Title);
		}

		SceneController.Instance.RevealScene();
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Scene: Title Exit" );
		IManager[] managers = GameStateManager.Instance.managers.ToArray();
		for (int i = 0; i < managers.Length; i++) {
			managers [i].CleanupManager (SceneType.Title);
		}
	}
}
