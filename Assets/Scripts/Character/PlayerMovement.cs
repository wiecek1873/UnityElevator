using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _gravity = -9.81f;
	[SerializeField] private float _speed = 10f;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private float _jumpHeight = 1f;
	[SerializeField] private float _groundDistance = 0.4f;

	private Vector3 _velocity;
	private bool _isGrounded;

	private bool IsGrounded()
	{
		return Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
	}

	private void ApplyGravity()
	{
		_velocity.y += _gravity * Time.deltaTime;
	}

	private void ResetGravityOnGround()
	{
		_velocity.y = -2f;
	}

	private void Jump()
	{
		_velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
	}
	private Vector3 GetMoveDirection()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		return transform.right * x + transform.forward * z;
	}
	
	void Update()
	{
		_isGrounded = IsGrounded();

		if (_isGrounded && _velocity.y < 0)
			ResetGravityOnGround();

		_characterController.Move(_speed * Time.deltaTime * GetMoveDirection());

		if (Input.GetButtonDown("Jump") && _isGrounded)
			Jump();

		ApplyGravity();

		_characterController.Move(_velocity * Time.deltaTime);
	}
}
