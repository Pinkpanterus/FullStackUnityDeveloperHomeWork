using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerInput : MonoBehaviour
    {
        public Action OnFireButtonPressed;
        public Action<int> OnMoveButtonPressed;
        private void Update()
        {
            CheckPlayerInput();
        }

        private void CheckPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                // _fireRequired = true;
                OnFireButtonPressed?.Invoke();
            if (Input.GetKey(KeyCode.LeftArrow))
                // this._moveDirection = -1;
                OnMoveButtonPressed?.Invoke(-1);
            else if (Input.GetKey(KeyCode.RightArrow))
                // this._moveDirection = 1;
                OnMoveButtonPressed?.Invoke(1);
            else
                // this._moveDirection = 0;
                OnMoveButtonPressed?.Invoke(0);
        }
        
    }
}