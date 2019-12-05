using UnityEngine;
using System;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	[SerializeField] private MeshRenderer m_blinder;

	private IEnumerator m_coroutine;
	private Material m_material;
	private Action m_callback;

	public bool IsPlaying{private set; get;}

	protected void Awake()
	{
		m_material = m_blinder.material;
		IsPlaying = false;
		m_blinder.gameObject.SetActive(true);
	}

	public void FadeOut(float p_duration, Action p_callback = null)
	{
		if(IsPlaying){ return; }
		// Debug.LogWarning("fading out");

		// if(m_coroutine != null){ StopCoroutine(m_coroutine); }
		m_coroutine = AnimateColorAlpha(0.0f, 1.0f, p_duration);
		m_callback = p_callback;
		IsPlaying = true;
		ControlsManager.Instance.SetPausable(false);
		// Debug.LogError(ControlsManager.Instance.GetPausable());
		StartCoroutine(m_coroutine);
	}

	public void FadeIn(float p_duration, Action p_callback = null)
	{
		if(IsPlaying){ return; }
		// Debug.LogWarning("fading in");

		// if(m_coroutine != null){ StopCoroutine(m_coroutine); }
		m_coroutine = AnimateColorAlpha(1.0f, 0.0f, p_duration);
		m_callback = p_callback;
		IsPlaying = true;
		ControlsManager.Instance.SetPausable(false);
		// Debug.LogError(ControlsManager.Instance.GetPausable());
		StartCoroutine(m_coroutine);
	}

	private IEnumerator AnimateColorAlpha(float p_from, float p_to, float p_duration)
	{
		Color colorMat = m_material.color;
		float lapseTime = 0.0f;
		float t = 0.0f;

		p_duration = 1.0f / p_duration;

		while (t < 1.0f)
		{
			lapseTime += Time.deltaTime;
			t = lapseTime * p_duration;
			colorMat.a = Mathf.Lerp(p_from, p_to, t);
			m_material.color = colorMat;
			yield return null;
		}

		colorMat.a = p_to;
		m_material.color = colorMat;
		IsPlaying = false;

		if(p_to == 0)
		{
			ControlsManager.Instance.SetPausable(true);
			// Debug.LogError(ControlsManager.Instance.GetPausable());
		}

		if(m_callback != null)
		{
			m_callback();
		}
	}
}
