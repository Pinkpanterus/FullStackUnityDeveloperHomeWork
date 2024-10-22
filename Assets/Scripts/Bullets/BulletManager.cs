using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField] private LevelBounds levelBounds;
        [SerializeField] private BulletPool bulletPool;
        public readonly HashSet<Bullet> m_activeBullets = new();


        private void FixedUpdate()
        {
            DestroyOutOfBoundariesBullets();
        }

        void DestroyOutOfBoundariesBullets()
        {
            List<Bullet> movingBullets = m_activeBullets.ToList();
            
            foreach (Bullet bullet in movingBullets)
            {
                if (!this.levelBounds.InBounds(bullet.transform.position))
                {
                    this.RemoveBullet(bullet);
                }
            }
        }

        public void SpawnBullet(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            Bullet bullet = bulletPool.Rent();
            bullet.Configure(position, color, physicsLayer, damage, isPlayer, velocity);
            bullet.OnCollisionEntered += RemoveBulletByCollision;
            m_activeBullets.Add(bullet);
        }

        private void RemoveBulletByCollision(Bullet bullet, Collision2D collision) {
            
            RemoveBullet(bullet);
        }


        private void RemoveBullet(Bullet bullet)
        {
            if (this.m_activeBullets.Remove(bullet)) 
            {
                bullet.OnCollisionEntered -= RemoveBulletByCollision;
                bulletPool.Return(bullet);
            }
        }
    }
}