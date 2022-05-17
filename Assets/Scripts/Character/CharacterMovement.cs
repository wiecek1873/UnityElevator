using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private float _playerSpeed = 2.0f;
	[SerializeField] private float _jumpHeight = 1.0f;
	[SerializeField] private float _gravity = -50f;
	[SerializeField] private float _slopeRaycastDistance = 1f;
	[SerializeField] private float _slopeForce = 10f;

	private Vector3 _playerVelocity;

	private bool IsGrounded()
	{
		return _characterController.isGrounded;
	}

	private bool OnSlope()
	{
		Vector3 characterBase = _characterController.transform.position + Vector3.down * _characterController.height / 2f;
		float raycastDistance = _slopeRaycastDistance + _characterController.skinWidth;

		Ray ray = new Ray(characterBase, Vector3.down * raycastDistance);

		if (Physics.Raycast(ray, out RaycastHit raycastHit, raycastDistance))
			return raycastHit.normal != Vector3.up;
		return false;
	}

	private void MoveHorizontally(Vector3 direction)
	{
		_characterController.Move(Time.deltaTime * _playerSpeed * direction.z * _characterController.transform.forward);
		_characterController.Move(Time.deltaTime * _playerSpeed * direction.x * _characterController.transform.right);
	}

	private void ResetVelocityOnGround()
	{
		_playerVelocity.y = 0f;
	}

	private void Jump()
	{
		_playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * Physics.gravity.y);
	}

	private void HandleGravity()
	{
		_playerVelocity.y += Physics.gravity.y * Time.deltaTime;
	}

	private void Update()
	{
		bool characterGrounded = IsGrounded();

		if (characterGrounded && _playerVelocity.y < 0)
			ResetVelocityOnGround();

		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		MoveHorizontally(moveDirection);

		if (Input.GetButtonDown("Jump") && characterGrounded)
			Jump();

		if (OnSlope() && _playerVelocity.y >= 0)
			_characterController.Move(Vector3.down * Time.deltaTime * _slopeForce);

		HandleGravity();

		_characterController.Move(_playerVelocity * Time.deltaTime);
	}

	private void Reset()
	{
		_characterController = GetComponent<CharacterController>();
	}
}
