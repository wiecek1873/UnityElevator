using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorsHandler : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private string _characterTag = "Character";

	private bool _characterBetweenDoors;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == _characterTag)
			_characterBetweenDoors = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == _characterTag)
			_characterBetweenDoors = false;
	}
}
