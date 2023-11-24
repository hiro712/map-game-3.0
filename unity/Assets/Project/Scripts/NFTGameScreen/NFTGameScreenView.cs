using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Web3Hackathon
{
    public class NFTGameScreenView: MonoBehaviour
    {
        [SerializeField] private Button _backToGameButton;
        public IObservable<Unit> BackToGameObservable => _backToGameButton.OnClickAsObservable();
    }
}