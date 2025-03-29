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

	//Movement calculations 
	[Header("Basic Movement")]
	[SerializeField][Range(0, 100)] float _maxSpeed;
	[SerializeField] float _xDrag;
	private Vector3 _direction;
	private Vector3 _velocity;
	private Vector3 _acceleration;
	[SerializeField] float _accelerationRate;
	[SerializeField] private CapsuleCollider _capsuleCollider;
	[SerializeField] private float _jumpDetectionHeight;

	public bool _onGround = false;
	private Vector3 _movementNormal = Vector2.zero;

	[Header("Jumping")]
	[SerializeField] private float jumpShortSpeed = 3f;   // Velocity for the lowest jump
	[SerializeField] private float jumpSpeed = 6f;          // Velocity for the highest jump

	bool jump = false;
	bool jumpCancel = false;

	[Header("Shooting")]
	[SerializeField] float shotStrength;

	public Vector3 Acceleration { get { return _acceleration; } }

	// Start is called before the first frame update
	void Start()
	{
		_rigidBody = gameObject.GetComponent<Rigidbody>();
		_rigidBody.freezeRotation = true;
		//_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
	}

	// Update is called once per frame
	void Update()
    {
        print($"{Acceleration.x}, {Acceleration.y}: " +
            $"{Mathf.Atan2(Acceleration.y, Acceleration.x)}");
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

		Vector3 directionFromSlope = _direction;

        //slope whose normal is facing up-left
        if (_movementNormal.x != 0 || !jump)
        {
            directionFromSlope = Quaternion.AngleAxis(-90, Vector3.forward) * _movementNormal * _direction.x;
        }

        _acceleration = Vector3.zero;
		_acceleration = (directionFromSlope * _accelerationRate * Time.fixedDeltaTime);

		_rigidBody.velocity += _acceleration;

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

	/*
    private void HandleMovement()
    {
		// Movement
		_acceleration = _direction * 10;
        _velocity.x = Mathf.Clamp(_velocity.x + _acceleration.x * Time.deltaTime, -_maxSpeed, _maxSpeed);
        _velocity.y = Mathf.Clamp(_velocity.y + _acceleration.y * Time.deltaTime, -_maxSpeed, _maxSpeed);
        //_velocity.x += (_acceleration.x * Time.deltaTime);

        //Ground friction
        if (_velocity.x > 0)
        {
			//apply xdrag
			//_velocity.x = Mathf.Clamp(_velocity.x - (_xDrag / 100000 * Time.deltaTime), 0, float.MaxValue);
			_velocity.x = _velocity.x - (_xDrag / 100000 * Time.deltaTime);
        }
        else if (_velocity.x < 0)
        {
			//_velocity.x = Mathf.Clamp(_velocity.x + (_xDrag / 100000 * Time.deltaTime), float.MinValue, 0);
			_velocity.x = _velocity.x + (_xDrag / 100000 * Time.deltaTime);
        }

        //Move the player
        _rigidBody.MovePosition(_rigidBody.position + (_velocity * Time.deltaTime));
    }
	*/

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
