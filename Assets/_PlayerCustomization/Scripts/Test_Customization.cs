using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _PlayerCustomization.Scripts
{
    public class Test_Customization : MonoBehaviour
    {
        [SerializeField] private SkinData skinToUnlock;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UnlockSkin()
        {
            LocalUser.UnlockSkin(skinToUnlock, () => Debug.Log("Skin unlocked!"), () => Debug.Log("Failed to unlock skin!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void ResetSkins()
        {
            LocalUser.GetUserData(userData =>
            {
                var defaultUserData = UserData.Default;
                userData.skins = defaultUserData.skins;
                LocalUser.SetUserData(userData, () => Debug.Log("Skins reset!"), () => Debug.Log("Failed"));
            }, () => Debug.Log("Failed!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UnlockAllSkins()
        {
            LocalUser.GetUserData(userData =>
            {
                userData.skins = new SkinData[12 + 5 + 4 + 3 + 11];

                var counter = 0;

                for (var i = 0; i < 12; i++)
                {
                    userData.skins[counter] = new SkinData {mainIndex = 0, subIndex = i};
                    counter += 1;
                }
                
                for (var i = 0; i < 5; i++)
                {
                    userData.skins[counter] = new SkinData {mainIndex = 1, subIndex = i};
                    counter += 1;
                }
                
                for (var i = 0; i < 4; i++)
                {
                    userData.skins[counter] = new SkinData {mainIndex = 2, subIndex = i};
                    counter += 1;
                }
                
                for (var i = 0; i < 3; i++)
                {
                    userData.skins[counter] = new SkinData {mainIndex = 3, subIndex = i};
                    counter += 1;
                }
                
                for (var i = 0; i < 11; i++)
                {
                    userData.skins[counter] = new SkinData {mainIndex = 4, subIndex = i};
                    counter += 1;
                }
                
                LocalUser.SetUserData(userData, () => Debug.Log("All skins unlocked!"), () => Debug.Log("Failed!"));
            }, () => Debug.Log("Failed!"));
        }
    }
}