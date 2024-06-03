using _CardPacks.Scripts;
using NaughtyAttributes;
using UI;
using UnityEngine;

namespace _UserOperations.Scripts
{
    public class Test_User_Operations : MonoBehaviour
    {
        [SerializeField] private int gem;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddGem()
        {
            LocalUser.AddGem(gem, () => Debug.Log($"{gem} gem(s) are added!"), () => Debug.LogError("gem could not be added!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryRemoveGem()
        {
            LocalUser.TryRemoveGem(gem, () => Debug.Log($"{gem} gem(s) are removed!"), () => Debug.LogError("gem could not be removed!"));
        }
        
        [SerializeField] private int coin;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddCoin()
        {
            LocalUser.AddCoin(coin, () => Debug.Log($"{coin} coin(s) are added!"), () => Debug.LogError("coin could not be added!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryRemoveCoin()
        {
            LocalUser.TryRemoveCoin(coin, () => Debug.Log($"{coin} coin(s) are removed!"), () => Debug.LogError("coin could not be removed!"));
        }
        
        [SerializeField] private int energy;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddEnergy()
        {
            LocalUser.AddEnergy(energy, () => Debug.Log($"{energy} energy(s) are added!"), () => Debug.LogError("energy could not be added!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryRemoveEnergy()
        {
            LocalUser.TryRemoveEnergy(energy, () => Debug.Log($"{energy} energy(s) are removed!"), () => Debug.LogError("energy could not be removed!"));
        }
        
        [SerializeField] private int experience;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddExperience()
        {
            LocalUser.AddExperience(experience, () => Debug.Log($"{experience} experience(s) are added!"), () => Debug.LogError("experience could not be added!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryRemoveExperience()
        {
            LocalUser.TryRemoveExperience(experience, () => Debug.Log($"{experience} experience(s) are removed!"), () => Debug.LogError("experience could not be removed!"));
        }

        [SerializeField] private int tokenCount;
        [SerializeField] private CardRarity rarity;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddUpgradeToken()
        {
            LocalUser.AddUpgradeToken(tokenCount, rarity, () => Debug.Log($"{tokenCount} token(s) are added!"), () => Debug.LogError("token could not be added!"));
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryRemoveUpgradeToken()
        {
            LocalUser.TryRemoveUpgradeToken(tokenCount, rarity, () => Debug.Log($"{tokenCount} token(s) are removed!"), reason => Debug.LogError($"[{reason}]: token could not be removed!"));
        }

        [SerializeField] private CardPackType cardPackType;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryAddCardPack()
        {
            LocalUser.TryAddCardPack(cardPackType, () => Debug.Log($"{cardPackType} Card Pack is added!"), reason => Debug.LogError($"[{reason}]: card pack could not be added!"));
        }

        [SerializeField] private int cardPackSlotIndex;
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TryUnlockCardPack()
        {
            LocalUser.TryUnlockCardPack(cardPackSlotIndex, () => Debug.Log("Card pack is unlocking!"), () => Debug.LogError("Card pack could not be unlocked!"));
        }
    }
}