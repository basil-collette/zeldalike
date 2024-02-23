using System;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    public class DeleteWindowManager : MonoBehaviour
    {
        [SerializeField] GameObject _infoWindow;

        private Action _onValidate;

        public void Show(Action OnValidate)
        {
            _onValidate = OnValidate;
            _infoWindow.SetActive(true);
        }

        public void Hide()
        {
            _infoWindow.SetActive(false);
        }

        public void OnValide()
        {
            _onValidate?.Invoke();
            _infoWindow.SetActive(false);
        }

        public void OnCancel()
        {
            _infoWindow.SetActive(false);
        }

    }
}
