using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Web3Hackathon
{
    public class UnownNFTView: MonoBehaviour
    {
        [SerializeField] private Transform _nftListRoot;
        public Transform NFTListRoot => _nftListRoot;
        [SerializeField] private Button _toOwnNFTButton;
        public IObservable<Unit> ToOwnObservable => _toOwnNFTButton.OnClickAsObservable();
        [SerializeField] private Image _laodingImage;
        
        public void DisplayLoading()
        {
            _laodingImage.gameObject.SetActive(true);
        }
        
        public void HideLoading()
        {
            _laodingImage.gameObject.SetActive(false);
        }
        
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