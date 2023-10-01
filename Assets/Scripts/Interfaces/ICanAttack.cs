namespace Gespell.Interfaces
{
    public interface ICanAttack
    {
        void Attack(IDamageable target, int amount);
    }
}