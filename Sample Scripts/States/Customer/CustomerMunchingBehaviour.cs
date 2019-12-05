using UnityEngine;
using System.Collections;

public class CustomerMunchingBehaviour : StateMachineBehaviour
{

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Munching: End Enter" );
		if(p_animator.gameObject.GetComponent<Customer>() != null)
		{
			if (p_animator.gameObject.GetComponent<Customer>().aOnIsEating != null) {
				p_animator.gameObject.GetComponent<Customer>().aOnIsEating (true);
			}
		}
		
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Munching: End Exit" );
		
	}
}
