using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{

	private const int INITIAL_OBJECT_COUNT = 3;

	private List<PoolableObject> pool = new List<PoolableObject>();
	private PoolableObject poolObject = null;
	private bool isInitialized = false;
	private GameObject gRoot;
	private int idx = 0;

	public void Initialize(PoolableObject pPoolObject) {
		gRoot = new GameObject (pPoolObject.name + " ObjectPool");
		if (!isInitialized) {
			isInitialized = true;
			poolObject = pPoolObject;
			for (int i = 0; i < INITIAL_OBJECT_COUNT; i++) {
				PoolableObject po = CreateObject ();
				po.gameObject.SetActive (false);
				Push (po);
			}
		}
	}

	private PoolableObject CreateObject() {
		PoolableObject po = GameObject.Instantiate<PoolableObject> (poolObject);
		po.name += " " + idx;
		po.Initialize (this);
		po.transform.SetParent (gRoot.transform);
		idx++;
		return po;
	}

	public void Push(PoolableObject pNewObject) {
		if (!isInitialized) {
			Initialize(GameObject.Instantiate(pNewObject));
		}
		if (!pool.Contains (pNewObject)) {
			pool.Add (pNewObject);
		}
	}

	public PoolableObject Pull()
	{
		PoolableObject returnedObject = null;
		if (pool.Count > 0) {
			returnedObject = pool [0];
			pool.RemoveAt (0);
		} else {
			if (!isInitialized) {
				Debug.LogError ("ObjectPool.Pull() error: no pool object assigned! Returning null");
			} else {
				returnedObject = CreateObject ();
			}
		}
		return returnedObject;
	}
}
