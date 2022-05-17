using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[Range(10f, 500)] [SerializeField] private float _mouseSensitivity = 250f;
	[SerializeField] private Transform _playerModel;
	[SerializeField] private Transform _camera;
	[SerializeField] private Vector2 _verticalRotationBounds = new Vector2(-90f, 90f);

	private float xRotation = 0f;

	void Update()
	{
		float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, _verticalRotationBounds.x, _verticalRotationBounds.y);

		_camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		_playerModel.Rotate(Vector3.up * mouseX);
	}
}
