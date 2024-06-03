using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using General;
using TMPro;
using UnityEngine;

namespace UI
{
   
    public struct BookSubTitle
    {
        public string Name { get; set; }

        public BookSubTitle(string name)
        {
            Name = name;
        }
    }
    
    public class Book : MonoBehaviour
    {
        [SerializeField] private NavigationPage navigationPagePrefab;
        [SerializeField] private CardPage cardPagePrefab;
        [SerializeField] private Transform pageParent;
        [SerializeField] private TMP_Text playerPointText;
        [SerializeField] private Camera mainCamera;
        
        
        private CollectionPageArrow[] collectionPageArrows;
        public event EventHandler<string> OnActivePageChanged;
        public static readonly string[] SUBTITLES = {"ASTRONAUT","PLANET","WOLF","LION","DOG","ROBOT","PENGUIN"
            ,"CAR","AIRPLANE","NINJA","SWORD","DRAGON","RING", "UNICORN"};
        
        private readonly List<BookSubTitle> subTitles = new List<BookSubTitle>();
        private readonly List<Page> allPages = new List<Page>();
        private readonly List<CardPage> cardPages = new List<CardPage>();
        private NavigationPage navigationPage;
        
        private Page activePage;
        private const float PageGap = 0.025f;
        private int bookPoint;


        private void Awake()
        {
            SetBook();
            InitPages();
        }

        private void Start()
        {
            CollectionMenuController.Instance.OnCollectionPageGone += OnCollectionPageGone;
            CollectionMenuController.Instance.OnCollectionPageArrive += OnCollectionPageArrive;
            
            playerPointText.text = bookPoint.ToString();
        }

        private void OnCollectionPageArrive(object sender, EventArgs e)
        {
            SetActivePage(allPages[0]);
        }

        private void OnCollectionPageGone(object sender, EventArgs e)
        {
            StopAllCoroutines();
            
            foreach (var page in allPages)
            {
                page.GetVisual().GoOriginalPos();
                SetActivePage(allPages[0]);
            }
        }

        private void OnDestroy()
        {
            CollectionMenuController.Instance.OnCollectionPageArrive -= OnCollectionPageArrive;
            CollectionMenuController.Instance.OnCollectionPageGone -= OnCollectionPageGone;

            if (collectionPageArrows != null)
            {
                foreach (var arrow in collectionPageArrows)
                {
                    arrow.OnCollectionPageArrowClicked -= OnCollectionPageArrowClicked;
                }
            }

            foreach (var page in cardPages)
            {
                page.OnPagePointChanged -= OnPagePointChanged;
            }
        }

        public NavigationPage GetNavigationPage()
        {
            return navigationPage;
        }

        public CardPage GetPageBySubTitle(string value)
        {
            return cardPages
                .FirstOrDefault(page => page.GetSubTitle().Name == value);
        }
        
        
        private void SetBook()
        {
            string[] subs = {"ASTRONAUT","PLANET","WOLF","LION","DOG","ROBOT","PENGUIN"
                ,"CAR","AIRPLANE","NINJA","SWORD","DRAGON","RING", "UNICORN"};
            
            foreach (var sub in subs)
            {
                var a = new BookSubTitle(sub);
                subTitles.Add(a);
            }
        }
        
        private void InitPages()
        {
            int counter = 0;

            var np = Instantiate(navigationPagePrefab, pageParent);
            
            navigationPage = np;
            allPages.Add(navigationPage);
            navigationPage.SetSubtitle(new BookSubTitle("EMPTY SUBTITLE"));
            
            for (int i = 0; i < subTitles.Count; i++)
            {
                counter++;
                
                var cardPage = Instantiate(cardPagePrefab, pageParent);
                
                cardPages.Add(cardPage);
                allPages.Add(cardPage);
                
                var pos = counter * PageGap;
                cardPage.transform.localPosition = new Vector3(0, pos, 0);
                cardPage.SetSubtitle(subTitles[i]);
                
                cardPage.OnPagePointChanged += OnPagePointChanged;
            } 

            SetActivePage(allPages[0]);
        }
        
