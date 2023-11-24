using Common;
using UnityEngine;

namespace Web3Hackathon
{
    public class CoinPresenter: MonoBehaviour
    {
        [SerializeField] private CoinView _view;
        [SerializeField] private CoinModel _model;

        public void Initialize()
        {
            _model.Initialize();
            _view.Initialize();
        }

        public void PlaceCoins()
        {
            _model.PlaceCoins();
        }
    }
}