namespace Character_Common
{
    public interface IDamageable
    {
        int MaxHealth { get; set; }
        int CurrentHealth { get; set; }
        
        void ReceiveDamage(int damage);

        void Die();
    }
}
