using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoftgamesAssignment
{
    public class FinishPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject _holder;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private Button _buttonOk;
        
        public void Activate(string text)
        {
            _textDescription.text = text;
            _holder.SetActive(true);
        }
        
        private void OnEnable()
        {
            _buttonOk.onClick.AddListener(LoadMenuScene);
        }

        private void OnDisable()
        {
            _buttonOk.onClick.RemoveListener(LoadMenuScene);
        }

        private void LoadMenuScene()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMenu);
        }
    }
}
