using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SoftgamesAssignment
{
    public class SceneLoadingManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        public static SceneLoadingManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //LoadProjectContext();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartLoading();
        }

        private async UniTask StartLoading()
        {
            await UniTask.WaitForSeconds(1);

            LoadSceneAdditive(Constants.SceneMenu);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _camera.gameObject.SetActive(false);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}