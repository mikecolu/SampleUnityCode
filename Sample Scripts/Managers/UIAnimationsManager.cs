using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationsManager : MonoInstance<UIAnimationsManager> {

	[SerializeField] private UIAnimItem[] m_uiItems;

	public Dictionary<SceneType, AUIAnimation[]> m_uiAnimItemsDictionary {private set; get;}

	[System.Serializable]
	public class UIAnimItem
	{
		public SceneType sceneType;
		public AUIAnimation[] uiAnim;
	}

	public override void InitializeManager (SceneType s) {
		
		AUIAnimation[] uiToAnimate = GetUIAnimation(s);
		if (uiToAnimate == null) {
			return;
		}

		for (int i = 0; i < uiToAnimate.Length; i++) {
			uiToAnimate [i].Show ();
		}
	}

	public override void CleanupManager (SceneType s) {}

	public void AnimateThenGoToState(SceneType p_nextState) {
		SceneType currentState = GameStateManager.Instance.GetCurrentState ();
		AUIAnimation[] uiToAnimate = GetUIAnimation(currentState);
		if (uiToAnimate == null) {
			return;
		}

		for (int i = 0; i < uiToAnimate.Length; i++) {
			uiToAnimate [i].Hide (p_nextState);
		}
	}

	protected override void Awake()
	{
		base.Awake();

		m_uiAnimItemsDictionary = new Dictionary<SceneType, AUIAnimation[]>();
		foreach(UIAnimItem item in m_uiItems)
		{
			m_uiAnimItemsDictionary.Add(item.sceneType, item.uiAnim);
		}
	}

	public AUIAnimation[] GetUIAnimation(SceneType p_sceneType)
	{
		if(m_uiAnimItemsDictionary.ContainsKey(p_sceneType))
		{
			return m_uiAnimItemsDictionary[p_sceneType];
		}

		return null;
	}
}
