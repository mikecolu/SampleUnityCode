using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandToNextState : ACommand {

	private SceneType m_nextState = SceneType.Title;
	private bool m_bInitialized = false;

	public override void Execute () {
		if (!m_bInitialized) {
			D.error ("Error: Command to next state not initialized! Ignoring execution...");
			return;
		}

//		GameStateManager.Instance.SwitchToState (m_nextState);
		SceneController.Instance.HideAndLoadScene (m_nextState);
	}

	public void Initialize(SceneType p_nextState) {
		m_nextState = p_nextState;
		m_bInitialized = true;
	}
}
