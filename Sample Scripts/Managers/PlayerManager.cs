using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoInstance<PlayerManager> {

	[SerializeField]
	private GameObject m_playerOrigin;
	private Vector3 m_playerPosition;
	private PlayerStance m_currentStance;

	public Vector3 GetPlayerCurrPosition()
	{
		return m_playerOrigin.transform.position;
	}

	public Vector3 GetPlayerOriginPosition()
	{
		return m_playerPosition;
	}

	void Start()
	{
		m_playerPosition = new Vector3(m_playerOrigin.transform.position.x, m_playerOrigin.transform.position.y, m_playerOrigin.transform.position.z);
	}
	
	public override void InitializeManager (SceneType s) {

		if (s == SceneType.Title) {
			
		}

		if (s == SceneType.Start) {
			
		}

		Pvr_UnitySDKAPI.System.UPvr_GetDeviceMode();

		#if ANDROID_DEVICE
			Debug.Log("Device Mode: " + Pvr_UnitySDKAPI.System.UPvr_GetDeviceMode());
		#endif
		
	}
	public override void CleanupManager (SceneType s) {}

	public void ChangePlayerPosition(SceneType p_scene)
	{
		if(p_scene == SceneType.Title)
		{
			m_playerOrigin.transform.position = new Vector3(m_playerPosition.x, m_playerPosition.y,m_playerPosition.z + 3.1f);
		}
		else{
			m_playerOrigin.transform.position = m_playerPosition;
		}
	}

	public void SetPlayerStance(PlayerStance p_stance)
	{
		m_currentStance = p_stance;
	}

	public void GetPlayerStance()
	{

	}
}
