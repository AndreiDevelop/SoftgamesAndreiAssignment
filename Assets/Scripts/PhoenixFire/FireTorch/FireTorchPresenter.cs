using UniRx;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.PhoenixFire
{
    public class FireTorchPresenter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [Inject] private FireTorchModel _fireTorchModel;
        
        private const string OrangeColorTrigger = "Orange";
        private const string GreenColorTrigger = "Green";
        private const string BlueColorTrigger = "Blue";
        
        void Start()
        {
            _fireTorchModel.TorchColor
                .Subscribe(color =>
                {
                    ChangeColor(color);
                })
                .AddTo(this);
        }

        private void ChangeColor(FireTorchColor color)
        {
            ResetTriggers();
            
            switch (color)
            {
                case FireTorchColor.Orange:
                    _animator.SetTrigger(OrangeColorTrigger);
                    break;
                case FireTorchColor.Green:
                    _animator.SetTrigger(GreenColorTrigger);
                    break;
                case FireTorchColor.Blue:
                    _animator.SetTrigger(BlueColorTrigger);
                    break;
            }
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(OrangeColorTrigger);
            _animator.ResetTrigger(GreenColorTrigger);
            _animator.ResetTrigger(BlueColorTrigger);
        }
    }
}
