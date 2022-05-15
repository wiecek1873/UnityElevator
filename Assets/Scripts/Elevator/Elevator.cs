using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
	public bool IsMoving { get; private set; }
	public bool IsMovingUp { get; private set; }

	[SerializeField] private float _speed = 2f;

	private List<ElevatorFloor> _floorsToVisit;

	public void Call(ElevatorFloor elevatorFloor)
	{
		if (_floorsToVisit.Contains(elevatorFloor))
			return;

		///todo floors sorting

		_floorsToVisit.Add(elevatorFloor);
	}

	private void MoveToNextFloor()
	{
		ElevatorFloor floorToVisit = _floorsToVisit.First();
		_floorsToVisit.RemoveAt(0);

		float moveDuration = Vector3.Distance(transform.position, floorToVisit.ElevatorPosition) / _speed;
		transform.DOMove(floorToVisit.ElevatorPosition, moveDuration)
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
			MoveToNextFloor();
			IsMoving = true;
		}
	}
}
