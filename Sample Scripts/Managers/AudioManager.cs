using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoInstance<AudioManager> {

	public AudioMixer m_masterMixer;
	public AudioMixerSnapshot m_paused;
	public AudioMixerSnapshot m_unpaused;

	public List<AudioClip> m_loopingClip;
	public List<AudioClip> m_triggerClip;

	private AudioSource m_source;
	public List<AudioClip> m_musicClip;
	private Dictionary<string, AudioClip> m_audioDict = new Dictionary<string, AudioClip>();

	[SerializeField]
	private Transform m_HMD;

	[SerializeField]
	private PoolableObject m_audioObject;
	private static ObjectPool mSpawnAudio = new ObjectPool();

	void Start()
	{
		m_source = GetComponent<AudioSource> ();	
		InitAudio ();
		mSpawnAudio.Initialize (m_audioObject);
		PlayMusic (Constants.BGM_MAINMENU);
	}

	void Update()
	{
		if (m_HMD != null) 
		{
			transform.position = m_HMD.transform.position;
		}
	}

	public override void InitializeManager(SceneType s)
	{
		if (s == SceneType.Title) {
			PlayMusic (Constants.BGM_MAINMENU);
		}

		if (s == SceneType.Tutorial) {
			PlayMusic (Constants.BGM_INGAME);
		}
	}

	public override void CleanupManager(SceneType s)
	{

	}

	void PauseGame()
	{
		ClearVolume ();
		m_paused.TransitionTo (0.01f);
	}

	void UnpauseGame()
	{
		m_unpaused.TransitionTo (0.01f);
	}

	public void SetSfxLevel(float p_level)
	{
		m_masterMixer.SetFloat ("soundEffectsTrackVolume", p_level);
	}

	public void SetMusicLevel(float p_level)
	{
		m_masterMixer.SetFloat ("musicTrackVolume", p_level);
	}

	private void ClearVolume()
	{
		m_masterMixer.ClearFloat ("musicTrackVolume");
	}

	public GameObject SpawnAudio(Vector3 p_position) {
		bool spawnedSuccessfully = false;

		GameObject g = mSpawnAudio.Pull ().gameObject;
			if (g) {
				g.SetActive (true);
				g.GetComponent<AudioController> ().GeneratePosition (p_position);

				return g;
			}

		return null;
	}

	void InitAudio()
	{
		foreach (AudioClip audio in m_loopingClip) 
		{
			m_audioDict.Add (audio.name, audio);
		}

		foreach (AudioClip audio in m_triggerClip) 
		{
			m_audioDict.Add (audio.name, audio);
		}

		foreach (AudioClip audio in m_musicClip) 
		{
			m_audioDict.Add (audio.name, audio);
		}
	}

	public void PlayMusic(string p_musicName)
	{
		if (m_source != null && m_source.clip != null &&
			p_musicName == m_source.clip.name)
			return;
		
		AudioClip currAudio = GetAudio(p_musicName);
		D.log (currAudio);
		if (currAudio) 
		{
			m_source.loop = true;
			m_source.clip = currAudio;
			m_source.Play ();
		}
	}

	public AudioClip GetAudio(string p_audioName)
	{
		AudioClip currAudio;

		if (m_audioDict.TryGetValue (p_audioName, out currAudio)) {

			return currAudio;
		}

		return null;
	}
}
