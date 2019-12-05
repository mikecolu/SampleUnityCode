using UnityEngine;
using UnityEngine.UI;

public class ConsoleToGUI : MonoBehaviour
{
	[SerializeField]
	private Text text;

	static string myLog = "";
	private string output;
	private string stack;

	void OnEnable()
	{
		Application.logMessageReceived += Log;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= Log;
	}

	public void Log(string logString, string stackTrace, LogType type)
	{
		output = logString;
		stack = stackTrace;
		myLog = output + "\n" + myLog;
		if (myLog.Length > 2000)
		{
			myLog = myLog.Substring(0, 2000);
		}
		text.text = myLog;
	}
}