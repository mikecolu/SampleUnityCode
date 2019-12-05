using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Customer))]
public class AnimationHandler : MonoBehaviour {

	private Animator m_animator;
	private Customer m_customer;

	void Awake()
	{
		m_animator = GetComponent<Animator> ();
		m_customer = GetComponent<Customer> ();
	}

	void OnEnable()
	{
		m_customer.aOnSpeedChanged += OnSpeedChanged;
		m_customer.aOnOpenMouth += OnOpenMouth;
		m_customer.aOnMunching += OnMunching;
		m_customer.aOnCalmIdle += OnCalmIdle;
		m_customer.aOnImpatientIdle += OnImpatientIdle;
		m_customer.aOnCorrect += OnCorrect;
		m_customer.aOnWrong += OnWrong;
		m_customer.aOnWillWalk += OnWillWalk;
		m_customer.aOnIsEating += OnIsEating;
		m_customer.aOnHappy += OnHappy;
		m_customer.aClearAnimation += ResetTriggers;
	}

	void OnDisable()
	{
		m_customer.aOnSpeedChanged -= OnSpeedChanged;
		m_customer.aOnOpenMouth -= OnOpenMouth;
		m_customer.aOnMunching -= OnMunching;
		m_customer.aOnCalmIdle -= OnCalmIdle;
		m_customer.aOnImpatientIdle -= OnImpatientIdle;
		m_customer.aOnCorrect -= OnCorrect;
		m_customer.aOnWrong -= OnWrong;
		m_customer.aOnWillWalk -= OnWillWalk;
		m_customer.aOnIsEating -= OnIsEating;
		m_customer.aOnHappy -= OnHappy;
		m_customer.aClearAnimation += ResetTriggers;
	}

	private void OnSpeedChanged(float speed, float maxSpeed) {
		float spdRatio = speed / maxSpeed;
		m_animator.SetFloat ("Blend", spdRatio);
	}

	private void OnWillWalk(bool willWalk) {

		// Debug.Log("Will Walk: " + willWalk);
		m_animator.SetBool ("IsMoving", willWalk);
	}

	private void OnIsEating(bool isEating) {

		m_animator.SetBool ("IsEating", isEating);
	}

	private void OnOpenMouth() {
		// ResetTriggers();

		// Debug.Log("State Open: " + m_animator.GetCurrentAnimatorStateInfo(0).IsName("Open Mouth"));

		if(m_animator.GetBool("IsMoving") == false && m_animator.GetBool("IsEating") == false)// && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Open Mouth") == false )// && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") == true)
		{
			m_animator.SetTrigger ("DoOpenMouth");
			m_customer.ChangeFacialExpression(4);

			if(m_customer.GetMouthOpen() == false)
			{
				GameObject source = AudioManager.Instance.SpawnAudio (transform.position);
				source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_OPENV2);
			}
			
		}

		
	}

	private void OnMunching() {
		// ResetTriggers();
		m_animator.SetTrigger ("DoMunching");
		m_customer.ChangeFacialExpression(5);

		GameObject source = AudioManager.Instance.SpawnAudio (transform.position);
			source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_MUNCH);
	}

	private void OnCalmIdle() {
		// Debug.Log("Calm Idle Trigger");
		// ResetTriggers();
		if(m_customer.GetIsMunching() == false && m_customer.GetMouthOpen() == false)
		{
			// Debug.Log("DoCalmIdle Trigger 1");
			m_animator.SetTrigger ("DoCalmIdle");
			m_customer.ChangeFacialExpression(0);
		}
	}

	private void OnHappy(){
		// ResetTriggers();

		if(m_customer.GetIsMunching() == false && m_customer.GetMouthOpen() == false)
		{
			// Debug.Log("DoCalmIdle Trigger 2");
			m_animator.SetTrigger ("DoCalmIdle");
			m_customer.ChangeFacialExpression(2);
		}
	}

	private void OnImpatientIdle() {
		
		// ResetTriggers();
		if(m_customer.GetIsMunching() == false && m_customer.GetMouthOpen() == false)
		{
			// Debug.Log("ImpatientIdle Trigger");
			m_animator.SetTrigger ("DoImpatientIdle");
			m_customer.ChangeFacialExpression(1);
		}
	}

	private void OnCorrect() {
		m_animator.SetInteger("CorrectAnim", Random.Range(0,2));
		// m_animator.SetTrigger ("DoCorrect");

		GameObject source = AudioManager.Instance.SpawnAudio (transform.position);

		if(GetComponent<Customer>().GetCustomerGender() == Gender.Male)
		{
			switch(Random.Range(0,2))
			{
				case 0: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_JOY1_BOY);
				break;
				case 1: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_JOY2_BOY);
				break;
			}
		}
		else{
			switch(Random.Range(0,2))
			{
				case 0: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_JOY1_GIRL);
				break;
				case 1: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_JOY2_GIRL);
				break;
			}
		}
		

		
	}

	private void OnWrong() {
		// m_animator.SetTrigger ("DoWrong");

		m_animator.SetInteger("CorrectAnim", 2);
		// m_customer.ChangeFacialExpression(2);

		GameObject source = AudioManager.Instance.SpawnAudio (transform.position);

		switch(Random.Range(0,3))
			{
				case 0: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_MAD1);
				break;
				case 1: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_MAD2);
				break;
				case 2: source.GetComponent<AudioController> ().PlayOnce (Constants.SFX_CUSTOMER_MAD3);
				break;
			}
	}

	private void ResetTriggers()
	{
		m_animator.SetBool ("IsMoving", true);
		m_animator.SetBool ("IsEating", false);
		m_animator.ResetTrigger("DoOpenMouth");
		m_animator.ResetTrigger("DoMunching");
		m_animator.ResetTrigger("DoCalmIdle");
		m_animator.ResetTrigger("DoImpatientIdle");
		m_animator.Play("Walk");
	}

	// private void OnCorrectFace() {
	// 	m_customer.ChangeFacialExpression(2);
	// }

	// private void OnWrongFace() {
	// 	m_customer.ChangeFacialExpression(3);
	// }

}
