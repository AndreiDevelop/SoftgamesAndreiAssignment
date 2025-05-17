using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoftgamesAssignment
{
    public class SceneMenuPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textFPS;
        [SerializeField] private Button _buttonExitMainMenu;

        private float _deltaTime;
        
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

        void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _textFPS.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}
