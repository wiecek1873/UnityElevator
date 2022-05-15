using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] [Range(0.1f, 5f)] private float _horizontalSensitivity = 1;

	private void Rotate(float angle)
	{
		_characterController.transform.Rotate(Vector3.up, angle, Space.Self);
	}

	void Update()
	{
		float rotationAngle = Input.GetAxis("Mouse X") * _horizontalSensitivity;
		Rotate(rotationAngle);
	}

	private void Reset()
	{
		_characterController = GetComponent<CharacterController>();
	}
}
