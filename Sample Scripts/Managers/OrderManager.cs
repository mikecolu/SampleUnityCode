using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoInstance<OrderManager> {

	[SerializeField]
	private OrderCard m_orderCard;
	private ObjectPool m_orderCardPool;

	[SerializeField]
	private GameObject[] m_orderPositions;

	private bool[] m_bIsPositionOccupied;
	private OrderCard[] m_orderCards;

	#region Dictionary Variables
	[SerializeField]
	private FlavorIcons[] m_flavorLargeIcons;
	public Dictionary<Flavors, Sprite> m_flavorLargeIconsDict {private set; get;}

	[SerializeField]
	private FlavorIcons[] m_flavorSmallIcons;
	public Dictionary<Flavors, Sprite> m_flavorSmallIconsDict {private set; get;}

	[SerializeField]
	private ToppingIcons[] m_toppingLargeIcons;
	public Dictionary<Toppings, Sprite> m_toppingLargeIconsDict {private set; get;}

	[SerializeField]
	private ToppingIcons[] m_toppingSmallIcons;
	public Dictionary<Toppings, Sprite> m_toppingSmallIconsDict {private set; get;}

	[SerializeField]
	private SyrupIcons[] m_syrupLargeIcons;
	public Dictionary<Syrups, Sprite> m_syrupLargeIconsDict {private set; get;}

	[SerializeField]
	private SyrupIcons[] m_syrupSmallIcons;
	public Dictionary<Syrups, Sprite> m_syrupSmallIconsDict {private set; get;}

	[System.Serializable]
	public class FlavorIcons
	{
		public Flavors flavor;
		public Sprite image;
	}

	[System.Serializable]
	public class ToppingIcons
	{
		public Toppings topping;
		public Sprite image;
	}

	[System.Serializable]
	public class SyrupIcons
	{
		public Syrups syrup;
		public Sprite image;
	}
	#endregion //Dictionary Variables

	protected override void Awake()
	{
		base.Awake();

		m_flavorLargeIconsDict = new Dictionary<Flavors, Sprite>();
		foreach(FlavorIcons item in m_flavorLargeIcons)
		{
			m_flavorLargeIconsDict.Add(item.flavor, item.image);
		}

		m_flavorSmallIconsDict = new Dictionary<Flavors, Sprite>();
		foreach(FlavorIcons item in m_flavorSmallIcons)
		{
			m_flavorSmallIconsDict.Add(item.flavor, item.image);
		}

		m_toppingLargeIconsDict = new Dictionary<Toppings, Sprite>();
		foreach(ToppingIcons item in m_toppingLargeIcons)
		{
			m_toppingLargeIconsDict.Add(item.topping, item.image);
		}

		m_toppingSmallIconsDict = new Dictionary<Toppings, Sprite>();
		foreach(ToppingIcons item in m_toppingSmallIcons)
		{
			m_toppingSmallIconsDict.Add(item.topping, item.image);
		}

		m_syrupLargeIconsDict = new Dictionary<Syrups, Sprite>();
		foreach(SyrupIcons item in m_syrupLargeIcons)
		{
			m_syrupLargeIconsDict.Add(item.syrup, item.image);
		}

		m_syrupSmallIconsDict = new Dictionary<Syrups, Sprite>();
		foreach(SyrupIcons item in m_syrupSmallIcons)
		{
			m_syrupSmallIconsDict.Add(item.syrup, item.image);
		}
	}

	private void Start() {
		// initialize position occupancy booleans
		m_bIsPositionOccupied = new bool[m_orderPositions.Length];
		m_orderCards = new OrderCard[m_orderPositions.Length];
		for (int i = 0; i < m_bIsPositionOccupied.Length; i++) {
			m_bIsPositionOccupied [i] = false;
		}

		// initialize order card pool
		if(m_orderCard != null) {
			m_orderCardPool = new ObjectPool ();
			m_orderCardPool.Initialize (m_orderCard);
		}
	}

	private void Update() {
		// test
		if(Input.GetKeyDown(KeyCode.A)) {
			// DrawOrderCard (
			// 	new Flavors[] {Flavors.Bubblegum}, 
			// 	new Toppings[] {Toppings.RainbowSprinkles}, 
			// 	new Syrups[] {Syrups.Blueberry}
			// );
		}
		if(Input.GetKeyDown(KeyCode.D)) {
			int posIdx = -1;
			for (int i = 0; i < m_bIsPositionOccupied.Length; i++) {
				if (m_bIsPositionOccupied [i] != false) {
					posIdx = i;
					break;
				}
			}

			if (posIdx == -1) {
				return;
			}
			CleanOrderCard (posIdx);
		}
	}

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	#region Public Functions
	public bool DrawOrderCard(Flavors[] p_flavors, Toppings[] p_toppings, Syrups[] p_syrups, int p_drawLoc) {
		int posIdx = -1;
		for (int i = 0; i < m_bIsPositionOccupied.Length; i++) {
			if (m_bIsPositionOccupied [i] != true) {
				posIdx = p_drawLoc;
				break;
			}
		}

		if (posIdx == -1) {
			return false;
		}

		OrderCard card = m_orderCardPool.Pull ().GetComponent<OrderCard> ();
		if (card != null) {
			if (p_flavors != null) {
				for (int i = 0; i < p_flavors.Length; i++) {
					card.InitializeLargeImage (GetFlavorLargeIcon(p_flavors[i]));
					card.InitializeSmallImage (GetFlavorSmallIcon(p_flavors[i]));
				}
			}

			if (p_syrups != null) {
				for (int i = 0; i < p_syrups.Length; i++) {
					card.InitializeLargeImage (GetSyrupLargeIcon(p_syrups[i]));
					card.InitializeSmallImage (GetSyrupSmallIcon(p_syrups[i]));
				}
			}
			
			if (p_toppings != null) {
				for (int i = 0; i < p_toppings.Length; i++) {
					card.InitializeLargeImage (GetToppingLargeIcon(p_toppings[i]));
					card.InitializeSmallImage (GetToppingSmallIcon(p_toppings[i]));
				}
			}

			card.transform.position = m_orderPositions [posIdx].transform.position;
			card.gameObject.SetActive (true);
			m_bIsPositionOccupied [posIdx] = true;
			m_orderCards [posIdx] = card;
		}
			
		return true;
	}

	public bool CleanOrderCard(int i) {
		if (i >= m_orderCards.Length || m_orderCards [i] == null) {
			return false;
		}

		m_orderCards [i].Deinitialize ();
		m_orderCards [i] = null;
		m_bIsPositionOccupied [i] = false;
		return true;
	}
	#endregion //Public Functions


	#region Dictionary Boilerplate
	public Sprite GetFlavorLargeIcon(Flavors p_flavor)
	{
		if(m_flavorLargeIconsDict.ContainsKey(p_flavor)) {
			return m_flavorLargeIconsDict[p_flavor];
		}
		return null;
	}

	public Sprite GetFlavorSmallIcon(Flavors p_flavor)
	{
		if(m_flavorSmallIconsDict.ContainsKey(p_flavor)) {
			return m_flavorSmallIconsDict[p_flavor];
		}
		return null;
	}

	public Sprite GetSyrupLargeIcon(Syrups p_syrup)
	{
		if(m_syrupLargeIconsDict.ContainsKey(p_syrup)) {
			return m_syrupLargeIconsDict[p_syrup];
		}
		return null;
	}

	public Sprite GetSyrupSmallIcon(Syrups p_syrup)
	{
		if(m_syrupSmallIconsDict.ContainsKey(p_syrup)) {
			return m_syrupSmallIconsDict[p_syrup];
		}
		return null;
	}

	public Sprite GetToppingLargeIcon(Toppings p_topping)
	{
		if(m_toppingLargeIconsDict.ContainsKey(p_topping)) {
			return m_toppingLargeIconsDict[p_topping];
		}
		return null;
	}

	public Sprite GetToppingSmallIcon(Toppings p_topping)
	{
		if(m_toppingSmallIconsDict.ContainsKey(p_topping)) {
			return m_toppingSmallIconsDict[p_topping];
		}
		return null;
	}
	#endregion //Dictionary Boilerplate
}
