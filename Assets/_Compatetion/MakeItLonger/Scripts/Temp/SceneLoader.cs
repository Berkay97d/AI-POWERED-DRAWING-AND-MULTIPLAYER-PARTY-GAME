using General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeItLonger.Temp
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        
        private bool isSceneLoaded = false;
        
        
        private void Update()
        {
            if (gameController.GetIsGameOver())
            {
                if(isSceneLoaded) return;
                
                isSceneLoaded = true;
                LazyCoroutines.WaitForSeconds(6f, () =>
                {
                    SceneManager.LoadScene(6);
                });
            }
        }
    }
}