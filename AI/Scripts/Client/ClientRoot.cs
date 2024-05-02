using Modules.Tables.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(ClientMovement))]
    [RequireComponent(typeof(ClientOrderHandler))]
    public class ClientRoot : MonoBehaviour
    {
        [SerializeField, TextArea(5, 20)] private string DEBUG_STRING;

        private string _ID;
        private AIRoot _aiRoot;
        private ClientMovement _movement;
        private ClientOrderHandler _orderHandler;
        private ClientSeatPlace[] _allClientSeatPlace;
        private ClientVisual _clientVisual;
        private ClientSoundHandler _soundHandler;
        private EmojiOverlay _emojiOverlay;
        private Vector3 _defaultPosition;

        public string ID => _ID;
        public ClientMovement Movement => _movement;
        public ClientOrderHandler OrderHandler => _orderHandler;

        private void Awake()
        {
            _movement = GetComponent<ClientMovement>();
            _orderHandler = GetComponent<ClientOrderHandler>();
            _clientVisual = GetComponentInChildren<ClientVisual>();
            _soundHandler = GetComponentInChildren<ClientSoundHandler>();
            _emojiOverlay = GetComponentInChildren<EmojiOverlay>();

            if (_clientVisual == null)
                throw new MissingReferenceException("Missing child: ClientVisual");
            if (_soundHandler == null)
                throw new MissingReferenceException("Missing child: ClientSoundHandler");
            if (_emojiOverlay == null)
                throw new MissingReferenceException("Missing child: EmojiOverlay");
        }

        public void Init(AIRoot aiRoot, ClientData clientData, Action<ClientRoot> exitCallback, ClientSeatPlace[] allClientSeatPlace) 
        {
            _aiRoot = aiRoot;

            _ID = GenerateID();
            _allClientSeatPlace = allClientSeatPlace;
            _defaultPosition = transform.position;

            _movement.Init(this, clientData, allClientSeatPlace, exitCallback);
            _orderHandler.Init(this);
            _clientVisual.Init(this);
            _soundHandler.Init(_orderHandler);
            _emojiOverlay.Init(_orderHandler);

            _aiRoot.OnAllClientSeatPlacesUpdated += AiRoot_OnAllClientSeatPlacesUpdated;
        }

        private void Update() => UpdateDebugString();

        private void AiRoot_OnAllClientSeatPlacesUpdated(List<ClientSeatPlace> allClientSeatPlaces)
        {
            _movement.UpdateAllClientSeatPlaces(allClientSeatPlaces);
        }

        public void SetDefaultState() 
        {
            transform.position = _defaultPosition;
            DisableClient();
        }

        private string GenerateID()
        {
            Guid guid = Guid.NewGuid();
            string id = guid.ToString();
            id = id.Substring(id.Length - 5);
            return id;
        }

        public void OnDisable() => DisableClient();

        private void DisableClient() 
        {
            _movement.Disable();
            _orderHandler.Disable();
            _clientVisual.Disable();

            _aiRoot.OnAllClientSeatPlacesUpdated -= AiRoot_OnAllClientSeatPlacesUpdated;
        }

        private void UpdateDebugString() 
        {
            DEBUG_STRING = $"Client id - {ID} " +
                $"\n On move - {_movement.OnMove}" +
                $"\n All seat place count - {_allClientSeatPlace.Length}" +
                $"\n Order list count - {_orderHandler.OrderList.Count} " +
                $"\n Move target type - {_movement.CurrentTargetType}" +
                $"\n Chose order - {_orderHandler.ChoseOrder}";

            for (int i = 0; i < _orderHandler.OrderList.Count; i++)
            {
                DEBUG_STRING += $"\n Order item ID - {_orderHandler.OrderList[i].Item.ItemSO.ID}";
            }
        }
    }
}
