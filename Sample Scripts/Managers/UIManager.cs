using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoInstance<UIManager> {

	[SerializeField]
	UIBoards[] m_uiBoards;

	[SerializeField]
	GameObject m_notifBoard;
	[SerializeField]
	Image m_notifBoardImage;
	[SerializeField]
	Text m_notifBoardText;

	[System.Serializable]
	class UIBoards
	{
		public GameObject uiBackground;
		public GameObject uiContent;
		public List<GameObject> satisfactionText;
	}

	// [SerializeField]
	// List<Text> m_satisfactionText; 

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	public void ShowUI(BoardType p_board)
	{

		switch(p_board)
		{
			case BoardType.EndGame: 
			m_uiBoards[0].uiBackground.SetActive(true);
			m_uiBoards[0].uiContent.SetActive(true);
			m_uiBoards[0].satisfactionText[0].SetActive(true);
			break;
			case BoardType.Exit: 
			m_uiBoards[1].uiBackground.SetActive(true);
			m_uiBoards[1].uiContent.SetActive(true);
			break;
			case BoardType.EndDay: 
			m_uiBoards[2].uiBackground.SetActive(true);
			m_uiBoards[2].uiContent.SetActive(true);
			m_uiBoards[2].satisfactionText[0].SetActive(true);
			m_uiBoards[2].satisfactionText[1].SetActive(true);
			break;
			case BoardType.Gameover: 
			m_uiBoards[3].uiBackground.SetActive(true);
			m_uiBoards[3].uiContent.SetActive(true);
			m_uiBoards[3].satisfactionText[0].SetActive(true);
			break;
			case BoardType.Paused: 
			m_uiBoards[4].uiBackground.SetActive(true);
			m_uiBoards[4].uiContent.SetActive(true);
			break;
			case BoardType.Notif:
			m_notifBoard.SetActive(true);
			break;
		}
	}

	public void HideUI()
	{
		foreach(UIBoards board in m_uiBoards)
		{
			if(board.uiBackground != null)
			{
				board.uiBackground.SetActive(false);
			}
			if(board.uiContent != null)
			{
				board.uiContent.SetActive(false);
			}
			foreach(GameObject label in board.satisfactionText)
			{
				label.SetActive(false);
			}
		}
	}

	public void HideNotifBoard()
	{
		m_notifBoard.SetActive(false);
	}

	public void SetDayLabel(int p_day)
	{
		m_uiBoards[2].satisfactionText[0].GetComponent<Text>().text = p_day.ToString();
	}

	public void SetSatisfactionRating(float p_rating)
	{
		if(m_uiBoards[0].satisfactionText[0].activeSelf)
		{
			m_uiBoards[0].satisfactionText[0].GetComponent<Text>().text = Mathf.RoundToInt(p_rating).ToString() + "%";
		}
		else if(m_uiBoards[2].satisfactionText[1].activeSelf)
		{
			m_uiBoards[2].satisfactionText[1].GetComponent<Text>().text = Mathf.RoundToInt(p_rating).ToString() + "%";
		}
		else if(m_uiBoards[3].satisfactionText[0].activeSelf)
		{
			m_uiBoards[3].satisfactionText[0].GetComponent<Text>().text = Mathf.RoundToInt(p_rating).ToString() + "%";
		}
	}

	public void SetNewItemNotif(int p_itemType, UnlockType p_type)
	{

		if(p_type == UnlockType.Flavor)
		{
			List<Flavors> flavor = Utilities.GetFlavor(p_itemType);

			switch(flavor[0])
			{
				case Flavors.Vanilla: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_flavor_vanilla");
				break;
				case Flavors.Chocolate: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_flavor_chocolate");
				break;
				case Flavors.Strawberry: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_flavor_strawberry");
				break;
				case Flavors.Bubblegum: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_flavor_bubblegum");
				break;
				case Flavors.GreenTea: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_flavor_greentea");
				break;
			}

			m_notifBoardText.text = "You have unlocked a new flavor!";
		}
		else if(p_type == UnlockType.Topping)
		{
			List<Toppings> topping = Utilities.GetToppings(p_itemType);

			switch(topping[0])
			{
				case Toppings.CrushedOreos: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_toppings_oreo");
				break;
				case Toppings.GummyWorms: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_toppings_gummypets");
				break;
				case Toppings.RainbowSprinkles: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_toppings_sprinkles");
				break;
				case Toppings.RiceKrispies: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_toppings_ricekrispies");
				break;
				case Toppings.TinyMarshmallow: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_toppings_marshmallow");
				break;
			}

			m_notifBoardText.text = "You have unlocked a new topping!";
		}
		else if(p_type == UnlockType.Syrup)
		{
			List<Syrups> syrup = Utilities.GetSyrups(p_itemType);

			switch(syrup[0])
			{
				case Syrups.Blueberry: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_syrup_blueberry");
				break;
				case Syrups.Caramel: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_syrup_caramel");
				break;
				case Syrups.Chocolate: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_syrup_chocolate");
				break;
				case Syrups.Honey: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_syrup_honey");
				break;
				case Syrups.Strawberry: m_notifBoardImage.sprite = AssetArchive.Instance.GetSpriteIcon("ICON_syrup_strawberry");
				break;
			}

			m_notifBoardText.text = "You have unlocked a new syrup!";
		}
	}
}
