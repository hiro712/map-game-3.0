using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Web3Hackathon
{
    public class UnOwnNFTElement: MonoBehaviour
    {
        [SerializeField] private RawImage _nftImage;
        private string _nftImageURL;
        private int _id;
        [SerializeField] private Button _mintButton;
        private Subject<Unit> _mintSubject = new Subject<Unit>();
        public IObservable<Unit> MintObservable => _mintSubject;
        private ReactiveProperty<bool> isMinting = new ReactiveProperty<bool>(false);
        public IObservable<bool> IsMintingObservable => isMinting;

        public void Initialize(int id, string nftImageURL)
        {
            _id = id;
            _nftImageURL = nftImageURL;
            GetTexture(this.GetCancellationTokenOnDestroy()).Forget();
            ObservableExtensions.Subscribe(_mintButton.OnClickAsObservable(), _ =>
            {
                MintERC721();
            });
        }

        private async UniTask GetTexture(CancellationToken cancellationToken)
        {
            Debug.Log("GetTexture called, url : " + _nftImageURL + ", and id " + _id);
            if (!_nftImageURL.StartsWith("http")) return;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_nftImageURL);
            await www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                _nftImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }

        private async UniTask MintERC721()
        {
            isMinting.Value = true;
            await WalletData.Instance.MintERC721(_id.ToString(), _nftImageURL);
            await APIController.Instance.UpdateImageData(_id, WalletData.Instance.WalletAddress.Value, 2,
                _nftImageURL);
            _mintSubject.OnNext(Unit.Default);
            isMinting.Value = false;
        }
    }
}