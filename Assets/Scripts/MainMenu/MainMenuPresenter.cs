using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoftgamesAssignment
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonAceOfShadows;
        [SerializeField] private Button _buttonMagicWords;
        [SerializeField] private Button _buttonPhoenixFlame;

        private void OnEnable()
        {
            _buttonAceOfShadows.onClick.AddListener(OnButtonAceOfShadowsClicked);
            _buttonMagicWords.onClick.AddListener(OnButtonMagicWordsClicked);
            _buttonPhoenixFlame.onClick.AddListener(OnButtonPhoenixFlameClicked);
        }

        private void OnDisable()
        {
            _buttonAceOfShadows.onClick.RemoveListener(OnButtonAceOfShadowsClicked);
            _buttonMagicWords.onClick.RemoveListener(OnButtonMagicWordsClicked);
            _buttonPhoenixFlame.onClick.RemoveListener(OnButtonPhoenixFlameClicked);
        }
        
        private void OnButtonAceOfShadowsClicked()
        {
            Debug.Log("Ace of Shadows clicked");
            
            // Load the Ace of Shadows scene
            SceneLoadingManager.Instance.LoadScene(Constants.SceneAceOfShadows);
        }
        
        private void OnButtonMagicWordsClicked()
        {
            Debug.Log("Magic Words clicked");
            
            // Load the Magic Words scene
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMagicWords);
        }
        
        private void OnButtonPhoenixFlameClicked()
        {
            Debug.Log("Phoenix Flame clicked");
            
            // Load the Phoenix Flame scene
            SceneLoadingManager.Instance.LoadScene(Constants.ScenePhoenixFlame);
        }
    }
}
