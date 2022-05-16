using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ElevatorDirection
{
	Up,
	Down,
	Idle
}

public class Elevator : MonoBehaviour
{
	//public event Action<ElevatorFloor> OnElevatorArrival();

	[HideInInspector] public ElevatorDirection ElevatorDirection { get; private set; }
	[HideInInspector] public int CurrentFloorNumber { get; private set; }

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private ElevatorDoorsAnimator _elevatorDoorsAnimator;
	[SerializeField] private float _speed = 2f;
	[SerializeField] private Ease _moveEase = Ease.Linear;
	[SerializeField] private float _waitTime = 1f;

	private List<ElevatorFloor> _floorsToVisit;

	public void Call(ElevatorFloor floorToVisit)
	{
		TryToAddVisit(floorToVisit);

		if (_floorsToVisit.Count > 0 && ElevatorDirection == ElevatorDirection.Idle)
			StartCoroutine(MoveToNextFloor());
	}

	private IEnumerator MoveToNextFloor()
	{
		ElevatorFloor floorToVisit = _floorsToVisit.First();

		SetElevatorDirection(floorToVisit);

		yield return WaitForDoorsClose();

		float moveDuration = Vector3.Distance(transform.position, floorToVisit.ElevatorPosition) / _speed;

		_rigidbody.DOMove(floorToVisit.ElevatorPosition, moveDuration)
			.SetEase(_moveEase)
			.SetUpdate(UpdateType.Fixed)
			.OnComplete(() => StartCoroutine(OnMoveComplete(floorToVisit)));
	}

	private IEnumerator OnMoveComplete(ElevatorFloor floor)
	{
		_floorsToVisit.Remove(floor);
		CurrentFloorNumber = floor.FloorNumber;

		yield return WaitForDoorsOpen();

		ElevatorDirection = ElevatorDirection.Idle;

		yield return new WaitForSeconds(_waitTime);

		if (_floorsToVisit.Count > 0)
			StartCoroutine(MoveToNextFloor());
	}

	private IEnumerator WaitForDoorsClose()
	{
		_elevatorDoorsAnimator.TryCloseDoors();
		while (_elevatorDoorsAnimator.DoorsState != ElevatorDoorsState.Closed)
		{
			if (_elevatorDoorsAnimator.DoorsState != ElevatorDoorsState.Working)
			{
				yield return new WaitForSeconds(_waitTime);
				_elevatorDoorsAnimator.TryCloseDoors();
			}
			yield return null;
		}
	}

	private IEnumerator WaitForDoorsOpen()
	{
		_elevatorDoorsAnimator.OpenDoors();
		while (_elevatorDoorsAnimator.DoorsState != ElevatorDoorsState.Opened)
		{
			if (_elevatorDoorsAnimator.DoorsState != ElevatorDoorsState.Working)
			{
				yield return new WaitForSeconds(_waitTime);
				_elevatorDoorsAnimator.OpenDoors();
			}
			yield return null;
		}
	}

	private void TryToAddVisit(ElevatorFloor element)
	{
		if (!_floorsToVisit.Contains(element) && element.FloorNumber != CurrentFloorNumber)
			AddToVisit(element);
	}

	private void AddToVisit(ElevatorFloor element)
	{
		_floorsToVisit.Add(element);
		SortToVisit();
	}

	private void SortToVisit()
	{

	}

	private void SetElevatorDirection(ElevatorFloor floorToVisit)
	{
		if (floorToVisit.FloorNumber > CurrentFloorNumber)
			ElevatorDirection = ElevatorDirection.Up;
		else if (floorToVisit.FloorNumber < CurrentFloorNumber)
			ElevatorDirection = ElevatorDirection.Down;
	}

	private void Start()
	{
		_floorsToVisit = new List<ElevatorFloor>();
		ElevatorDirection = ElevatorDirection.Idle;
	}
}
