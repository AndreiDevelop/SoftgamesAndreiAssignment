using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoftgamesAssignment
{
    public class SceneMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonExitMainMenu;

        private void OnEnable()
        {
            _buttonExitMainMenu.onClick.AddListener(OnButtonExitMainMenuClicked);
        }

        private void OnDisable()
        {
            _buttonExitMainMenu.onClick.RemoveListener(OnButtonExitMainMenuClicked);
        }
        
        private void OnButtonExitMainMenuClicked()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMenu);
        }
    }
}
