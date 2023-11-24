using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Web3Hackathon.ScreenSystem;

namespace Web3Hackathon
{
    public class NFTGameItem: BaseSubGameItem
    {
        [SerializeField] private Button _button;
        public override void Initialize()
        {
            _button.OnClickAsObservable().Subscribe(_ =>
            {
                ScreenManager.Instance.ChangeScreen(ScreenEnum.NFTGame).Forget();
            });
            _button.OnClickAsObservable().Subscribe(_ =>
            {
                MoveDetail();
            });
        }

        public void MoveDetail()
        {
            ScreenManager.Instance.ChangeScreen(ScreenEnum.NFTGame).Forget();
        }
    }
}