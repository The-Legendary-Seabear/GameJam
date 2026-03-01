using CGL.Controller;
using CGL.Data;
using CGL.Events;
using CGL.Extensions;
using UnityEngine;

namespace CGL.Animation
{
	// drives animator movement parameters from a CharacterControllerBase component.
	// attach alongside any CharacterControllerBase-derived component on a player or enemy.
	[RequireComponent(typeof(AnimatorController))]
	public class CharacterAnimatorController : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField]
		[Tooltip("Animator float parameter name for movement speed.")]
		private string speedParameter = "Speed";

		[SerializeField]
		[Tooltip("Animator bool parameter name for grounded state.")]
		private string groundedParameter = "IsGrounded";

		[SerializeField]
		[Tooltip("Animator bool parameter name for falling state.")]
		private string fallingParameter = "IsFalling";

		[SerializeField]
		[Tooltip("Animator trigger parameter name for jump.")]
		private string jumpParameter = "Jump";

		[SerializeField]
		[Tooltip("Animator trigger parameter name for death.")]
		private string deathParameter = "Death";

		[SerializeField]
		[Tooltip("Animator trigger parameter name for damage")]
		private string damageParameter = "Damage";

        [SerializeField]
		[Tooltip("Raised when the actor dies.")]
		private EventSO onDeathEvent;

		[SerializeField]
		[Tooltip("Raised when the actor jumps. Subscribe to set jump trigger.")]
		private EventSO onJumpEvent;

		[SerializeField]
		[Tooltip("Raised when the actor is damaged")]
		private EventSO onDamageEvent;

		private AnimatorController animatorController;
		private CharacterControllerBase controller;

		// cached parameter hashes for performance
		private int speedHash;
		private int groundedHash;
		private int fallingHash;
		private int jumpHash;
		private int deathHash;
		private int damageHash;

		private void Awake()
		{
			animatorController = GetComponent<AnimatorController>();
			controller = GetComponent<CharacterControllerBase>();

			if (controller == null)
				Debug.LogWarning("ActorAnimatorController: no CharacterControllerBase found on this GameObject.", this);

			// cache hashes once on awake
			speedHash = Animator.StringToHash(speedParameter);
			groundedHash = Animator.StringToHash(groundedParameter);
			fallingHash = Animator.StringToHash(fallingParameter);
			jumpHash = Animator.StringToHash(jumpParameter);
			deathHash = Animator.StringToHash(deathParameter);
			damageHash = Animator.StringToHash(damageParameter);
		}

		private void OnEnable()
		{
			onJumpEvent?.Subscribe(OnJump);
			onDeathEvent?.Subscribe(OnDeath);
			onDamageEvent?.Subscribe(OnDamage);
		}

		private void OnDisable()
		{
			onJumpEvent?.Unsubscribe(OnJump);
			onDeathEvent?.Unsubscribe(OnDeath);
			onDamageEvent?.Unsubscribe(OnDamage);
		}

		private void LateUpdate()
		{
			if (controller == null) return;

			float speed = controller.Velocity.MagnitudeXZ();

			animatorController.Animator.SetFloat(speedHash, speed);
			animatorController.Animator.SetBool(groundedHash, controller.IsGrounded);
			animatorController.Animator.SetBool(fallingHash, controller.IsFalling);
		}

		// jump is event-driven so the trigger is never missed or held across frames
		private void OnJump()
		{
			animatorController.Animator.SetTrigger(jumpHash);
		}

		private void OnDeath()
		{
			animatorController.Animator.SetTrigger(deathHash);
		}

		private void OnDamage()
		{
			animatorController.Animator.SetTrigger(damageHash);
		}
	}
}