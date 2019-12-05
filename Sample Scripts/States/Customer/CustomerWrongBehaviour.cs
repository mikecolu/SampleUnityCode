using UnityEngine;
using System.Collections;

public class CustomerWrongBehaviour : StateMachineBehaviour
{

	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Wrong: End Enter" );
		if(p_animator.gameObject.GetComponent<Customer>() != null)
		{

			p_animator.gameObject.GetComponent<Customer>().m_customerStatus = CustomerFace.Wrong;
			p_animator.gameObject.GetComponent<Customer>().ChangeFacialExpression(3);
		}
		
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Wrong: End Exit" );

		if (p_animator.gameObject.GetComponent<Customer>().aOnWillWalk != null) {
				p_animator.gameObject.GetComponent<Customer>().aOnWillWalk (true);
		}

		p_animator.gameObject.GetComponent<Customer>().StopWaitTime ();

		
	}
}
