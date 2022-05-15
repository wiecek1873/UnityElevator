using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRaycaster : MonoBehaviour
{
	public Interactable CurrentFocus { get; private set; }

	[SerializeField] private Transform _origin;
	[SerializeField] private LayerMask _targetLayers;
	[SerializeField] private float _maxDistance = 5f;

	private void OnRaycastHit(RaycastHit raycastHit)
	{
		if (CurrentFocus != null && CurrentFocus.gameObject == raycastHit.collider.gameObject)
			return;

		if (CurrentFocus != null)
			CurrentFocus.OnFocusExit();

		CurrentFocus = raycastHit.collider.GetComponent<Interactable>();
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
		Ray ray = new Ray(_origin.position, _origin.forward);

		if (Physics.Raycast(ray, out RaycastHit raycastHit, _maxDistance, _targetLayers))
			OnRaycastHit(raycastHit);
		else
			OnRaycastMiss();

		if (CurrentFocus != null && Input.GetMouseButtonUp(0))
			CurrentFocus.Use();
	}
}
