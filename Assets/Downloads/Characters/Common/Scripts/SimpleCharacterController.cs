using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class SimpleCharacterController : MonoBehaviour
{

	[SerializeField] float _moveSpeed = 70;
	[SerializeField] float _movementSmoothing = 0.1f;
	[SerializeField] bool _normalizedMovement = true;
	[SerializeField] GameObject _upObject;
	[SerializeField] GameObject _leftObject;
	[SerializeField] GameObject _rightObject;
	[SerializeField] GameObject _downObject;


	enum Direction { Up, Right, Down, Left };
	enum Expression { Neutral, Angry, Smile, Surprised };

	private Direction _currentDirection;
	private Direction _previousDirection;
	private float _angle = 180;
	private float _speed;
	private Vector2 _axisVector = Vector2.zero;
	private Vector3 _currentVelocity = Vector3.zero;

	Rigidbody2D _rb2D;
	Animator _currentAnimator;
	Animator downAnimator;
	Animator upAnimator;
	Animator rightAnimator;
	Animator leftAnimator;

	void Start()
	{
		_rb2D = GetComponent<Rigidbody2D>();

		_upObject.SetActive(false);
		_leftObject.SetActive(false);
		_rightObject.SetActive(false);
		_downObject.SetActive(true);

		downAnimator = _downObject.GetComponent<Animator>();
		upAnimator = _upObject.GetComponent<Animator>();
		rightAnimator = _rightObject.GetComponent<Animator>();
		leftAnimator = _leftObject.GetComponent<Animator>();

		_currentAnimator = downAnimator;
	}

	void Update()
	{

		// get speed from the rigid body to be used for animator parameter Speed
		_speed = _rb2D.velocity.magnitude;

		// Get input axises
		_axisVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//normalize it for good topdown diagonal movement
		if (_normalizedMovement == true)
		{
			_axisVector.Normalize();
		}

		// Find out which direction to face and do what is appropiate

		// Only update angle of direction if input axises are pressed
		if (!(_axisVector.x == 0 && _axisVector.y == 0))
		{
			// Find out what direction angle based on input axises
			_angle = Mathf.Atan2(_axisVector.x, _axisVector.y) * Mathf.Rad2Deg;

			// Round out to prevent jittery direction changes.
			_angle = Mathf.RoundToInt(_angle);
		}


		if (_angle > -45 && _angle < 45)  // UP
		{
			_currentDirection = Direction.Up;
		}

		else if (_angle < -135 || _angle > 135) // DOWN
		{
			_currentDirection = Direction.Down;
		}

		else if (_angle >= 45 && _angle <= 135) // RIGHT
		{
			_currentDirection = Direction.Right;
		}

		else if (_angle <= -45 && _angle >= -135)  // LEFT
		{
			_currentDirection = Direction.Left;
		}

		// Did direction change?
		if (_previousDirection != _currentDirection)
        {

            switch (_currentDirection)
            {
					case Direction.Up:
					// Activate appropiate game object
					_upObject.SetActive(true);
					_rightObject.SetActive(false);
					_leftObject.SetActive(false);
					_downObject.SetActive(false);

					_currentAnimator = upAnimator;
					break;

					case Direction.Down:
					// Activate appropiate game object
					_upObject.SetActive(false);
					_rightObject.SetActive(false);
					_leftObject.SetActive(false);
					_downObject.SetActive(true);

					_currentAnimator = downAnimator;
					break;

					case Direction.Left:
					// Activate appropiate game object
					_upObject.SetActive(false);
					_rightObject.SetActive(false);
					_leftObject.SetActive(true);
					_downObject.SetActive(false);

					_currentAnimator = leftAnimator;
					break;

					case Direction.Right:
					// Activate appropiate game object
					_upObject.SetActive(false);
					_rightObject.SetActive(true);
					_leftObject.SetActive(false);
					_downObject.SetActive(false);

					_currentAnimator = rightAnimator;
					break;
			}

		}

		// Set speed parameter to the animator
		_currentAnimator.SetFloat("Speed", _speed);

		// Set current direction as previous
		_previousDirection = _currentDirection;


		// Check keys for actions and use appropiate function
		//
		if (Input.GetKey(KeyCode.Space))  // SWING ATTACK
		{
			PlayAnimation("Swing");
		}

		else if (Input.GetKey(KeyCode.C))  // THRUST ATTACK
		{
			PlayAnimation("Thrust");
		}

		else if (Input.GetKey(KeyCode.X))  // BOW ATTACK
		{
			PlayAnimation("Bow");
		}

		else if (Input.GetKey(KeyCode.V))  // SET NEUTRAL FACE
		{
			SetExpression(Expression.Neutral);
		}

		else if (Input.GetKey(KeyCode.B))  // SET ANGRY FACE
		{
			SetExpression(Expression.Angry);
		}
	}

	void FixedUpdate()
	{
		// Move our character
		Move();
	}

	void PlayAnimation(string animationName)
	{
		// Play given animation in the current directions animator
		_currentAnimator.Play(animationName, 0);
	}

	void SetExpression(Expression expressionToSet)
	{
		// convert enum to int for the animator paremeter.
		int expressionNumber = (int)expressionToSet;

		// If the current direction is not up change expression (Up direction doesn't show any expressions)
		if (!(_currentDirection == Direction.Up)) // UP
		{
			_currentAnimator.SetInteger("Expression", expressionNumber);
		}

	}

	void Move()
	{
		// Set target velocity to smooth towards
		Vector2 targetVelocity = new Vector2(_axisVector.x * _moveSpeed * 10f, _axisVector.y * _moveSpeed * 10) * Time.fixedDeltaTime;

		// Smoothing out the movement
		_rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
	}
}
