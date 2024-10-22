using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private bool isPlayer;
        private int damage;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionEntered?.Invoke(this, collision);
            this.DealDamage(this, collision.gameObject);
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            int damage = bullet.damage;
            if (damage > 0)
            {
                if (other.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.GetDamage(damage);
                }
            }
        }

        public void Configure(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            this.transform.position = position;
            this.spriteRenderer.color = color;
            this.gameObject.layer = physicsLayer;
            this.damage = damage;
            this.isPlayer = isPlayer;
            this.rigidbody2D.velocity = velocity;
        }
    }
}