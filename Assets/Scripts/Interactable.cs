using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[Header("Interactable")]
	[SerializeField] private QuickOutline _quickOutline;
	[SerializeField] private Color _focusColor = Color.cyan;
	[SerializeField] private Color _mouseButtonDown = Color.green;
	[SerializeField] private AudioSource _mouseButtonUpSound;

	public abstract void Use();

	public virtual void OnMouseButtonDown()
	{
		_quickOutline.OutlineColor = _mouseButtonDown;

	}

	public virtual void OnMouseButtonUp()
	{
		_quickOutline.OutlineColor = _focusColor;
		Use();

		if (_mouseButtonUpSound != null)
			_mouseButtonUpSound.Play();
	}

	public virtual void OnFocus()
	{
		_quickOutline.enabled = true;
		_quickOutline.OutlineColor = _focusColor;
	}

	public virtual void OnFocusExit()
	{
		_quickOutline.enabled = false;
	}
}
