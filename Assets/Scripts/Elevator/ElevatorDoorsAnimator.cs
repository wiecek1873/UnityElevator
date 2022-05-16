using UnityEngine;
using DG.Tweening;

public enum ElevatorDoorsState
{
	Opened,
	Closed,
	Working
}

public class ElevatorDoorsAnimator : MonoBehaviour
{
	public ElevatorDoorsState DoorsState { get; private set; }

	[SerializeField] private ElevatorDoorsTriggerHandler _elevatorDoorsTriggerHandler;
	[SerializeField] private Animator _animator;
	[SerializeField] private string _animatorParameterName;
	[SerializeField] private float _closeTime = 2.5f;

	private float _doorsOpenness = 1;
	private Tween _activeTween;

	public void OpenDoors()
	{
		DoorsState = ElevatorDoorsState.Working;

		_activeTween.Kill();

		_doorsOpenness = _animator.GetFloat(_animatorParameterName);

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 1, _closeTime - _doorsOpenness)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterName, _doorsOpenness))
			.OnComplete(() =>DoorsState = ElevatorDoorsState.Opened);
	}

	public void CloseDoors()
	{
		_activeTween.Kill();

		_doorsOpenness = _animator.GetFloat(_animatorParameterName);

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 0, _doorsOpenness * _closeTime)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterName, _doorsOpenness))
			.OnComplete(() => DoorsState = ElevatorDoorsState.Closed);
	}

	protected virtual void OnCharacterBetweenDoorsEntered()
	{
		OpenDoors();
	}

	protected virtual void OnCharacterBetweenDoorsExited()
	{
		//CloseDoors();
	}

	private void Start()
	{
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsEntered += OnCharacterBetweenDoorsEntered;
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsExited += OnCharacterBetweenDoorsExited;
	}
}
