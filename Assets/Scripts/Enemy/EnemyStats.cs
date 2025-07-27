using UnityEngine;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        public EnemyConfigSO config;
        private EnemyConfigSO configInstance;
        private float currentHealth;

        void Start()
        {
            configInstance = ScriptableObject.Instantiate(config);
            currentHealth = configInstance.health;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"Enemy took {damage} damage, current health: {currentHealth}");
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                player.GainXP(configInstance.xpReward);
                player.GainGold(configInstance.goldReward);
            }
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
    }
}