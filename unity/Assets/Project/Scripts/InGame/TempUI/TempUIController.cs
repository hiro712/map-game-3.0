using Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Web3Hackathon
{
    public class TempUIController: SingletonMonoBehaviour<TempUIController>
    {
        [SerializeField] private Button _settingButton;
        [SerializeField] private NFTDisplayPopUp _nftDisplayPopUp;
        public NFTDisplayPopUp NFTDisplayPopUp => _nftDisplayPopUp;
        [SerializeField] private SettingPopUp _settingPopUp;
        public SettingPopUp SettingPopUp => _settingPopUp;
        
        private void Start()
        {
            _settingButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingPopUp.Show();
            });
            _nftDisplayPopUp.Initialize();
            _settingPopUp.Initialize();
        }
    }
}