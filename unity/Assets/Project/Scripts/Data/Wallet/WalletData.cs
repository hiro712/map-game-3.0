using System;
using Common;
using Cysharp.Threading.Tasks;
using Thirdweb;
using UniRx;
using UnityEngine;
using Web3Hackathon.ScreenSystem;

namespace Web3Hackathon
{
    public class WalletData : SingletonMonoBehaviour<WalletData>
    {
        private ReactiveProperty<string> _walletAddress = new ReactiveProperty<string>();
        public IReadOnlyReactiveProperty<string> WalletAddress => _walletAddress;
        private ReactiveProperty<float> _balance = new ReactiveProperty<float>(0);
        public IReadOnlyReactiveProperty<float> Balance => _balance;
        private Subject<Unit> _onConnected = new Subject<Unit>();
        public IObservable<Unit> OnConnected => _onConnected;
        private ThirdwebSDK _sdk;
        private const string _erc721ContractAddress = "0xFF96AE4dD04eEC86565C398AAffB87ec5b5e8524";
        private const string _erc20ContractAddress = "0x198c7908ED993e8dB33E3b33bF10376Bb1251b77";
        private Contract _erc721Contract;
        private Contract _erc20Contract;

        protected override void Awake()
        {
            base.Awake();
            // Initialize();
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // ThirdwebManager.Instance.Initialize("zKatana");
            _sdk = ThirdwebManager.Instance.SDK;
            // _erc721Contract = _sdk.GetContract(_erc721ContractAddress);
            // _erc20Contract = _sdk.GetContract(_erc20ContractAddress);
            Debug.Log("WalletData initialized");
        }

        public async UniTask Connect()
        {
            var connection = new WalletConnection(
                provider: WalletProvider.Metamask,
                chainId: 1261120
            );
            var address = await _sdk.wallet.Connect(connection);
            ChangeAddress(address);
            var data = await _sdk.wallet.GetBalance();
            // data.displayValueはstring型なのでfloat型に変換する
            var balance = float.Parse(data.displayValue);
            AddBalance(balance);
            ScreenManager.Instance.ChangeScreen(ScreenEnum.InGame).Forget();
            _onConnected.OnNext(Unit.Default);
        }

        public void AddBalance(float amount)
        {
            var balance = _balance.Value + amount;
            _balance.Value = balance;
        }

        public void ChangeAddress(string address)
        {
            _walletAddress.Value = address;
        }

        public async UniTask MintERC721(string name, string imagePath)
        {
            _erc721Contract = _sdk.GetContract(_erc721ContractAddress);
            await _erc721Contract.ERC721.Mint(new NFTMetadata()
            {
                name = name,
                image = imagePath
            });
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            AddBalance(-0.3f);
        }

        public async UniTask MintERC20(float amount)
        {
            _erc20Contract = _sdk.GetContract(_erc20ContractAddress);
            await _erc20Contract.ERC20.Mint(amount.ToString());
            var data = await _sdk.wallet.GetBalance();
            // data.displayValueはstring型なのでfloat型に変換する
            var balance = float.Parse(data.displayValue);
            _balance.Value = balance;
        }
    }
}