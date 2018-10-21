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
		_animator?.Play("Attack01");
	}

	public void Die()
	{
		_animator?.Play("Death");
	}

	public void Move()
	{
		_animator?.Play("Walk");
	}
}
