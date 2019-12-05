//#define DEBUG_ENABLE_LEVELDATA_LOGS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoInstance<LevelDataManager> {

	#if DEBUG_ENABLE_LEVELDATA_LOGS
	private string m_strLog = null;

	private void OnGUI()
	{
		GUILayout.TextField( m_strLog );
	}
	#endif

	protected override void Awake()
	{
//		base.Awake();
//
//		LoadLevelDataFromXml( "levelTest_data" );
//
//		#if DEBUG_ENABLE_LEVELDATA_LOGS
//		m_strLog = m_levelData.ToString();
//		#endif
//
//		Debug.Log( m_levelData );
//
//		StartSpawn();
	}

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	public LevelDataXml LoadLevelDataFromXml( string p_strPath )
	{
		LevelDataXml toReturn;
		if( string.IsNullOrEmpty( p_strPath ) ) {
			Debug.LogError( "Cannot load Xml from path: " + p_strPath );
			return null;
		}

		toReturn = LevelDataXml.Load( p_strPath );

		if( toReturn == null ) {
			Debug.LogError( "Cannot load level Data from: " + p_strPath );
			return null;
		}
		return toReturn;
	}
}
