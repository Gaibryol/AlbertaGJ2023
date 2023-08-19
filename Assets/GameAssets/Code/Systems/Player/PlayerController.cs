using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private PlayerControls playerControls;
	private InputAction move;
	private InputAction dash;
	private InputAction interact;
	private InputAction attack;

	private float movespeed;

	private PlayerMovement playerMovement;
	private PlayerAnimations playerAnimations;

	private void Dash(InputAction.CallbackContext context)
	{
		StartCoroutine(playerMovement.HandleDash(transform.position, Constants.Player.Movement.DashForce, Constants.Player.Movement.DashDuration));
		StartCoroutine(playerAnimations.HandleDashAnim(Constants.Player.Movement.DashDuration));
	}

	private void Interact(InputAction.CallbackContext context)
	{
		// Interact
		Debug.Log("interact");
	}

	private void Attack(InputAction.CallbackContext context)
	{
		// Attack
		Debug.Log("attack");
	}

	private void Awake()
	{
		playerControls = new PlayerControls();
		playerMovement = new PlayerMovement(GetComponent<Rigidbody2D>());
		playerAnimations = new PlayerAnimations(GetComponent<Animator>());
	}

	// Start is called before the first frame update
	private void Start()
    {
		movespeed = Constants.Player.Movement.Movespeed;
    }

    // Update is called once per frame
    private void Update()
    {
		playerMovement.HandleMovement(move.ReadValue<Vector2>(), movespeed);
		playerAnimations.HandleMovementAnim(move.ReadValue<Vector2>());
    }

	private void OnEnable()
	{
		move = playerControls.Player.Move;
		move.Enable();

		dash = playerControls.Player.Dash;
		dash.Enable();
		dash.performed += Dash;

		interact = playerControls.Player.Interact;
		interact.Enable();
		interact.performed += Interact;

		attack = playerControls.Player.Attack;
		attack.Enable();
		attack.performed += Attack;
	}

	private void OnDisable()
	{
		move.Disable();
		dash.Disable();
		interact.Disable();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AttackTrigger")
		{
			// Munis helth
		}
    }
}
