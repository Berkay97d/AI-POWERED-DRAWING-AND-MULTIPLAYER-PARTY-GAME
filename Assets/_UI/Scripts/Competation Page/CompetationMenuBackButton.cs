using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class CompetationMenuBackButton : MonoBehaviour
    {
        [SerializeField] private Transform competationMenuMainTransform;
        [SerializeField] private Transform gameModesMainTransform;
        
        private Button button;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(GoBack);
        }

        private void GoBack()
        {
            /*creationMenuMainTransform.localScale = Vector3.zero;
            mainCanvasesMainTransform.gameObject.SetActive(true);
            creationMenuMainTransform.gameObject.SetActive(false);*/
            
            competationMenuMainTransform.localScale = Vector3.zero;
            gameModesMainTransform.localScale = Vector3.zero;
            gameModesMainTransform.gameObject.SetActive(true);
                
            gameModesMainTransform.DOScale(Vector3.one, 0.35f).OnComplete(() =>
            {
                competationMenuMainTransform.gameObject.SetActive(false);
            });
        }
    }
}