using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
	public bool IsMoving { get; private set; }
	public bool IsMovingUp { get; private set; }

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private ElevatorDoorsAnimator _elevatorDoorsAnimator;
	[SerializeField] private float _speed = 2f;

	private List<ElevatorFloor> _floorsToVisit;

	public void Call(ElevatorFloor elevatorFloor)
	{
		if (_floorsToVisit.Contains(elevatorFloor))
			return;

		///todo floors sorting

		_floorsToVisit.Add(elevatorFloor);
	}

	private IEnumerator MoveToNextFloor()
	{
		ElevatorFloor floorToVisit = _floorsToVisit.First();
		_floorsToVisit.RemoveAt(0);

		_elevatorDoorsAnimator.CloseDoors();

		while (!_elevatorDoorsAnimator.DoorsAreClosed)
			yield return null;

		float moveDuration = Vector3.Distance(transform.position, floorToVisit.ElevatorPosition) / _speed;
		_rigidbody.DOMove(floorToVisit.ElevatorPosition, moveDuration)
			.SetEase(Ease.InOutQuart)
			.SetUpdate(UpdateType.Fixed);
	}

	private void OnMoveComplete()
	{

	}



	private void Start()
	{
		_floorsToVisit = new List<ElevatorFloor>();
	}

	private void Update()
	{
		if (!IsMoving && _floorsToVisit.Count > 0)
		{
			StartCoroutine(MoveToNextFloor());
			IsMoving = true;
		}
	}
}
