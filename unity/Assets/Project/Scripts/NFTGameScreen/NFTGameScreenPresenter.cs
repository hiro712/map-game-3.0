using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Web3Hackathon
{
    public class NFTGameScreenPresenter: MonoBehaviour
    {
        [SerializeField] private NFTGameScreenView _view;
        private NFTGameScreenModel _model;
        [SerializeField] private OwnNFTPresenter _ownNFTPresenter;
        [SerializeField] private UnownNFTPresenter _unownNFTPresenter;

        private void Start()
        {
            _model = new NFTGameScreenModel();
            Initialize();
            ShowOwnNFT();
        }
        private void Initialize()
        {
            _ownNFTPresenter.Initialize();
            _unownNFTPresenter.Initialize();
            _view.BackToGameObservable.Subscribe(_ =>
            {
                BackToGame();
            });
            _ownNFTPresenter.ToUnOwnObservable.Subscribe(_ =>
            {
                ShowUnownNFT();
            });
            _unownNFTPresenter.ToOwnObservable.Subscribe(_ =>
            {
                ShowOwnNFT();
            });
            _unownNFTPresenter.UpdateListObservable.Subscribe(_ =>
            {
                _ownNFTPresenter.MakeList().Forget();
                _unownNFTPresenter.MakeList().Forget();
            });
        }
        private void BackToGame()
        {
            _model.BackToGame();
        }
        
        private void ShowOwnNFT()
        {
            _ownNFTPresenter.gameObject.SetActive(true);
            _unownNFTPresenter.gameObject.SetActive(false);
        }
        
        private void ShowUnownNFT()
        {
            _ownNFTPresenter.gameObject.SetActive(false);
            _unownNFTPresenter.gameObject.SetActive(true);
        }
    }
}