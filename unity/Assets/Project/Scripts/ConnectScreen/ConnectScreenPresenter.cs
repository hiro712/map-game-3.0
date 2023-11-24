using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Web3Hackathon.ScreenSystem;

namespace Web3Hackathon
{
    public class ConnectScreenPresenter: MonoBehaviour
    {
        [SerializeField] private ConnectScreenView view;
        private ConnectScreenModel _model;
        
        public void Initialize()
        {
            _model = new ConnectScreenModel();
            view.OnConnectButtonClicked.Subscribe(_ =>
            {
                Debug.Log("model connect");
                _model.Connect();
            }).AddTo(gameObject);
        }
    }
}