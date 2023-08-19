using System;
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
	[SerializeField] private int startingHealth;

	private PlayerMovement playerMovement;
	private PlayerAnimations playerAnimations;
	private PlayerAttacks playerAttacks;
	private Health health;

	private int orbs;
	private List<bool> combo = new List<bool>();

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Dash(InputAction.CallbackContext context)
	{
		StartCoroutine(playerMovement.HandleDash(transform.position, Constants.Player.Movement.DashForce, Constants.Player.Movement.DashDuration));
		StartCoroutine(playerAnimations.HandleDashAnim(Constants.Player.Movement.DashDuration));
	}

	private void Attack(InputAction.CallbackContext context)
	{
		playerAttacks.HandleAttack(transform);
		StartCoroutine(playerAnimations.HandleAttackAnim());
		Debug.Log("attack");

		if (combo.Count >= 3) return; // Wait for Reset combo to trigger
		combo.Add(false); // Change to True for Special attack, False for normal attack
		eventBrokerComponent.Publish(this, new UIEvents.SetCombo(combo));
		if (combo.Count >= 3)
		{
			StartCoroutine(ResetCombo(.5f)); // Should time before can start a new attack
		}
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
		health = new Health(startingHealth);
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
    
		eventBrokerComponent.Subscribe<HealthEvents.IncreasePlayerHealth>(IncreasePlayerHealthHandler);
		eventBrokerComponent.Subscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
	}

	private void OnDisable()
	{
		move.Disable();
		dash.Disable();
		dashAttack.Disable();
		specialAttack.Disable();
		eventBrokerComponent.Unsubscribe<HealthEvents.IncreasePlayerHealth>(IncreasePlayerHealthHandler);
		eventBrokerComponent.Unsubscribe<InteractionEvents.IncreaseOrbs>(IncreaseOrbsHandler);
	}

	#region EventBroker Handlers
	private void IncreaseOrbsHandler(BrokerEvent<InteractionEvents.IncreaseOrbs> inEvent)
	{
		orbs += 1;
		eventBrokerComponent.Publish(this, new UIEvents.SetOrbs(orbs));
	}
	
	private void IncreasePlayerHealthHandler(BrokerEvent<HealthEvents.IncreasePlayerHealth> inEvent)
	{
		health.Value += inEvent.Payload.Value;
		eventBrokerComponent.Publish(this, new UIEvents.SetHealth(health.Value));
	}
	#endregion

	private IEnumerator ResetCombo(float delay)
	{
		yield return new WaitForSeconds(delay);
		combo.Clear();
		eventBrokerComponent.Publish(this, new UIEvents.SetCombo(combo));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == Constants.Enemy.AttackTriggerTag || collision.gameObject.tag == Constants.Enemy.ProjectileTag)
		{
			health.Value -= 1;
			eventBrokerComponent.Publish(this, new UIEvents.SetHealth(health.Value));
		}

		if (collision.gameObject.tag == Constants.Enemy.ProjectileTag)
		{
			Destroy(collision.gameObject);
		}

		IInteractable interactable = collision.GetComponent<IInteractable>();
		if (interactable != null)
		{
			interactable.Interact();
		}
	}
}
