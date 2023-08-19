using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private PlayerControls playerControls;
	private InputAction move;
	private InputAction dash;
	private InputAction attack;
	private InputAction dashAttack;
	private InputAction specialAttack;

	private float movespeed;

	private PlayerMovement playerMovement;
	private PlayerAnimations playerAnimations;
	private PlayerAttacks playerAttacks;

	private void Dash(InputAction.CallbackContext context)
	{
		StartCoroutine(playerMovement.HandleDash(transform.position, Constants.Player.Movement.DashForce, Constants.Player.Movement.DashDuration));
		StartCoroutine(playerAnimations.HandleDashAnim(Constants.Player.Movement.DashDuration));
	}

	private void Attack(InputAction.CallbackContext context)
	{
		playerAttacks.HandleAttack(transform);
		StartCoroutine(playerAnimations.HandleAttackAnim());
	}

	private void DashAttack(InputAction.CallbackContext context)
	{
		StartCoroutine(playerMovement.HandleDashAttack(transform.position, Constants.Player.Movement.DashAttackForce, Constants.Player.Movement.DashAttackDuration));
		StartCoroutine(playerAnimations.HandleDashAttackAnim(Constants.Player.Movement.DashAttackDuration));
	}

	private void SpecialAttack(InputAction.CallbackContext context)
	{
		playerAttacks.HandleSpecialAttack(transform);
		StartCoroutine(playerAnimations.HandleSpecialAttackAnim());
	}

	private void Awake()
	{
		playerControls = new PlayerControls();
		playerMovement = new PlayerMovement(GetComponent<Rigidbody2D>());
		playerAnimations = GetComponent<PlayerAnimations>();
		playerAttacks = GetComponent<PlayerAttacks>();
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

		attack = playerControls.Player.Attack;
		attack.Enable();
		attack.performed += Attack;

		dashAttack = playerControls.Player.DashAttack;
		dashAttack.Enable();
		dashAttack.performed += DashAttack;

		specialAttack = playerControls.Player.SpecialAttack;
		specialAttack.Enable();
		specialAttack.performed += SpecialAttack;
	}

	private void OnDisable()
	{
		move.Disable();
		dash.Disable();
		dashAttack.Disable();
		specialAttack.Disable();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AttackTrigger")
		{
			// Munis helth
		}
    }
}
