using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class MonoInstance<T> : MonoBehaviour, IManager where T : MonoBehaviour
{
	protected static T s_instance = null;
	public static T Instance
    { 
        get
        {
            if( s_instance == null )
            {
                // s6bri:
                // to ensure that we have an instance value
                // in case this is accessed from awake of another class
                // which is executed before the awake of this class<T>
                s_instance = GameObject.FindObjectOfType<T>();
            }

            return s_instance;
        }
    }

	public static void Create( Transform p_parent = null )
	{
		if( s_instance == null ) {
			s_instance = Utilities.InstantiateManager<T>( typeof( T ).Name, p_parent );
		}
	}

	protected virtual void Awake()
	{
		if( s_instance == null ) {
			s_instance = this as T;
		}
		if (GameStateManager.Instance) {
			GameStateManager.Instance.RegisterManager (this);
		}
	}

	protected virtual void OnDestroy()
	{
		s_instance = null;
	}

	public abstract void InitializeManager (SceneType s);
	public abstract void CleanupManager (SceneType s);
}
