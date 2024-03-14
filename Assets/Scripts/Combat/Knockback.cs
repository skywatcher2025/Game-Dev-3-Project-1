using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;
    
	[SerializeField] float _knockbackTime = .2f;

	private Vector3 _hitdirection;
	private float _knockbackThrust;

	private Rigidbody2D _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		OnKnockbackStart += ApplyKnockbackThrust;	//Creates listener
		OnKnockbackEnd += StopKnockRoutine;			//Creates listener
	}

	private void OnDisable()
	{
		OnKnockbackStart -= ApplyKnockbackThrust;	//Removes listener
		OnKnockbackEnd -= StopKnockRoutine;			//Removes listener
	}

	public void GetKnockedBack(Vector3 hitdirection, float knockbackThrust)
	{
		_hitdirection = hitdirection;
		_knockbackThrust = knockbackThrust;
		
		OnKnockbackStart?.Invoke();
	}

	void ApplyKnockbackThrust()
	{
		Vector3 forceVector = (transform.position - _hitdirection).normalized * _knockbackThrust * _rigidbody.mass;
		_rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
		StartCoroutine(KnockRoutine());
	}

	IEnumerator KnockRoutine()
	{
		yield return new WaitForSeconds(_knockbackTime);
		OnKnockbackEnd?.Invoke();
	}

	void StopKnockRoutine()
	{
		_rigidbody.velocity = Vector2.zero;
	}
}
