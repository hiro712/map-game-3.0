using UnityEngine;
using UniRx;

namespace Web3Hackathon
{
    public class StatusPresenter: MonoBehaviour
    {
        [SerializeField] private StatusView _view;
        private StatusModel _model;
        
        public void Initialize()
        {
            _model = new StatusModel();
            _model.DisplayAddress.Subscribe(address =>
            {
                _view.SetAddressText(address);
            }).AddTo(gameObject);
            _model.DisplayBalance.Subscribe(balance =>
            {
                _view.SetBalanceText(balance);
            }).AddTo(gameObject);
        }
    }
}