using UnityEngine;

public class CharacterAnimator : MonoBehaviour, ICharacterAnimation
{
	[SerializeField] private Animator _animator;

	public void Idle()
	{
		_animator?.Play("Idle");
	}

	public void Attack()
	{
		_animator?.Play("Attack");
	}

	public void Die()
	{
		_animator?.Play("Die");
	}

	public void Move()
	{
		_animator?.Play("Move");
	}
}
