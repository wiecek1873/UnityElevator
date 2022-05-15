using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private float _playerSpeed = 2.0f;
	[SerializeField] private float _jumpHeight = 1.0f;

	private bool _characterGrounded;
	private float _groundRaycastDistance = 0.01f;
	private Vector3 _playerVelocity;

	private bool IsGrounded()
	{
		Vector3 characterBase = _characterController.transform.position + Vector3.down * _characterController.height / 2f;
		float raycastDistance = _groundRaycastDistance + _characterController.skinWidth;

		Ray ray = new Ray(characterBase, Vector3.down * raycastDistance);

		Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 0.01f);
		return Physics.Raycast(ray, raycastDistance);
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
		_characterGrounded = IsGrounded();

		if (_characterGrounded && _playerVelocity.y < 0)
			ResetVelocityOnGround();

		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		MoveHorizontally(moveDirection);

		if (Input.GetButtonDown("Jump") && _characterGrounded)
			Jump();

		HandleGravity();

		_characterController.Move(_playerVelocity * Time.deltaTime);
	}

	private void Reset()
	{
		_characterController = GetComponent<CharacterController>();
	}
}
