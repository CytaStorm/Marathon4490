using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	private Rigidbody _rigidBody;

	//Movement calculations 
	[Header("Basic Movement")]
	[SerializeField][Range(0, 100)] float _maxSpeed;
	[SerializeField] float _xDrag;
	private Vector3 _direction;
	private Vector3 _velocity;
	private Vector2 _acceleration;


	[Header("Jumping")]
	[SerializeField] private float jumpShortSpeed = 3f;   // Velocity for the lowest jump
	[SerializeField] private float jumpSpeed = 6f;          // Velocity for the highest jump

	bool jump = false;
	bool jumpCancel = false;

	[Header("Shooting")]
	[SerializeField] float shotStrength;

	private bool _onGround = false;

	// Start is called before the first frame update
	void Start()
	{
		_rigidBody = gameObject.GetComponent<Rigidbody>();
		_rigidBody.freezeRotation = true;
	}

	// Update is called once per frame
	void Update()
    {
        HandleMovement();
    }
	void FixedUpdate()
	{
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
	}

    private void HandleMovement()
    {
        // Movement
        _acceleration.x = _direction.x / 100 * Time.deltaTime;
        _velocity.x = Mathf.Clamp(_velocity.x + _acceleration.x * Time.deltaTime, -_maxSpeed, _maxSpeed);
        _velocity.x += (_acceleration.x * Time.deltaTime);

        //Ground friction
        if (_velocity.x > 0)
        {
            //apply xdrag
            _velocity.x = Mathf.Clamp(_velocity.x - (_xDrag / 100000 * Time.deltaTime), 0, float.MaxValue);
        }
        else if (_velocity.x < 0)
        {
            _velocity.x = Mathf.Clamp(_velocity.x + (_xDrag / 100000 * Time.deltaTime), float.MinValue, 0);
        }

        //Move the player
        _rigidBody.MovePosition(_rigidBody.position + (_velocity / Time.deltaTime));
    }

	private void OnCollisionEnter(Collision collision)
	{
		//TODO: check if collision is ground
		_onGround = true;
			
		//foreach (ContactPoint item in collision.contacts)
		//{
		//	Debug.DrawRay(item.point, item.normal * 100, UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
		//}
		print("ON Ground");
	}

	private void OnCollisionExit(Collision collision)
	{
		//TODO: check if collision is ground
		_onGround = false;
		print("OFF Ground");
	}

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
