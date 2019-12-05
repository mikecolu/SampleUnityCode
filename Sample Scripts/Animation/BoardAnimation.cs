using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoardAnimation : MonoBehaviour {

	private Animator m_animator;
	void Awake()
	{
		m_animator = GetComponent<Animator> ();
	}

	void OnEnable()
	{
		GameplayManager.m_aOpenBoard += OpenBoard;
		// ControlsManager.m_aShowBoard += OpenBoard;
	}

	void OnDisable()
	{
		GameplayManager.m_aOpenBoard -= OpenBoard;
		// ControlsManager.m_aShowBoard -= OpenBoard;
	}

	void OpenBoard(bool b_pWillOpen)
	{
		if(b_pWillOpen)
		{
			m_animator.ResetTrigger("TriggerOut");
			m_animator.SetTrigger ("TriggerIn");
			Debug.Log("animate");
		}
		else{
			m_animator.SetTrigger ("TriggerOut");
			// m_animator.ResetTrigger("TriggerOut");
		}
	}
}
