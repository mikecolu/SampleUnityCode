using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("leveldata")]
public class LevelDataXml
{
	[XmlElement("day")] public List<DayData> listDayData;

	public DayData[] days;

	public LevelDataXml(){
	}

	public LevelDataXml( List<DayData> p_days )
	{
		days = new DayData[p_days.Count];

		for( int i = 0; i < p_days.Count; i++ ) {

			days[i] = new DayData();
			days[i].day = p_days[i].day;
			days [i].satisfactionGoal = p_days [i].satisfactionGoal;
			days [i].levelTime = p_days [i].levelTime;
			days [i].satisfactionDepreciation = p_days [i].satisfactionDepreciation;
			days [i].customerType = p_days [i].customerType;
			days [i].unlockedFlavors = p_days [i].unlockedFlavors;
			days [i].unlockedSyrups = p_days [i].unlockedSyrups;
			days [i].unlockedToppings = p_days [i].unlockedToppings;
		}
	}

	public static LevelDataXml Load( string p_strPath )
	{
		return (LevelDataXml)XmlHelper.LoadFromResources( "LevelData/" + p_strPath, typeof(LevelDataXml) );
	}

	public void Save( string p_strPath )
	{
		XmlHelper.SaveToResources( "LevelData/" + p_strPath, typeof(LevelDataXml), this );
	}
}

[XmlType("day")]
public class DayData
{
	[XmlAttribute("day")] public int day;
	[XmlAttribute("satisfaction")] public int satisfactionGoal;
	[XmlAttribute("timer")] public int levelTime;
	[XmlAttribute("depreciation")] public float satisfactionDepreciation;
	[XmlAttribute("customer")] public int customerType;
	[XmlAttribute("flavors")] public int unlockedFlavors;
	[XmlAttribute("toppings")] public int unlockedToppings;
	[XmlAttribute("syrups")] public int unlockedSyrups;

	public DayData(){}

public enum CustomerType
{
	AverageJoe = 1 << 0,
	ChillBro = 1 << 1,
	WhishyWashy = 1 << 2,
	Critic = 1 << 3
}

// Type of Flavors
public enum Flavors
{
	Chocolate = 1 << 0,
	Vanilla  = 1 << 1,
	Strawberry = 1 << 2,
	Bubblegum   = 1 << 3,
	GreenTea    = 1 << 4
}

// Type of Toppings
public enum Toppings
{
	TinyMarshmallow      = 1 << 0,
	RainbowSprinkles = 1 << 1,
	RiceKrispies   = 1 << 2,
	GummyWorms   = 1 << 3,
	CrushedOreos    = 1 << 4
}

// Type of Syrups
public enum Syrups
{
	Chocolate      = 1 << 0,
	Caramel = 1 << 1,
	Strawberry   = 1 << 2,
	Blueberry   = 1 << 3,
	Honey    = 1 << 4
}