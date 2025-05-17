using UnityEngine;
using UniRx;

namespace SoftgamesAssignment.PhoenixFire
{
    public enum FireTorchColor
    {
        Orange,
        Green,
        Blue
    }
    
    public class FireTorchModel
    {
        public ReactiveProperty<FireTorchColor> TorchColor = new ReactiveProperty<FireTorchColor>(FireTorchColor.Orange);
        
        public void SetColor(FireTorchColor color)
        {
            TorchColor.Value = color;
        }
    }
}
