using UnityEngine;

public class CharacterRaycaster : MonoBehaviour
{
	public Interactable CurrentFocus { get; private set; }

	[SerializeField] private Transform _origin;
	[SerializeField] private LayerMask _targetLayers;
	[SerializeField] private float _maxDistance = 5f;

	private Ray _forwardRay;
	private RaycastHit _raycastHit;

	private void OnRaycastHit()
	{
		if (CurrentFocus != null)
		{
			if (CurrentFocus.gameObject == _raycastHit.collider.gameObject)
				return;
			else
				CurrentFocus.OnFocusExit();
		}

		CurrentFocus = _raycastHit.collider.GetComponent<Interactable>();
		CurrentFocus.OnFocus();
	}

	private void OnRaycastMiss()
	{
		if (CurrentFocus != null)
			CurrentFocus.OnFocusExit();

		CurrentFocus = null;
	}

	private void Update()
	{
		_forwardRay = new Ray(_origin.position, _origin.forward);

		if (Physics.Raycast(_forwardRay, out _raycastHit, _maxDistance, _targetLayers))
			OnRaycastHit();
		else
			OnRaycastMiss();

		if (CurrentFocus == null)
			return;

		if (Input.GetMouseButton(0))
			CurrentFocus.OnMouseButtonDown();

		if (Input.GetMouseButtonUp(0))
			CurrentFocus.OnMouseButtonUp();
	}
}
