using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElevatorDoorsTriggerHandler : MonoBehaviour
{
	public event Action CharacterBetweenDoorsEntered;
	public bool CharacterBetweenDoors { get; private set; }

	[SerializeField] private string _characterTag = "Character";

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_characterTag))
		{
			CharacterBetweenDoors = true;
			CharacterBetweenDoorsEntered?.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(_characterTag))
			CharacterBetweenDoors = false;
	}
}
