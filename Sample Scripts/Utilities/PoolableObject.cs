using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour, IPoolable
{
	private ObjectPool parentPool;

	public virtual void Initialize (ObjectPool pParent) {
		parentPool = pParent;
	}
		
	public virtual void Deinitialize () {
		parentPool.Push (this);
	}
}
