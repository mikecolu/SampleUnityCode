using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoInstance<SceneController>
{
	private const float TRANSITION_DURATION = 1f;

	#region PRIVATE VARIABLES
	private Animator        m_animator = null;
	private SceneTransition m_sceneTransition;
	#endregion // PRIVATE VARIABLES

	#region DELEGATES
	public delegate void TransitionState();
	public static event TransitionState OnRevealScene;
	public static event TransitionState OnApplySceneChange;
	#endregion // DELEGATES

	private SceneType m_currentScene = SceneType.Start;

	protected override void Awake()
	{
		base.Awake();

		m_animator = GetComponent<Animator>();
		Debug.Assert( m_animator != null, "Scene Controller Animator is null." );

		m_sceneTransition = GetComponent<SceneTransition>();
		Debug.Assert( m_sceneTransition != null, "SceneTransition component is null." );
	}

	private void Start() {
		SceneManager.LoadSceneAsync (1, LoadSceneMode.Additive);
	}

	public override void InitializeManager (SceneType s) 
	{
		if(s == SceneType.Intro)
		{
			RevealScene();
		}
	}
	public override void CleanupManager (SceneType s) {}

	public void StateToStart() 	{ StateTo( SceneType.Start ); }
	public void StateToIntro() 	{ StateTo( SceneType.Intro ); }
	public void StateToTitle() 	{ StateTo( SceneType.Title ); }
	public void StateToTutorial() { StateTo( SceneType.Tutorial );}
	public void StateToGame() 	{ StateTo( SceneType.Game ); }
	public void StateToEnd() 	{ StateTo( SceneType.End ); }

	#region PUBLIC FUNCTIONS
	public SceneType GetCurrentState() {
		return m_currentScene;
	}

	public void HideAndLoadScene(SceneType p_sceneType)
	{
		m_currentScene = p_sceneType;
		m_sceneTransition.FadeOut(TRANSITION_DURATION, ApplyChange);
	}

	public void RevealScene()
	{
		if(OnRevealScene != null){ OnRevealScene(); }
		m_sceneTransition.FadeIn(TRANSITION_DURATION);
	}

	public void StateTo(SceneType p_sceneType) {
		if(GetCurrentState() != p_sceneType) 
			Load(p_sceneType); 
	}
	#endregion // PUBLIC FUNCTIONSs

	#region PRIVATE FUNCTIONS
	private void Load( SceneType p_type )
	{
		Debug.Log( "Scene Controller: State to - " + p_type.ToString() );

		m_currentScene = p_type;

		if( m_animator != null ) {		
			m_animator.Play( p_type.ToString() );
		}		
	}

	private void ApplyChange()
	{
		if(OnApplySceneChange != null){ OnApplySceneChange(); }
		if( m_animator != null )
		{
			m_animator.Play(GetCurrentState().ToString());
		}	
	}
	#endregion // PRIVATE FUNCTIONS
}
