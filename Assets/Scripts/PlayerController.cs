using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : MonoBehaviour
{
	private Rigidbody _rigidBody;

	[Header("Debug Only!")]
	public Vector3 rigidbodyVelocity;

    //Movement calculations 
    [Header("Basic Movement")]
	[SerializeField] float _speed;
	private Vector3 _direction;
	private Vector3 _velocity;
	[SerializeField] float _accelerationRate;
	[SerializeField] private CapsuleCollider _capsuleCollider;
	[SerializeField] private float _jumpDetectionHeight;

	private Collider _collider;
	[SerializeField] private PhysicMaterial _slopeFrictionMaterial;
	[SerializeField] private PhysicMaterial _noFrictionMaterial;

	public bool _onGround = false;
	public bool _onSlope = false;
	private Vector3 _movementNormal = Vector2.zero;

	[Header("Jumping")]
	[SerializeField] private float jumpShortSpeed = 3f;   // Velocity for the lowest jump
	[SerializeField] private float jumpSpeed = 6f;          // Velocity for the highest jump

	bool jump = false;
	bool jumpCancel = false;

	[Header("Shooting")]
	[SerializeField] float shotStrength;


	// Start is called before the first frame update
	void Start()
	{
		_rigidBody = gameObject.GetComponent<Rigidbody>();
		_rigidBody.freezeRotation = true;
		//_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
		_collider = gameObject.GetComponent<Collider>();
	}

	// Update is called once per frame
	void Update()
    {
        rigidbodyVelocity = _rigidBody.velocity;
    }
	void FixedUpdate()
	{
		// -- JUMP -- //

		//Check if player is on ground
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, _jumpDetectionHeight))
		{
			if (hit.collider.tag == "Ground" && !jump)
			{
				_onGround = true;
				_movementNormal = hit.normal;
                //print(_movementNormal);
                //print($"{Acceleration.x}, {Acceleration.y}: " +
                //$"{Mathf.Atan2(Acceleration.y, Acceleration.x)}");
                //print(Mathf.Rad2Deg * Mathf.Atan2(Acceleration.y, Acceleration.x));
            }
            else
            {
				_onGround = false;
            }
        } else
		{
			print("did not hit");
		}

		// Normal jump (full speed)
		if (jump)
		{
			_rigidBody.velocity = new Vector3(_rigidBody.velocity.x, jumpSpeed, 0);
			jump = false;
		}
		// Cancel the jump when the button is no longer pressed
		if (jumpCancel)
		{
			if (_rigidBody.velocity.y > jumpShortSpeed)
				_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpShortSpeed);
			jumpCancel = false;
		}

		// -- MOVE -- //

		Vector3 horizontalMove = _rigidBody.velocity;
		horizontalMove.x = _direction.x * _speed;
		_rigidBody.velocity = horizontalMove;

    }

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _jumpDetectionHeight);
		//print(transform.position + _velocity * 10);
		Gizmos.DrawLine(transform.position, transform.position + _velocity);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position + _direction);
		//print(_velocity);
		//Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
    }


	//private void OnCollisionEnter(Collision collision)
	//{
	//	//TODO: check if collision is ground
	//	_onGround = true;
	//		
	//	print("enter collision: " + collision.gameObject.name);
	//	//foreach (ContactPoint item in collision.contacts)
	//	//{
	//	//	Debug.DrawRay(item.point, item.normal * 100, UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
	//	//}
	//	print("ON Ground");
	//}

	//private void OnCollisionExit(Collision collision)
	//{
	//	//TODO: check if collision is ground
	//	print(collision.gameObject.name);
	//	_onGround = false;
	//	print("OFF Ground");
	//}

	public float GetAngleToMouse()
	{
		Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector3 localLookVect = mouseLocation - transform.position;
		localLookVect.Normalize();
		float angleOfRot = Mathf.Atan2(localLookVect.y, localLookVect.x);

		angleOfRot *= Mathf.Rad2Deg;
		//print(angleOfRot);
		return angleOfRot;
		//return Quaternion.Euler(0, 0, angleOfRot);
	}
}
