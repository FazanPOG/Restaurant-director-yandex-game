using Modules.Root.Scripts;
using System;
using System.Collections;
using UnityEngine;

namespace Modules.Player.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Range(0, 20)] private float _moveSpeed;
        [SerializeField] private LayerMask _triggerLayerMask;

        private PlayerInputHandler _playerInputHandler;

        private float _defaultMoveSpeed;
        private float _maxSpeed;
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        internal void Init(PlayerInputHandler playerInputHandler)
        {
            _playerInputHandler = playerInputHandler;
            _maxSpeed = _moveSpeed;
            _defaultMoveSpeed = _moveSpeed;
        }

        private void Update()
        {
            MovementHandler();
        }

        private void MovementHandler()
        {
            Vector2 inputVector = _playerInputHandler.MovementInput.normalized;

            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            float dir = _playerInputHandler.GetMaxInputDirection();
            _moveSpeed = _maxSpeed * dir;

            float moveDistance = _moveSpeed * Time.deltaTime;
            float playerRadius = .3f;
            float playerHeight = 2f;
            bool canMove = !Physics.CapsuleCast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) , transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance, _triggerLayerMask);
            Debug.DrawRay(transform.position + Vector3.up * playerHeight, moveDir, Color.green, moveDistance, false);

            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                Debug.DrawRay(transform.position, moveDirX, Color.yellow, moveDistance, false);
                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                    else
                    {
                        //can't move on any direction
                    }
                }
            }
            if (canMove)
            {
                transform.position += moveDir * _moveSpeed * Time.deltaTime;
            }
            _isMoving = moveDir != Vector3.zero;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

        public void SpeedBonus(float bonusCoefficient) 
        {
            _maxSpeed *= bonusCoefficient;
            StartCoroutine(SpeedBonusTimer());
        }

        private IEnumerator SpeedBonusTimer(float timer = 60f) 
        {
            yield return new WaitForSeconds(timer);
            _maxSpeed = _defaultMoveSpeed;
        }
    }
}
