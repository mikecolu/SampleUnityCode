using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUIAnimation : MonoBehaviour {

	protected bool bIsShown = false;
	public bool BIsShown { 
		get { 
			return bIsShown;
		}
	}

	protected virtual void Start() {
		// Hide ();
	}

	#region Animation
	public void Show()
	{
		AnimateIn();
	}

	public void Hide()
	{
		AnimateOut();
	}

	public void Hide(SceneType p_nextState)
	{
		CommandToNextState command = gameObject.AddComponent<CommandToNextState> ();
		command.Initialize (p_nextState);
		AnimateOut(command);
	}

	protected abstract void AnimateIn ();

	protected abstract void AnimateOut (ACommand command = null);
	#endregion
}
