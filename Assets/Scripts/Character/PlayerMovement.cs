using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _gravity = -9.81f;
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _jumpHeight = 1f;
	[Header("Ground")]
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private float _groundDistance = 0.4f;
	[Header("Slopes")]
	[SerializeField] private float _slopeRaycastDistance = 0.5f;
	[Header("Platforms")]
	[SerializeField] private LayerMask _platformMask;
	[SerializeField] private float _platformRaycastDistance = 0.4f;

	private Vector3 _velocity;
	private bool _isGrounded;
	private bool _isOnPlatform;
	private Transform _previousPlatform;
	private Vector3 _previousPlatformPosition;

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

	private bool IsOnPlatform()
	{
		return Physics.CheckSphere(_groundCheck.position, _platformRaycastDistance, _platformMask);
	}

	private void CopyPlatformMove()
	{
		Ray ray = new Ray(_groundCheck.position, Vector3.down);

		if (!Physics.Raycast(ray, out RaycastHit raycastHit, _platformRaycastDistance))
		{
			_previousPlatform = null;
			_previousPlatformPosition = Vector3.zero;
			return;
		}

		if (_previousPlatform == null)
		{
			_previousPlatform = raycastHit.collider.transform;
			_previousPlatformPosition = _previousPlatform.position;
		}
		else
		{
			if (_previousPlatform == raycastHit.collider.transform)
			{
				_characterController.Move(_previousPlatform.position - _previousPlatformPosition);
				_previousPlatformPosition = _previousPlatform.position;
			}
			else
			{
				_previousPlatform = raycastHit.collider.transform;
				_previousPlatformPosition = _previousPlatform.position;
			}
		}
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
		_isOnPlatform = IsOnPlatform();

		bool isOnSurface = _isGrounded || _isOnPlatform;

		if (_isOnPlatform)
			CopyPlatformMove();

		if (isOnSurface && _velocity.y < 0)
			ResetGravityOnGround();

		Vector3 moveDirection = GetMoveDirection();
		moveDirection = ChangeDirectionOnSlope(moveDirection);

		_characterController.Move(_speed * Time.deltaTime * moveDirection);

		if (Input.GetButtonDown("Jump") && isOnSurface)
			Jump();

		ApplyGravity();

		_characterController.Move(_velocity * Time.deltaTime);
	}
}
