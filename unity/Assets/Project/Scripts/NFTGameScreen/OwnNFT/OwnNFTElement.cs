using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Web3Hackathon
{
    public class OwnNFTElement: MonoBehaviour
    {
        [SerializeField] private RawImage _nftImage;
        private string _nftImageURL;
        private int _id;
        
        public void Initialize(int id, string nftImageURL)
        {
            _id = id;
            _nftImageURL = nftImageURL;
            GetTexture(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        private async UniTask GetTexture(CancellationToken cancellationToken)
        {
            Debug.Log("GetTexture callec, url: " + _nftImageURL);
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
    }
}