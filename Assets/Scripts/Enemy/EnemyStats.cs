using UnityEngine;


public class EnemyStats : CharacterStats
{
    public EnemyConfigSO config;
    private EnemyConfigSO configInstance;

    void Start()
    {
        configInstance = ScriptableObject.Instantiate(config);
    }

    public EnemyConfigSO GetEnemyConfigSOInstance()
    {
        return config;
    }

    public override void TakeDamage(float damage)
    {
        configInstance.health -= damage;
        Debug.Log($"Enemy took {damage} damage, current health: {configInstance.health}");
        if (configInstance.health <= 0)
        {
            Die();
        }

        CombatTextSpawner.Instance.ShowWorldText($"{damage}", new Color(1f, 0f, 0.6f), transform.position + Vector3.up * 1.8f);


    }

    public override void Die()
    {
        var player = FindFirstObjectByType<PlayerStats>();
        if (player != null)
        {
            player.GainXP(configInstance.xpReward);
            //player.GainGold(configInstance.goldReward);
        }
        AudioManager.Instance.PlaySfx(SfxType.EnemyDeath);
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
}
