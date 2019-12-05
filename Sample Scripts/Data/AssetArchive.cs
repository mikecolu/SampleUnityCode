using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AssetArchive : Singleton<AssetArchive> {

	private AssetArchive(){}

	const string m_orderCardImagePath = "Sprites/OrderCardImages/";
	const string m_orderCardIconPath = "Sprites/OrderCardIcons/";
	const string m_UIPath = "Sprites/UI/";

	public override void InitializeManager (SceneType s){}
	public override void CleanupManager (SceneType s){}

	private Sprite[] m_orderCardImages;
	private Sprite[] m_orderCardIcons;
	private Sprite[] m_UIIcons;

	public Sprite GetSpriteImage(string p_spriteName)
	{
		foreach(Sprite sprite in m_orderCardImages)
		{
			if (sprite.name == p_spriteName) {

				return sprite;
			}
		}

		return null;
	}

	public Sprite GetSpriteIcon(string p_spriteName)
	{
		foreach(Sprite sprite in m_orderCardIcons)
		{
			if (sprite.name == p_spriteName) {

				return sprite;
			}
		}

		return null;
	}

	public Sprite GetUIIcon(string p_spriteName)
	{
		foreach(Sprite sprite in m_UIIcons)
		{
			if (sprite.name == p_spriteName) {

				return sprite;
			}
		}

		return null;
	}

	public void LoadAssets()
	{
		m_orderCardImages = Resources.LoadAll( m_orderCardImagePath, typeof(Sprite) ).Cast<Sprite>().ToArray();
		m_orderCardIcons = Resources.LoadAll( m_orderCardIconPath, typeof(Sprite) ).Cast<Sprite>().ToArray();
		m_UIIcons = Resources.LoadAll( m_UIPath, typeof(Sprite) ).Cast<Sprite>().ToArray();
	}

}
