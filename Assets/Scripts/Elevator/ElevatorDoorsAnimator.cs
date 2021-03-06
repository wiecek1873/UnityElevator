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

	private int _animatorParameterHash;
	private float _doorsOpenness = 1;
	private Tween _activeTween;

	public void OpenDoors()
	{
		DoorsState = ElevatorDoorsState.Working;

		_doorsOpenness = _animator.GetFloat(_animatorParameterHash);
		float remainingDuration = (1f - _doorsOpenness) * _closeTime;

		_activeTween.Kill();

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 1, remainingDuration)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterHash, _doorsOpenness))
			.OnComplete(() =>DoorsState = ElevatorDoorsState.Opened);
	}

	public void TryCloseDoors()
	{
		if (!_elevatorDoorsTriggerHandler.CharacterBetweenDoors)
			CloseDoors();
	}

	public void CloseDoors()
	{
		DoorsState = ElevatorDoorsState.Working;

		_doorsOpenness = _animator.GetFloat(_animatorParameterHash);
		float remainingDuration = _doorsOpenness * _closeTime;

		_activeTween.Kill();

		_activeTween = DOTween.To(() => _doorsOpenness, x => _doorsOpenness = x, 0, remainingDuration)
			.OnUpdate(() => _animator.SetFloat(_animatorParameterHash, _doorsOpenness))
			.OnComplete(() => DoorsState = ElevatorDoorsState.Closed);
	}

	private void OnCharacterBetweenDoorsEntered()
	{
		OpenDoors();
	}

	private void Awake()
	{
		_animatorParameterHash = Animator.StringToHash(_animatorParameterName);
	}

	private void OnEnable()
	{
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsEntered += OnCharacterBetweenDoorsEntered;
	}

	private void OnDestroy()
	{
		_elevatorDoorsTriggerHandler.CharacterBetweenDoorsEntered -= OnCharacterBetweenDoorsEntered;
	}
}
