using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Utilities 
{
	
	#region Strings

	public static string RemoveSubstring (string p_mainStr, string p_subStr)
	{
		p_mainStr = p_mainStr.Replace(p_subStr, string.Empty);
		p_mainStr = p_mainStr.Trim();
		return p_mainStr;
	}

	#endregion

	#region Instantiate

	public static GameObject InstantiateResource( string p_path, Transform p_parent = null )
	{
		UnityEngine.Object resourceObj = Resources.Load( p_path );
		if( resourceObj == null ) {
			D.assert( resourceObj != null );
			return null;
		}
		
		GameObject gameObj = GameObject.Instantiate( resourceObj ) as GameObject;
		if( gameObj == null ) {
			D.assert( gameObj != null );
			return null;
		}

		gameObj.name = Utilities.RemoveSubstring( gameObj.name, "(Clone)" );

		if( p_parent != null ) {
			gameObj.transform.SetParent( p_parent );
		}

		return gameObj;
	}

	public static T InstantiateResource<T>( string p_path, Transform p_parent = null ) where T:MonoBehaviour
	{
		T comp = InstantiateResource( p_path, p_parent ).GetComponent<T>();
		if( comp == null ) {
			D.assert( comp != null );
			return null;
		}

		return comp;
	}

	public static T InstantiateManager<T>( string p_name, Transform p_parent = null ) where T:MonoBehaviour
	{
		return InstantiateResource<T>( "Prefabs/Managers/" + p_name, p_parent );
	}

	public static T InstantiateCanvas<T>( string p_name, Transform p_parent = null ) where T:MonoBehaviour
	{
		return InstantiateResource<T>( "Prefabs/Canvas/" + p_name, p_parent );
	}

	#endregion

	#region Network

	public static bool HasInternet()
	{
		NetworkReachability reachability = Application.internetReachability;

		switch( reachability )
		{
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			return true;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			return true;
		}

		return false;
	}

	#endregion


	#region UI

	public static bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
//		if (results.Count > 0)	D.log (results[0].gameObject);
		return results.Count > 0;
	}

	public static bool IsPointerOverUIObjectAndNot3DObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		for ( int i = 0; i < results.Count; i++ )
		{
			if ( results[i].gameObject.layer != 10 ) {
				return true;
			}
		}
		return false;
	}

	#endregion

	#region DateTime
		
	public static System.Collections.IEnumerator WaitForRealTime( float p_delay )
	{
		while( true ){
			float pauseEndTime = Time.realtimeSinceStartup + p_delay;
			while ( Time.realtimeSinceStartup < pauseEndTime ){
				yield return 0;
			}
			break;
		}
	}

	public static int GetElapsedTimeInSeconds( DateTime p_startDateTime )
	{
		TimeSpan span = System.DateTime.Now.Subtract( p_startDateTime );		
		return span.Seconds;
	}

	#endregion

	#region NavMesh

	public static float GetPathLength( UnityEngine.AI.NavMeshPath path )
	{
		float d = float.PositiveInfinity;

		if (path != null && path.corners.Length > 1)
		{
			d = 0.0f;
			for (int i = 0; i < path.corners.Length-1; i++)
			{
				var p0 = path.corners[i + 0];
				var p1 = path.corners[i + 1];
				d += Vector3.Distance(p1, p0);
			}
		}
		return d;
	}

	#endregion

	#region Math

	// returns -1 when to the left, 1 to the right, and 0 for forward/backward
	public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);

		if (dir > 0.0f) {
			return 1.0f;
		} else if (dir < 0.0f) {
			return -1.0f;
		} else {
			return 0.0f;
		}
	}

	#endregion

	public static int OnesCount(int p_input)
	{
		// source: http://aggregate.org/MAGIC/#Population Count (Ones Count)
		p_input -= ((p_input >> 1) & 0x55555555);
		p_input  = (((p_input >> 2) & 0x33333333) + (p_input & 0x33333333));
		p_input  = (((p_input >> 4) + p_input) & 0x0f0f0f0f);
		p_input += (p_input >> 8);
		p_input += (p_input >> 16);
		p_input &= 0x0000003f;

		return p_input;
	}

	public static int ChooseFlavor(List<int> p_flavor)
	{
		int coneFlavor = 0;

		foreach(int flavor in p_flavor)
		{
			switch (flavor) {
			case 1:
				coneFlavor |= (int)Flavors.Chocolate;
				break;
			case 2:
				coneFlavor |= (int)Flavors.Vanilla;
				break;
			case 3: 
				coneFlavor |= (int)Flavors.Strawberry;
				break;
			case 4: 
				coneFlavor |= (int)Flavors.Bubblegum;
				break;
			case 5: 
				coneFlavor |= (int)Flavors.GreenTea;
				break;
			}
		}

		return coneFlavor;
	}

	public static int ChooseTopping(List<int> p_topping)
	{
		int coneTopping = 0;

		foreach(int topping in p_topping)
		{
			switch (topping) {
			case 1:
				coneTopping |= (int)Toppings.TinyMarshmallow;
				break;
			case 2:
				coneTopping |= (int)Toppings.RainbowSprinkles;
				break;
			case 3: 
				coneTopping |= (int)Toppings.RiceKrispies;
				break;
			case 4: 
				coneTopping |= (int)Toppings.GummyWorms;
				break;
			case 5: 
				coneTopping |= (int)Toppings.CrushedOreos;
				break;
			}
		}

		return coneTopping;
	}

	public static int ChooseSyrup(List<int> p_syrup)
	{
		int coneSyrup = 0;

		foreach(int syrup in p_syrup)
		{
			switch (syrup) {
			case 1:
				coneSyrup |= (int)Syrups.Chocolate;
				break;
			case 2:
				coneSyrup |= (int)Syrups.Caramel;
				break;
			case 3: 
				coneSyrup |= (int)Syrups.Strawberry;
				break;
			case 4: 
				coneSyrup |= (int)Syrups.Blueberry;
				break;
			case 5: 
				coneSyrup |= (int)Syrups.Honey;
				break;
			}
		}

		return coneSyrup;
	}

	public static List<Flavors> GetFlavor(int p_flavors)
	{
		// Debug.Log("p_flavors: " + p_flavors);

		List<Flavors> flavors = new List<Flavors> ();

		if ((p_flavors & (int) Flavors.Chocolate) != 0 ) {
			flavors.Add (Flavors.Chocolate);
		}
		if ((p_flavors & (int) Flavors.Vanilla) != 0 ) {
			flavors.Add (Flavors.Vanilla);
		}
		if ((p_flavors & (int) Flavors.Strawberry) != 0 ) {
			flavors.Add (Flavors.Strawberry);
		}
		if ((p_flavors & (int) Flavors.Bubblegum) != 0 ) {
			flavors.Add (Flavors.Bubblegum);
		}
		if ((p_flavors & (int) Flavors.GreenTea) != 0 ) {
			flavors.Add (Flavors.GreenTea);
		}
		
		// foreach(Flavors it in flavors)
		// {
		// 	Debug.Log(it);
		// }

		return flavors;
	}

	public static List<Toppings> GetToppings(int p_toppings)
	{
		// Debug.Log("p_toppings: " + p_toppings);

		List<Toppings> toppings = new List<Toppings> ();

		if ((p_toppings & (int) Toppings.TinyMarshmallow) != 0 ) {
			toppings.Add (Toppings.TinyMarshmallow);
		}
		if ((p_toppings & (int) Toppings.RainbowSprinkles) != 0 ) {
			toppings.Add (Toppings.RainbowSprinkles);
		}
		if ((p_toppings & (int) Toppings.RiceKrispies) != 0 ) {
			toppings.Add (Toppings.RiceKrispies);
		}
		if ((p_toppings & (int) Toppings.GummyWorms) != 0 ) {
			toppings.Add (Toppings.GummyWorms);
		}
		if ((p_toppings & (int) Toppings.CrushedOreos) != 0 ) {
			toppings.Add (Toppings.CrushedOreos);
		}

		// foreach(Toppings it in toppings)
		// {
		// 	Debug.Log(it);
		// }

		return toppings;
	}

	public static List<Syrups> GetSyrups(int p_syrups)
	{
		// Debug.Log("p_syrups: " + p_syrups);

		List<Syrups> syrups = new List<Syrups> ();

		if ((p_syrups & (int) Syrups.Chocolate) != 0 ) {
			syrups.Add (Syrups.Chocolate);
		}
		if ((p_syrups & (int) Syrups.Caramel) != 0 ) {
			syrups.Add (Syrups.Caramel);
		}
		if ((p_syrups & (int) Syrups.Strawberry) != 0 ) {
			syrups.Add (Syrups.Strawberry);
		}
		if ((p_syrups & (int) Syrups.Blueberry) != 0 ) {
			syrups.Add (Syrups.Blueberry);
		}
		if ((p_syrups & (int) Syrups.Honey) != 0 ) {
			syrups.Add (Syrups.Honey);
		}

		// foreach(Syrups it in syrups)
		// {
		// 	Debug.Log(it);
		// }

		return syrups;
	}
}
