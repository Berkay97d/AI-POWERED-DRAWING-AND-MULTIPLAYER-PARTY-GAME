using TMPro;
using UnityEngine;

namespace UI
{
    public class ScreenLogger : MonoBehaviour
    {
        private const string Path = nameof(ScreenLogger);


        [SerializeField] private LogMessageUI logMessage;
        

        private static ScreenLogger ms_Instance;


        public static void Log(string value)
        {
            var instance = GetInstance();

            instance.logMessage.SetText(value);
            instance.logMessage.PopUp();
        }


        private static ScreenLogger GetInstance()
        {
            if (ms_Instance) return ms_Instance;

            var prefab = Resources.Load<ScreenLogger>(Path);

            ms_Instance = Instantiate(prefab);
            DontDestroyOnLoad(ms_Instance);

            return ms_Instance;
        }
    }
}