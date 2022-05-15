using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public abstract void Use();

	public virtual void OnFocus()
	{
		//todo
	}

	public virtual void OnFocusExit()
	{
		//todo
	}
}
