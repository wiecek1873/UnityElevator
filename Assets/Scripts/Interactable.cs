using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public abstract void Use();

	public virtual void OnFocus()
	{

	}

	public virtual void OnFocusExit()
	{

	}
}
