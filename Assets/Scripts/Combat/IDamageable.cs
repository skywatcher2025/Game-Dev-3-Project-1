public interface IDamageable : IHittable
{
    void TakeDamage(int damageAmount, float knockBackThrust);
}
