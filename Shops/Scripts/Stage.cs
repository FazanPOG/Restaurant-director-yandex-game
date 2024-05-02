using Modules.Common.Scripts;
using Modules.Items.Scripts;
using Modules.Tables.Scripts;
using Modules.UI.Scripts;
using Modules.YandexGames.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Shops.Scripts
{
    public class Stage : MonoBehaviour, IOpenable
    {
        [Header("If has")]
        [SerializeField] private Table[] _tables;
        [Header("Previous stage invinsible wall")]
        [SerializeField] private GameObject _invinsibleWall;
        [Space(15)]
        [Header("If has not, use previous stage cash register")]
        [SerializeField] private CashRegister _cashRegister;
        [Space(15)]
        [SerializeField, TextArea(5, 10)] private string DEBUG_STRING;

        private ClientSeatPlace[] _allClientSeatPlaces;
        private List<ClientSeatPlace> _openClientSeatPlaces;
        private StageVisual _visual;
        private Shop _shop;
        private OpenUI _openUI;
        private bool _isOpen;

        public List<ClientSeatPlace> OpenClientSeatPlaces => _openClientSeatPlaces;
        public CashRegister CashRegister => _cashRegister;
        public Table[] Tables => _tables;
        public Shop Shop => _shop;
        public Item[] StageItems => GetStageItems().ToArray();
        public OpenUI OpenUI => _openUI;
        public bool IsOpen => _isOpen;

        public event Action<Stage> OnOpened;
        public event Action OnOpenClientSeatPlacesCountChanged;

        private void Awake()
        {
            _visual = GetComponentInChildren<StageVisual>();
            _allClientSeatPlaces = GetComponentsInChildren<ClientSeatPlace>();
            _openUI = GetComponentInChildren<OpenUI>();

            if (_visual == null)
                throw new MissingReferenceException("Missing child game objects: StageVisual");

            _openClientSeatPlaces = new List<ClientSeatPlace>();
        }

        internal void Init(Shop shop)
        {
            _shop = shop;

            foreach (ClientSeatPlace place in _allClientSeatPlaces)
            {
                place.Init(this);

                if (place.IsOpen)
                    _openClientSeatPlaces.Add(place);
                else
                    place.OnOpened += ClientSeatPlace_OnOpened;
            }

            _isOpen = SaveSystem.Instance.LoadOpenObject(gameObject);

            if (_openUI != null) 
            {
                if (_isOpen == false) 
                {
                    _openUI.Init();
                }
                else
                {
                    _openUI.Disable();

                    if (_invinsibleWall != null)
                        _invinsibleWall.gameObject.SetActive(false);
                }
            }
            else 
            {
                _isOpen = true;

                if (_invinsibleWall != null)
                    _invinsibleWall.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            DEBUG_STRING = "";

            DEBUG_STRING = $"IsOpen: {_isOpen} ";
            if (_openUI != null)
                DEBUG_STRING += "OpenUI excist";
            else
                DEBUG_STRING += "OpenUI does not excist";
        }

        private void ClientSeatPlace_OnOpened(ClientSeatPlace place)
        {
            _openClientSeatPlaces.Add(place);

            OnOpenClientSeatPlacesCountChanged?.Invoke();
        }

        public void Open()
        {
            _visual.Enable();

            if (_allClientSeatPlaces.Length > 0)
                EnableClientSeatPlaces();

            if (_invinsibleWall != null)
                _invinsibleWall.gameObject.SetActive(false);

            _isOpen = true;

            OnOpened?.Invoke(this);
        }

        private List<Item> GetStageItems() 
        {
            List<Item> items = new List<Item>();

            foreach (Table table in _tables) 
            {
                if(items.Contains(table.GetItem()) == false)
                    items.Add(table.GetItem());
            }

            return items;
        }

        public void Disable()
        {
            _visual.Disable();

            if (_shop.IsStageNextToOpen(this) == false) 
                _openUI.Disable();

            if (_allClientSeatPlaces.Length > 0)
                DisableClientSeatPlaces();
            if (_invinsibleWall != null)
                _invinsibleWall.gameObject.SetActive(true);
        }

        private void EnableClientSeatPlaces() 
        {
            foreach(ClientSeatPlace place in _allClientSeatPlaces) 
            {
                place.gameObject.SetActive(true);
            }
        }

        private void DisableClientSeatPlaces() 
        {
            foreach(ClientSeatPlace place in _allClientSeatPlaces) 
            {
                place.gameObject.SetActive(false);
            }
        }

    }
}

