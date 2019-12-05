using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("database")]
public class SaveDataXml
{
	private static readonly string SAVEDATA_FILENAME = "user";

	#region Xml Format
	public int score;
	#endregion

	public SaveDataXml(){
	}

	public static SaveDataXml Load()
	{
		return (SaveDataXml)XmlHelper.LoadFromDataPath<SaveDataXml>( SAVEDATA_FILENAME );
	}

	public void Save()
	{
		XmlHelper.SaveToDataPath<SaveDataXml>( SAVEDATA_FILENAME, this );
	}

	public override string ToString()
	{
		string strLog = string.Empty;
		strLog += "score: " + score;
		return strLog;
	}
}
