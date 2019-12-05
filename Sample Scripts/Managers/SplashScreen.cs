using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SplashScreen : MonoInstance<SplashScreen>
{
	private const float ANIMATION_DELAY = 0.5f;
	private const float FADE_SPEED = 2.0f;
	private const float DISPLAY_DURATION = 2.0f;

	[SerializeField] private Renderer m_logoRenderer;
	private Material m_logoMat;

	#region IMPLEMENT MONOINSTANCE FUNCTIONS
	public override void InitializeManager (SceneType s)
	{
		if(s == SceneType.Start){ StartCoroutine(AnimateLogo()); }
	}

	public override void CleanupManager (SceneType s) {}
	#endregion // IMPLEMENT MONOINSTANCE FUNCTIONS

	#region MONOBEHAVIOUR FUNCTIONS
	protected override void Awake()
	{
		base.Awake();
		Assert.IsNotNull(m_logoRenderer);
		m_logoMat = m_logoRenderer.material;
	}
	#endregion // MONOBEHAVIOUR FUNCTIONS

	private IEnumerator AnimateLogo()
	{
		yield return new WaitForSeconds(ANIMATION_DELAY);
		Color color = m_logoMat.GetColor("_Color");
		float alpha = 0.0f;

		m_logoRenderer.enabled = true;

		while(alpha < 1.0f)
		{
			alpha  += Time.deltaTime * FADE_SPEED;
			color   = m_logoMat.GetColor("_Color");
			color.a = alpha;

			m_logoMat.SetColor("_Color", color);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(DISPLAY_DURATION);

		while(alpha > 0.0f)
		{
			alpha  -= Time.deltaTime * FADE_SPEED;
			color   = m_logoMat.GetColor("_Color");
			color.a = alpha;

			m_logoMat.SetColor("_Color", color);
			yield return new WaitForEndOfFrame();
		}

		m_logoRenderer.enabled = false;
		yield return new WaitForSeconds(ANIMATION_DELAY);
		// GameStateManager.Instance.SwitchToIntro();
		GameStateManager.Instance.SwitchToTitle();
	}
}
