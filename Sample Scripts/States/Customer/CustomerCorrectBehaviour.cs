using UnityEngine;
using System.Collections;

public class CustomerCorrectBehaviour : StateMachineBehaviour
{
	public override void OnStateEnter( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Correct: End Enter" );
		if(p_animator.gameObject.GetComponent<Customer>() != null)
		{

			p_animator.gameObject.GetComponent<Customer>().m_customerStatus = CustomerFace.Correct;
			p_animator.gameObject.GetComponent<Customer>().ChangeFacialExpression(2);
		}
	}

	public override void OnStateExit( Animator p_animator, AnimatorStateInfo p_stateInfo, int p_layerIndex )
	{
		Debug.Log( "Customer Correct: End Exit" );
		if(p_animator.gameObject.GetComponent<Customer>() != null)
		{
			if (p_animator.gameObject.GetComponent<Customer>().aOnWillWalk != null) {
				p_animator.gameObject.GetComponent<Customer>().aOnWillWalk (true);
			}

			p_animator.gameObject.GetComponent<Customer>().StopWaitTime ();
			GameplayManager.Instance.AddRoundSatisfaction();
		}
	}
}
