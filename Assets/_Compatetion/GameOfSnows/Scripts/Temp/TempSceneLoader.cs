using System.Linq;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using _Compatetion.GameOfSnows.Scripts;
using EnterAndExitUI_Competation.Scripts;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempSceneLoader : MonoBehaviour
{
    private void Start()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, Contester[] e)
    {
        var playerOrder = e
            .Select(contester => contester.GetIndex())
            .ToArray();
                
        LazyCoroutines.WaitForSeconds(3f, () =>
        {
            ExitTest_Script.SetPlayerOrder(playerOrder);
            SceneManager.LoadScene(6);
        });
    }
}
