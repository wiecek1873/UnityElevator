using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElevatorDoorsTriggerHandler : MonoBehaviour
{
	public event Action CharacterBetweenDoorsEntered;
	public event Action CharacterBetweenDoorsExited;

	[SerializeField] private string _characterTag = "Character";

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == _characterTag)
			CharacterBetweenDoorsEntered?.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == _characterTag)
			CharacterBetweenDoorsExited?.Invoke();
	}
}
