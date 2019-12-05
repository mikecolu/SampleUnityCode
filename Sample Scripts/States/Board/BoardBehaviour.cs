using UnityEngine;
using System.Collections;

public class BoardBehaviour : StateMachineBehaviour
{

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		// Debug.Log( "Board: End Exit" );

		// p_animator.gameObject.transform.parent.gameObject.SetActive(false);
		p_animator.ResetTrigger("TriggerIn");
		UIManager.Instance.HideUI();
		UIManager.Instance.HideNotifBoard();
		// GameplayManager.Instance.ShowNewNotif();
	}
}
