using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : PoolableObject {

	private AudioSource m_source;

	// Use this for initialization
	void Awake () {

		m_source = GetComponent<AudioSource> ();	

	}

	void Update () {
		if (m_source.clip != null) {
			
			if (m_source.isPlaying == false) {
				Deinitialize ();
//				D.log("Removed");
			}
		}
	}

	public override void Initialize (ObjectPool pParent) {
		base.Initialize (pParent);
	}

	public override void Deinitialize () {
		base.Deinitialize ();
		gameObject.SetActive (false);
	}

	public void GeneratePosition(Vector3 p_soundPosition)
	{
		transform.position = p_soundPosition;
	}

	public void PlayLoop(string p_loopName)
	{
		AudioClip currAudio = AudioManager.Instance.GetAudio(p_loopName);

		if (currAudio) 
		{
			m_source.loop = true;
			m_source.clip = currAudio;
			m_source.Play ();
		}
	}

	public void PlayOnce(string p_audioName, float p_volume = 1.0f)
	{
		AudioClip currAudio = AudioManager.Instance.GetAudio(p_audioName);

		if (currAudio) 
		{
			m_source.loop = false;
			m_source.clip = currAudio;
			m_source.volume = p_volume;
			m_source.PlayOneShot (currAudio);
		}
	}

	public void PlayRandomStart(string p_audioName, float p_rangeMin, float p_rangeMax)
	{
		AudioClip currAudio = AudioManager.Instance.GetAudio(p_audioName);

		if (currAudio) 
		{
			float start = Random.Range (p_rangeMin, p_rangeMax);
			m_source.clip = currAudio;
			m_source.time = start * m_source.clip.length;

			m_source.Play();
		}
	}

	public void StopAudio()
	{
		m_source.Stop();
		Deinitialize ();
	}

	public void PauseAudio()
	{
		m_source.Pause();
	}

	public void ContinueAudio()
	{
		m_source.UnPause();
	}
}
