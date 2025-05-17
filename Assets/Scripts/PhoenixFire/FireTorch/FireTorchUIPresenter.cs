using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoftgamesAssignment.PhoenixFire
{
    public class FireTorchUIPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonOrange;
        [SerializeField] private Button _buttonGreen;
        [SerializeField] private Button _buttonBlue;
        
        [Inject] private FireTorchModel _fireTorchModel;

        private void OnEnable()
        {
            _buttonOrange.onClick.AddListener(OnButtonOrangeClicked);
            _buttonGreen.onClick.AddListener(OnButtonGreenClicked);
            _buttonBlue.onClick.AddListener(OnButtonBlueClicked);
        }

        private void OnDisable()
        {
            _buttonOrange.onClick.RemoveListener(OnButtonOrangeClicked);
            _buttonGreen.onClick.RemoveListener(OnButtonGreenClicked);
            _buttonBlue.onClick.RemoveListener(OnButtonBlueClicked);
        }
        
        private void OnButtonBlueClicked()
        {
            _fireTorchModel.SetColor(FireTorchColor.Blue);
        }

        private void OnButtonGreenClicked()
        {
            _fireTorchModel.SetColor(FireTorchColor.Green);
        }

        private void OnButtonOrangeClicked()
        {
            _fireTorchModel.SetColor(FireTorchColor.Orange);
        }
    }
}
