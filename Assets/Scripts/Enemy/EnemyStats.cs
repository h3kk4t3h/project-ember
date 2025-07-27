using UnityEngine;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        public EnemyConfigSO config;
        private float currentHealth;

        void Start()
        {
            currentHealth = config.health;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
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
                player.GainXP(config.xpReward);
                player.GainGold(config.goldReward);
            }
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
    }
}