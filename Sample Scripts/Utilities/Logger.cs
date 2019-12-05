using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoInstance<Logger> {

	[SerializeField] private Text m_textLog;
	string m_strLog;

	public override void InitializeManager (SceneType s) {
	}

	public override void CleanupManager (SceneType s) {
	}

	void Update()
	{
		m_textLog.text = m_strLog;
	}

	public void Log(string p_strLog)
	{
		m_strLog += "\n" + p_strLog;

		// limit string length
		if(m_strLog.Length >= 5000)
		{
			int len = m_strLog.Length / 2;
			int idx = m_strLog.Length - len;
			m_strLog = m_strLog.Substring(idx, len);
		}
	}
}
