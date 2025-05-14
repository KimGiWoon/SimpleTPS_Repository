using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    public class ObservableProperty<T>
    {
        [SerializeField] T _value;
        public T Value { get { return _value; } set { if (value.Equals(value)) return; _value = value; Notify(); } }
        private UnityEvent<T> _onValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);
        }

        public void CancelSubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }

        public void CancelsubscibeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }

        private void Notify()
        {
            _onValueChanged?.Invoke(Value);
        }

    }
}

