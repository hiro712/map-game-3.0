using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Web3Hackathon.ScreenSystem;
using UniRx;

namespace Web3Hackathon
{
    public class InGamePresenter: MonoBehaviour
    {
        [SerializeField] private InGameView _view;
        private InGameModel _model;
        [SerializeField] private StatusPresenter _statusController;
        [SerializeField] private CoinPresenter _coinController;
        [SerializeField] private FramePresenter _frameController;
        [SerializeField] private SubGamesPresenter _subGamesController;

        private async void Start()
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            Initialize();
        }
        
        private void Initialize()
        {
            _statusController.Initialize();
            _coinController.Initialize();
            _frameController.Initialize();
            _subGamesController.Initialize();
            WalletData.Instance.OnConnected.Subscribe(_ =>
            {
                PlaceCoinsAndFrames();
            });
        }

        private void PlaceCoinsAndFrames()
        {
            _coinController.PlaceCoins();
            _frameController.PlaceFrames();
        }
    }
}