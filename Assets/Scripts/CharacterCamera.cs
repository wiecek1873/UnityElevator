using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
	[SerializeField] private Transform _camera;
	[SerializeField] [Range(0.1f, 5f)] private float _verticalSensitivity = 1f;
	[SerializeField] private Vector2 _rotationBounds = new Vector2(-90f, 50f);

	//todo Limit rotation
	private void Rotate(float angle)
	{
		_camera.transform.Rotate(Vector3.right, angle, Space.Self);
	}

	void Update()
	{
		float rotationAngle = -Input.GetAxis("Mouse Y") * _verticalSensitivity;
		Rotate(rotationAngle);
	}
}
