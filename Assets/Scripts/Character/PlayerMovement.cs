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
	[Header("Slopes")]
	[SerializeField] private float _slopeRaycastDistance = 0.5f;

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

	private Vector3 ChangeDirectionOnSlope(Vector3 moveDirection)
	{
		Ray ray = new Ray(_groundCheck.position, Vector3.down);

		if (!Physics.Raycast(ray, out RaycastHit raycastHit, _slopeRaycastDistance))
			return moveDirection;

		if (raycastHit.normal == Vector3.up)
			return moveDirection;

		Vector3 leftPerpendicularVector = Vector3.Cross(moveDirection, raycastHit.normal);
		return Quaternion.AngleAxis(90f, raycastHit.normal) * leftPerpendicularVector;
	}

	void Update()
	{
		_isGrounded = IsGrounded();

		if (_isGrounded && _velocity.y < 0)
			ResetGravityOnGround();

		Vector3 moveDirection = GetMoveDirection();
		moveDirection = ChangeDirectionOnSlope(moveDirection);

		_characterController.Move(_speed * Time.deltaTime * moveDirection);

		if (Input.GetButtonDown("Jump") && _isGrounded)
			Jump();

		ApplyGravity();

		_characterController.Move(_velocity * Time.deltaTime);
	}
}
