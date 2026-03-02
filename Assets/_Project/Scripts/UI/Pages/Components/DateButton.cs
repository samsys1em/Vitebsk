using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace Scripts.UI.Pages.Components
{
    [RequireComponent(typeof(Button))]
    public abstract class DateButton<TDataType> : MonoBehaviour
    {
        public event Action<TDataType> Down;

        [SerializeField] private TDataType _data;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;
        
        private TMP_Text _yearText;
        private Button _button;
        private static DateButton<TDataType> _current;


        protected TDataType Data => _data;
        protected TMP_Text YearText => _yearText;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ButtonDown);

            _yearText = GetComponentInChildren<TMP_Text>();

            OnAwake();
        }

        public void ButtonDown()
        {
            OnButtonDown();
        }

        private void OnButtonDown()
        {
            if (_current == this)
                return;

            _current?.SetSelect(false);
            _current = this;
            _current.SetSelect(true);
            Down?.Invoke(_data);
        }

        public void SetSelect(bool value)
        {
            if (value == false && _current == this)
                _current = null;

            _yearText.color = value ? _activeColor : _inactiveColor;
            OnSelectChanged(value);
        }

        protected virtual void OnSelectChanged(bool value)
        {
        }

        protected virtual void OnAwake()
        {
        }
    }
}