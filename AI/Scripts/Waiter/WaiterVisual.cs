using Modules.Items.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class WaiterVisual : MonoBehaviour
    {
        private const string IsCarry = nameof(IsCarry);
        private const string IsMoving = nameof(IsMoving);

        private WaiterMovement _waiterMovement;
        private WaiterCarryHandler _waiterCarryHandler;
        private Animator _animator;

        private void Awake() => _animator = GetComponent<Animator>();

        internal void Init(WaiterMovement waiterMovement, WaiterCarryHandler waiterCarryHandler)
        {
            _waiterMovement = waiterMovement;
            _waiterCarryHandler = waiterCarryHandler;

           _waiterMovement.OnMoved += WaiterMovement_OnMoved;
            _waiterMovement.OnTargetReached += WaiterMovement_OnTargetReached;
            _waiterCarryHandler.OnPutItem += OnItemCountChanged;
            _waiterCarryHandler.OnTookItem += OnItemCountChanged;
        }

        private void WaiterMovement_OnTargetReached() => _animator.SetBool(IsMoving, false);

        private void WaiterMovement_OnMoved() => _animator.SetBool(IsMoving, true);

        private void OnItemCountChanged()
        {
            if(_waiterCarryHandler.ItemCarryList.Count == 0) 
                _animator.SetBool(IsCarry, false);
            else
                _animator.SetBool(IsCarry, true);

            UpdateItemVisual(_waiterCarryHandler.ItemCarryList);
        }

        internal void UpdateItemVisual(List<Item> itemCarryList)
        {
            float nextPos = 0f;

            for (int i = 0; i < itemCarryList.Count; i++)
            {
                itemCarryList[i].transform.localPosition = Vector3.zero;
                itemCarryList[i].transform.localRotation = Quaternion.identity;

                if (i > 0)
                    itemCarryList[i].transform.localPosition = new Vector3(0f, nextPos, 0f);

                nextPos += itemCarryList[i].TopPoint.localPosition.y * itemCarryList[i].transform.localScale.y;
            }
        }

        private void OnDisable()
        {
            _waiterCarryHandler.OnPutItem -= OnItemCountChanged;
            _waiterCarryHandler.OnTookItem -= OnItemCountChanged;
        }
    }
}
