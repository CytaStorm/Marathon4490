using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : MonoBehaviour
{
	public void Move(InputAction.CallbackContext ctx)
	{
		//print("here");
		if (ctx.performed)
		{
			float input = ctx.ReadValue<float>();

			//slope whose normal is facing up-left
			if (_movementNormal.x != 0 || !jump)
			{
				_direction = Quaternion.AngleAxis(-90, Vector3.forward) * _movementNormal * input;
			}
			else
			{
				_direction = new Vector3(input, 0, 0);
			}
		}
		else if (ctx.canceled)
		{
			_direction = Vector3.zero;
		}
				
	}

	public void Fire(InputAction.CallbackContext ctx)
	{
		Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 forceDirection = transform.position - mouseLocation;
		forceDirection.z = 0;
		forceDirection.Normalize();

		// Multiply direction vector by force scalar
		forceDirection *= shotStrength;
		print(forceDirection);
		_rigidBody.AddForce(forceDirection);
	}

	public void Jump(InputAction.CallbackContext ctx)
	{
		if ((ctx.performed) && (_onGround))
		{
			Debug.Log("JUMP");
			_onGround = false;
			jump = true;
		}
		else if (ctx.canceled)
		{
			Debug.Log("JUMP off");
			jumpCancel = true;
		}
	}

	public void Slide(InputAction.CallbackContext ctx)
	{
		//TODO: If on ground, begin sliding
		//If in air, hold slide to start sliding on ground with no loss of momentum
	}

}
