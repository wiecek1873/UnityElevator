using UnityEngine;

public class CursorManager : MonoBehaviour
{
	[SerializeField] private bool _hideCursorOnStart;

	private void Start()
	{
		if (_hideCursorOnStart)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}