        private void SetActivePage(Page page)
        {
            if (page.TryGetComponent(out NavigationPage np))
            {
                foreach (var element in np.GetNavigationElements())
                {
                    element.OnClick += OnNavigationElementClick;
                }
            }
            
            if (activePage != null)
            {
                if (activePage.TryGetComponent(out NavigationPage npp) )
                {
                    foreach (var element in npp.GetNavigationElements())
                    {
                        element.OnClick -= OnNavigationElementClick;
                    }
                }
            }
            
            if (activePage != null)
            {
                if (activePage.TryGetComponent(out CardPage c) )
                {
                    c.GetNavigationPageTransferButton().OnClick -= OnNavigationButtonClick;
                }
            }
            
            if (page.TryGetComponent(out CardPage cardPage))
            {
                cardPage.GetNavigationPageTransferButton().OnClick += OnNavigationButtonClick;
            }
            
            if (activePage != null)
            {
                collectionPageArrows = activePage.GetCollectionPageArrows();
            
                foreach (var arrow in collectionPageArrows)
                {
                    arrow.OnCollectionPageArrowClicked -= OnCollectionPageArrowClicked;
                }
            }
            
            activePage = page;
            
            collectionPageArrows = activePage.GetCollectionPageArrows();
            
            foreach (var arrow in collectionPageArrows)
            {
                arrow.OnCollectionPageArrowClicked += OnCollectionPageArrowClicked;
            }
            
            OnActivePageChanged?.Invoke(this,page.GetSubTitle().Name);
        }

        private void OnNavigationElementClick(object sender, CardPage e)
        {
            GoPageInstant(e);
        }

        private void OnNavigationButtonClick(object sender, EventArgs e)
        {
            GoNavigationPageInstant();
        }

        private void GoPageInstant(Page targetPage)
        {
            StartCoroutine(InstantRoutine(targetPage));
        }
        
        private IEnumerator InstantRoutine(Page targetPage)
        {
            for (int i = 0; i < allPages.Count; i++)
            {
                if (targetPage != allPages[i])
                {
                    allPages[i].GetVisual().TurnPageLeft();
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    break;
                }
            }
                
            SetActivePage(targetPage);
        }
        
        private void GoNavigationPageInstant()
        {
            StartCoroutine(InnerRoutine());

            IEnumerator InnerRoutine()
            {
                for (var i = allPages.Count-1; i >= 0; i--)
                {
                    var page = allPages[i];
                    if (!page.GetVisual().GetIsOriginalPos())
                    {
                        page.GetVisual().TurnPageRight();
                        yield return new WaitForSeconds(.05f);
                    }
                }

                SetActivePage(allPages[0]);
            }
        }

        private void OnCollectionPageArrowClicked(object sender, bool e)
        {
            if (e)
            { 
                if (!CanPagesTurnBack()) return;
                
                GetPrivPage(activePage).GetVisual().TurnPageRight();
                SetActivePage(GetPrivPage(activePage));
                
                return;
            }
            
            if (!CanPagesTurnForward()) return;
            activePage.GetVisual().TurnPageLeft();
            SetActivePage(GetNextPage(activePage));
        }
        
        private void OnPagePointChanged(object sender, EventArgs e)
        {
            bookPoint = 0;
            
            foreach (var page in cardPages)
            {
                bookPoint += page.GetPagePoint();
            }

            playerPointText.text = bookPoint.ToString();
        }
        
        private bool CanPagesTurnBack()
        {
            return activePage != allPages[0];
        }

        private bool CanPagesTurnForward()
        {
            return activePage != allPages[^1];
        }

        private Page GetNextPage(Page p)
        {
            for (int i = 0; i < allPages.Count; i++)
            {
                if (allPages[i] == p)
                {
                    return allPages[i + 1];
                }
            }

            return null;
        }
        
        private Page GetPrivPage(Page p)
        {
            for (int i = 0; i < allPages.Count; i++)
            {
                if (allPages[i] == p)
                {
                    return allPages[i - 1];
                }
            }

            return null;
        }

        public List<CardPage> GetAllCardPages()
        {
            return cardPages;
        }
        
        public Page GetActivePage()
        {
            return activePage;
        }

        public int GetBookPoint()
        {
            return bookPoint;
        }
        
        


    }
}