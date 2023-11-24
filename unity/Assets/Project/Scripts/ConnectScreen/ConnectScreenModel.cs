using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Web3Hackathon.ScreenSystem;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Web3Hackathon
{
    public class ConnectScreenModel
    {
        public void Connect()
        {
            WalletData.Instance.Connect();
        }
    }
}