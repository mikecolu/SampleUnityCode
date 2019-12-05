using System.Collections.Generic;
using System.Xml.Serialization;  
using System.IO;
using UnityEngine;
using System.Xml;
using System.Text;

public class XmlHelper
{
	#if UNITY_EDITOR
	private static readonly string SAVE_DATA_PATH = Application.dataPath + "/SaveData";
	#else
	private static readonly string SAVE_DATA_PATH = Application.persistentDataPath + "/SaveData";
	#endif


	public static object LoadFromResources( string p_filename, System.Type p_type )
	{
		XmlSerializer serializer = new XmlSerializer( p_type );
		TextAsset textAsset = (TextAsset)Resources.Load( "Xml/" + p_filename, typeof(TextAsset) );

		if( textAsset == null ) {
			Debug.LogError( "XmlHelper: " + p_filename + " is not found." );
			return null;
		}

		MemoryStream assetStream = new MemoryStream( textAsset.bytes );
		object container = serializer.Deserialize( assetStream );
		assetStream.Close();
		return container;
	}

	public static object LoadFromDataPath<T>( string p_filename ) where T : class
	{
		if( !Directory.Exists( SAVE_DATA_PATH ) ) {
			return null;
		}

		return EncryptedXmlSerializer.Load<T>( GetFormattedFilePath( p_filename ) );
	}

	public static void SaveToResources( string p_filename, System.Type p_type, object p_instance )
	{
		string filename = Application.dataPath + "/Resources/Xml/" + p_filename + ".xml";

		{
			XmlSerializer serializer = new XmlSerializer( p_type );
			Encoding encoding = Encoding.GetEncoding( "UTF-8" );
			using( StreamWriter stream = new StreamWriter( filename, false, encoding ) ) {
				serializer.Serialize ( stream, p_instance );
				stream.Close();
			}
		}

	}

	public static void SaveToDataPath<T>( string p_filename, object p_instance ) where T : class
	{
		if( !Directory.Exists( SAVE_DATA_PATH ) ) {
			Directory.CreateDirectory( SAVE_DATA_PATH );
		}

		EncryptedXmlSerializer.Save<T>( GetFormattedFilePath( p_filename ), p_instance );

	}

	private static string GetFormattedFilePath( string p_filename )
	{
		return SAVE_DATA_PATH + "/" + p_filename + ".xml";
	}
} 
