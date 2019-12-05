using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoInstance<SpawnManager> {

	[SerializeField]
	private string m_levelDataFile;
	private int m_level = 0;
	private Coroutine mSpawnCoroutine = null;
	private Coroutine mSpawnCone = null;
	[SerializeField]
	private List<PoolableObject> gSpawnables;
	private List<ObjectPool> mSpawnObjectPools = new List<ObjectPool> ();
	[SerializeField]
	private List<GameObject> gSpawnZones;


	private LevelDataXml mLevelData = null;

	private int m_customerCount = 0;
	private int m_coneCount = 0;

	public int GetCustomerCount()
	{
		return m_customerCount;
	}

	public void ReduceCustomerCount()
	{
		m_customerCount--;
	}

	public void ResetCustomerCount()
	{
		m_customerCount = 0;
	}

	public void ReduceConeCount()
	{
		m_coneCount--;
	}

	public void ResetConeCount()
	{
		m_coneCount = 0;
	}

	public void ResetSpawnCounts()
	{
		m_customerCount = 0;
		m_coneCount = 0;
	}

	void Start(){

		foreach(PoolableObject p in gSpawnables) {
			if (p != null) {
				ObjectPool o = new ObjectPool ();
				o.Initialize (p);
				mSpawnObjectPools.Add (o);
			}
		}

	}

	public override void InitializeManager (SceneType s) {
		if (s == SceneType.Game || s == SceneType.Tutorial) {
			
			StartSpawn ();

		}
	}

	public override void CleanupManager (SceneType s) {
		if (s == SceneType.Game || s == SceneType.Tutorial) {
			StopSpawn ();
		}
	}

	private void SetLevel( int i) {
		m_level = i;
	}

	private void StartSpawn()
	{
		mSpawnCoroutine = StartCoroutine( SpawnCustomerCoroutine() );
		mSpawnCone = StartCoroutine ( SpawnConeCoroutine() );
	}

	private void StopSpawn() {
		if (mSpawnCoroutine != null) {
			StopCoroutine (mSpawnCoroutine);
		}

		if (mSpawnCone != null) {
			StopCoroutine (mSpawnCone);
		}

	}

	private GameObject SpawnObject(int i)
	{
		if (GameStateManager.Instance.GetCurrentState () != SceneType.Game && GameStateManager.Instance.GetCurrentState () != SceneType.Tutorial)
			return null;

		GameObject returnedObj = null;

		if (mSpawnObjectPools.Count > i) {

			if (i == 0 || i == 1) {
				GameObject g = mSpawnObjectPools [i].Pull ().gameObject;
				if (g) {
					Vector3 vSpawnPoint = new Vector3 (0, 0, 0);
					vSpawnPoint = gSpawnZones [Random.Range (0, 2)].transform.position;
					g.transform.position = vSpawnPoint;

					UnityEngine.AI.NavMeshAgent gAgent = g.GetComponent<UnityEngine.AI.NavMeshAgent> ();
					if (gAgent) {
						gAgent.Warp (vSpawnPoint);
					}
					g.SetActive (true);
					returnedObj = g;
				}
			}

			if(i == 2)
			{
				GameObject g = mSpawnObjectPools [i].Pull ().gameObject;
				if (g) {
					Vector3 vSpawnPoint = new Vector3 (0, 0, 0);

					if (GameplayManager.Instance.GetConeMiddlePos () == false) {
						if (GameplayManager.Instance.GetConePoint (1) != null) {
							vSpawnPoint = GameplayManager.Instance.GetConePoint (1);
							GameplayManager.Instance.SetConeMiddlePos (true);
							g.GetComponent<Cone> ().SetConePos (ConePosition.Middle);

						}
					} else if (GameplayManager.Instance.GetConeLeftPos () == false) {
						if ( GameplayManager.Instance.GetConePoint (0) != null) {
							vSpawnPoint =  GameplayManager.Instance.GetConePoint (0);
							GameplayManager.Instance.SetConeLeftPos (true);
							g.GetComponent<Cone> ().SetConePos (ConePosition.Left);
						}
					} else if (GameplayManager.Instance.GetConeRightPos () == false) {
						if ( GameplayManager.Instance.GetConePoint (2) != null) {
							vSpawnPoint =  GameplayManager.Instance.GetConePoint (2);
							GameplayManager.Instance.SetConeRightPos (true);
							g.GetComponent<Cone> ().SetConePos (ConePosition.Right);
						}
					}

					g.transform.position = vSpawnPoint;
					g.SetActive (true);
					g.GetComponent<Cone> ().SnapCone (vSpawnPoint);
					returnedObj = g;
				}
			}
		}

		return returnedObj;
	}

	private IEnumerator SpawnConeCoroutine()
	{
		while (true) {

			if (m_coneCount < 3) {

				m_coneCount++;
				GameObject g = SpawnObject (2);
			}

			yield return new WaitForSeconds( 0.5f );
		}
	}

	private IEnumerator SpawnCustomerCoroutine()
	{
		while (true) {
			

			if (m_customerCount < 3 && GameplayManager.Instance.IsWaveOver() == false && SceneController.Instance.GetCurrentState() == SceneType.Game) {

				m_customerCount++;
				int customerGender = Random.Range (0,2);

				GameObject g = SpawnObject (customerGender);

				if(g != null)
				{
					if(customerGender == 0)
					{
						g.GetComponent<Customer>().SetCustomerGender(Gender.Male);
					}
					else{
						g.GetComponent<Customer>().SetCustomerGender(Gender.Female);
					}
				}

			}
			else if(m_customerCount < 1 && GameplayManager.Instance.IsWaveOver() == false && SceneController.Instance.GetCurrentState() == SceneType.Tutorial && TutorialManager.Instance.GetTutorialPhase() == 3){

				m_customerCount++;
				int customerGender = Random.Range (0,2);

				GameObject g = SpawnObject (customerGender);

				if(g != null)
				{
					if(customerGender == 0)
					{
						g.GetComponent<Customer>().SetCustomerGender(Gender.Male);
					}
					else{
						g.GetComponent<Customer>().SetCustomerGender(Gender.Female);
					}
				}
			}

			yield return new WaitForSeconds( Random.Range (1,4) );
		}
	}
}
