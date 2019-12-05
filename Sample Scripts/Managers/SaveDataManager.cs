#define DEBUG_ENABLE_SAVEDATA_LOGS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoInstance<SaveDataManager> {

	private SaveDataXml m_saveData = null;

	#if DEBUG_ENABLE_SAVEDATA_LOGS
	private string m_strLog = string.Empty;

	private void OnGUI()
	{
		GUILayout.TextField( m_strLog, GUILayout.Width( 1000 ) );
	}
	#endif

	protected override void Awake()
	{
		base.Awake();
	}

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	private void Load()
	{
		m_saveData = SaveDataXml.Load();

		if( m_saveData == null ) {
			Debug.Log( "SaveDataManager: No data to load" );
			m_saveData = new SaveDataXml();
		}

		#if DEBUG_ENABLE_SAVEDATA_LOGS
		m_strLog += "\n" + m_saveData.ToString();
		#endif
	}

	private void Unload()
	{
		m_saveData = null;
	}

	private void Save()
	{
		if( m_saveData == null ) {
			Debug.LogError( "SaveDataManager: null Save" );
			return;
		}

		m_saveData.Save();
	}
}
