using UnityEngine;

public class ElevatorButton : Interactable
{
	[Header("Elevator button")]
	[SerializeField] private Elevator _elevator;
	[SerializeField] private ElevatorFloor _elevatorFloor;

	public override void Use()
	{
		_elevator.Call(_elevatorFloor);
	}

	public override void OnFocus()
	{
		base.OnFocus();
	}

	public override void OnFocusExit()
	{
		base.OnFocusExit();
	}
}
