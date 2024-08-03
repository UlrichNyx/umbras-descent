public class PlayerStats : Stats
{
    public override void Die()
    {
        gameManager.PlayerDeath();
    }
}
