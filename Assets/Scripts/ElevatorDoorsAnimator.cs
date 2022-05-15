using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorsAnimator : MonoBehaviour
{
	[SerializeField] private ElevatorDoorsHandler _elevatorDoorsHandler;
	[SerializeField] private Animator _animator;
	[SerializeField] private string _animatorParameterName;

	private float _doorsOpenness;

	protected virtual void OnCharacterBetweenDoorsEntered()
	{

	}

	protected virtual void OnCharacterBetweenDoorsExited()
	{

	}

	private void Start()
	{
		_elevatorDoorsHandler.CharacterBetweenDoorsEntered += OnCharacterBetweenDoorsEntered;
		_elevatorDoorsHandler.CharacterBetweenDoorsExited += OnCharacterBetweenDoorsExited;
	}

	private void Update()
	{
		_animator.SetFloat(_animatorParameterName, _doorsOpenness);
	}
}
