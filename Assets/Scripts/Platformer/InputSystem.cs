using UnityEngine;

namespace Platformer
{
	public class InputSystem : MonoBehaviour
	{
		public static float HorizontalRaw()
		{
			return Input.GetAxisRaw("Horizontal");
		}

		public static bool Jump()
		{
			return Input.GetButtonDown("Jump");
		}

	}
}
