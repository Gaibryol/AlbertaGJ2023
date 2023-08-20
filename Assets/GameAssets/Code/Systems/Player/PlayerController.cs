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

	private PlayerMovement playerMovement;
	private PlayerAnimations playerAnimations;
	private PlayerAttacks playerAttacks;
	private Health health;
	private FlashEffect flash;

	private bool isInvulnerable = false;
	private bool isDead = false;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Dash(InputAction.CallbackContext context)
	{
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        StartCoroutine(playerMovement.HandleDash(transform.position, Constants.Player.Movement.DashForce, Constants.Player.Movement.DashDuration, direction));
		StartCoroutine(playerAnimations.HandleDashAnim(Constants.Player.Movement.DashDuration, direction));
	}

	private void Attack(InputAction.CallbackContext context)
	{
		Vector3 inputDirection = playerAttacks.HandleAttack(transform);
		StartCoroutine(playerAnimations.HandleAttackAnim(inputDirection));
	}

	private void DashAttack(InputAction.CallbackContext context)
	{
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;
        StartCoroutine(playerMovement.HandleDashAttack(transform.position, Constants.Player.Movement.DashAttackForce, Constants.Player.Movement.DashAttackDuration, direction));
		StartCoroutine(playerAnimations.HandleDashAttackAnim(Constants.Player.Movement.DashAttackDuration, direction));
	}

	private void SpecialAttack(InputAction.CallbackContext context)
	{
        Vector3 inputDirection = playerAttacks.HandleSpecialAttack(transform);
		StartCoroutine(playerAnimations.HandleSpecialAttackAnim(inputDirection));
	}

	private void Awake()
	{
		playerControls = new PlayerControls();
		playerMovement = new PlayerMovement(GetComponent<Rigidbody2D>(), GetComponentsInChildren<Collider2D>()[1]);
		playerAnimations = GetComponent<PlayerAnimations>();
		playerAttacks = GetComponent<PlayerAttacks>();
		flash = GetComponent<FlashEffect>();
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

		movespeed = PlayerStats.Movespeed;
		health = new Health(PlayerStats.MaxHealth);
		eventBrokerComponent.Publish(this, new UIEvents.SetHealth(health.Value));

		eventBrokerComponent.Subscribe<HealthEvents.IncreasePlayerHealth>(IncreasePlayerHealthHandler);
	}

	private void OnDisable()
	{
		move.Disable();
		attack.performed -= Attack;
		dash.Disable();
		dash.performed -= Dash;
		dashAttack.Disable();
		dashAttack.performed -= DashAttack;
		specialAttack.Disable();
		specialAttack.performed -= SpecialAttack;
		eventBrokerComponent.Unsubscribe<HealthEvents.IncreasePlayerHealth>(IncreasePlayerHealthHandler);
	}

	private IEnumerator StartInvulnerabilityTimer()
	{
		isInvulnerable = true;
		flash.StartFlash();
		yield return new WaitForSeconds(PlayerStats.InvulnerabilityTime);
		isInvulnerable = false;
		flash.StopFlash();
	}

	private void Death()
	{
		isDead = true;
		playerAnimations.Death(true);
		eventBrokerComponent.Publish(this, new GameModeEvents.PlayerDeath());
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Death));
		this.enabled = false;
	}

	private void TakeDamage(int damage)
	{
		if (isInvulnerable || isDead) return;
        health.Value -= damage;
        eventBrokerComponent.Publish(this, new UIEvents.SetHealth(health.Value));
        if (health.Value <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(StartInvulnerabilityTimer());
        }
    }

	#region EventBroker Handlers
	private void IncreasePlayerHealthHandler(BrokerEvent<HealthEvents.IncreasePlayerHealth> inEvent)
	{
		health.Value += inEvent.Payload.Value;
		eventBrokerComponent.Publish(this, new UIEvents.SetHealth(health.Value));
	}
	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == Constants.Enemy.AttackTriggerTag || collision.gameObject.tag == Constants.Enemy.ProjectileTag)
		{
			TakeDamage(PlayerStats.DamageTakenFromHit);
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == Constants.EnvironmentHazard.Tag)
		{
			TakeDamage(collision.transform.GetComponent<EnvironmentHazard>().Damage);
		}
	}
}
