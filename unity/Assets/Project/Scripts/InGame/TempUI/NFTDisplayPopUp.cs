using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

namespace Web3Hackathon
{
    public class NFTDisplayPopUp : MonoBehaviour
    {
        [SerializeField] private RawImage _targetImage;
        private string _uri;
        [SerializeField] private Image _popUp;
        [SerializeField] private Image _loadingBG;
        [SerializeField] private Button _closeButton;

        public void Initialize()
        {
            Close();
            _closeButton.OnClickAsObservable().Subscribe(_ => { Close(); });
        }

        public async UniTask SetUri(string uri)
        {
            _uri = uri;
            await GetTexture(CancellationToken.None);
        }

        public void OpenLoading()
        {
            _loadingBG.gameObject.SetActive(true);
        }

        private void Close()
        {
            _popUp.gameObject.SetActive(false);
            _loadingBG.gameObject.SetActive(false);
        }

        public void OpenPopUp()
        {
            _popUp.gameObject.SetActive(true);
            _loadingBG.gameObject.SetActive(true);
            GetTexture(CancellationToken.None).Forget();
        }

        private async UniTask GetTexture(CancellationToken cancellationToken)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_uri);
            await www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                _targetImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }
    }
}