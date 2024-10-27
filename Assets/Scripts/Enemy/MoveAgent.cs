using System;
using UnityEngine;

namespace ShootEmUp
{
    [RequireComponent(typeof(MoveComponent))]
    public class MoveAgent : MonoBehaviour
    {
        public event Action OnDestinationReached;
        [SerializeField] private MoveComponent moveComponent;
        private Vector2 destination;
        private bool isPointReached = true;
        
        private void FixedUpdate()
        {
            if (isPointReached)
                return;
            
            Vector2 vector = destination - (Vector2)transform.position;
            if (vector.magnitude <= 0.25f)
            {
                isPointReached = true;
                OnDestinationReached?.Invoke();
                return;
            }

            Vector2 direction = vector.normalized;
            moveComponent.Move(direction);
        }
        
        
        public void SetDestination(Vector2 endPoint)
        {
            destination = endPoint;
            isPointReached = false;
        }
    }
}