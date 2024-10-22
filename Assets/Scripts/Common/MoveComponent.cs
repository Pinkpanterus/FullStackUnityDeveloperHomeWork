using System;
using UnityEngine;

namespace ShootEmUp
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed = 5.0f;


        public void Move(Vector2 moveDirection)
        {
            Vector2 moveStep = moveDirection * Time.fixedDeltaTime * _speed;
            Vector2 targetPosition = _rigidbody.position + moveStep;
            _rigidbody.MovePosition(targetPosition);
        }
    }
}