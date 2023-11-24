using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Web3Hackathon
{
    public class ConnectScreenView: MonoBehaviour
    {
        [SerializeField] private Button connectButton;
        public IObservable<Unit> OnConnectButtonClicked => connectButton.OnClickAsObservable();
    }
}