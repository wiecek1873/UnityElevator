using UnityEngine;
using DG.Tweening;

public class ElevatorDoorsAnimator : MonoBehaviour
{
	[SerializeField] private ElevatorDoorsTriggerHandler _elevatorDoorsTriggerHandler;
	[SerializeField] private Animator _animator;
	[SerializeField] private string _animatorParameterName;

	private float _doorsOpenness = 1;
	private Tween _activeTween;

	protected virtual void OnCharacterBetweenDoorsEntered()
	{
		OpenDoors();
	}

	protected virtual void OnCharacterBetweenDoorsExited()
	{
		//CloseDoors();
	}

	private void OpenDoors()
	{
		_activeTween.Kill();

		_doorsOpenness = _animator.GetFloat(_animatorParameterName);

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 1, 1 - _doorsOpenness)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterName, _doorsOpenness));
	}

	private void CloseDoors()
	{
		_activeTween.Kill();

		_doorsOpenness = _animator.GetFloat(_animatorParameterName);

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 0, _doorsOpenness)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterName, _doorsOpenness));
	}

	private void Start()
	{
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsEntered += OnCharacterBetweenDoorsEntered;
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsExited += OnCharacterBetweenDoorsExited;
	}
}
