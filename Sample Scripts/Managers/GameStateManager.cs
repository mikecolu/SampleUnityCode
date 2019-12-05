using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoInstance<GameStateManager> {

	public List<IManager> managers = new List<IManager>();

	// Use this for initialization
	void Start () {
		
		SceneController.Instance.StateToStart ();

		AssetArchive.Instance.LoadAssets ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (KeyCode.Q)) {
			SceneController.Instance.StateToTitle ();
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			SceneController.Instance.StateToGame();
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			// activate pause
		}
		if (Input.GetKeyUp (KeyCode.R)) {
			SceneController.Instance.StateToEnd ();
		}
	}

	public override void InitializeManager (SceneType s) {}
	public override void CleanupManager (SceneType s) {}

	public void RegisterManager(IManager manager) {
		if (!managers.Contains (manager)) {
			managers.Add (manager);
		}
	}

	public SceneType GetCurrentState() {
		return SceneController.Instance.GetCurrentState ();
	}

	public void SwitchToState(SceneType p_sceneType) {
		SceneController.Instance.StateTo(p_sceneType);
	}

	public void SwitchToStart() {
		SceneController.Instance.StateToStart();
	}

	public void SwitchToIntro() {
		SceneController.Instance.StateToIntro();
	}

	public void SwitchToTitle() {
		SceneController.Instance.StateToTitle();
	}

	public void SwitchToGame() {
		SceneController.Instance.StateToGame ();
	}

	public void SwitchToEnd() {
		SceneController.Instance.StateToEnd ();
	}
}
