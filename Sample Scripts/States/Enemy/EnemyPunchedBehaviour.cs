using System.Collections;
using UnityEngine;

public class EnemyPunchedBehaviour : StateMachineBehaviour {

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{

	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		p_animator.gameObject.transform.parent.GetComponent<Enemy>().bIsPunched = false;
	}
}
