using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Web3Hackathon
{
    public class OwnNFTView: MonoBehaviour
    {
        [SerializeField] private Transform _nftListRoot;
        public Transform NFTListRoot => _nftListRoot;
        [SerializeField] private Button _toUnOwnNFTButton;
        public IObservable<Unit> ToUnOwnObservable => _toUnOwnNFTButton.OnClickAsObservable();
        
        public void Initialize()
        {
            
        }
        
        public void ClearList()
        {
            foreach (Transform child in _nftListRoot)
            {
                Destroy(child.gameObject);
            }
        }
    }
}