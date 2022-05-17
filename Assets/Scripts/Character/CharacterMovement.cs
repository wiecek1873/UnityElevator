using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private float _playerSpeed = 2.0f;
	[SerializeField] private float _jumpHeight = 1.0f;
	[SerializeField] private float _groundRaycastDistance = 0.5f;
	[SerializeField] private float _slopeRaycastDistance = 0.35f;
	[SerializeField] private float _slopeForce = 10f;
	[SerializeField] private float _platformRaycastDistance = 0.5f;

	private Vector3 _playerVelocity;
	private GameObject _platformLastFrame;
	private Vector3 _platformPositionLastFrame;

	private bool OnGround()
	{
		if(_characterController.isGrounded)
			return true;

		Vector3 characterBase = CharacterBase();
		float raycastDistance = _groundRaycastDistance + _characterController.skinWidth;

		Ray ray = new Ray(characterBase, Vector3.down * raycastDistance);

		return Physics.Raycast(ray, out RaycastHit raycastHit, raycastDistance);
	}

	private bool OnSlope()
	{
		Vector3 characterBase = CharacterBase();
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

	private Vector3 CharacterBase()
	{
		return _characterController.transform.position + Vector3.down * _characterController.height / 2f;
	}

	private void MoveWithPlatform()
	{
		Vector3 characterBase = CharacterBase();
		float raycastDistance = _platformRaycastDistance + _characterController.skinWidth;

		Ray ray = new Ray(characterBase, Vector3.down * raycastDistance);

		if(Physics.Raycast(ray, out RaycastHit raycastHit, raycastDistance))
		{
			//todo Refactor this into readable
			if(_platformLastFrame != null)
			{
				if(_platformLastFrame == raycastHit.collider.gameObject)
				{
					_characterController.Move(_platformLastFrame.transform.position - _platformPositionLastFrame);
					_platformPositionLastFrame = _platformLastFrame.transform.position;
				}
				else
				{
					_platformLastFrame = raycastHit.collider.gameObject;
					_platformPositionLastFrame = _platformLastFrame.transform.position;
				}
			}
			else
			{
				_platformLastFrame = raycastHit.collider.gameObject;
				_platformPositionLastFrame = _platformLastFrame.transform.position;
			}
		}
	}

	private void Update()
	{
		bool onGround = OnGround();

		Debug.Log(onGround);
		
		if (onGround && _playerVelocity.y < 0)
			ResetVelocityOnGround();

		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		MoveHorizontally(moveDirection);

		if (Input.GetButtonDown("Jump") && onGround)
			Jump();

		if (OnSlope() && _playerVelocity.y >= 0)
			_characterController.Move(Vector3.down * Time.deltaTime * _slopeForce);

		MoveWithPlatform();

		HandleGravity();

		_characterController.Move(_playerVelocity * Time.deltaTime);
	}

	private void Reset()
	{
		_characterController = GetComponent<CharacterController>();
	}
}
