using UniRx;
using UnityEngine;

namespace Web3Hackathon
{
    public class StatusModel
    {
        private ReactiveProperty<string> _displayAddress = new ReactiveProperty<string>();
        public IReadOnlyReactiveProperty<string> DisplayAddress => _displayAddress;
        private ReactiveProperty<string> _displayBalance = new ReactiveProperty<string>();
        public IReadOnlyReactiveProperty<string> DisplayBalance => _displayBalance;

        public StatusModel()
        {
            WalletData.Instance.WalletAddress.Subscribe(address =>
            {
                _displayAddress.Value = TruncateStringWithDots(address);
            }).AddTo(WalletData.Instance);
            WalletData.Instance.Balance.Subscribe(balance =>
            {
                _displayBalance.Value = balance.ToString();
            }).AddTo(WalletData.Instance);
        }
        
        private string TruncateStringWithDots(string input)
        {
            if (input == null || input.Length <= 8)
            {
                return input; // 文字列が8文字以下の場合は変更なし
            }

            string firstFour = input.Substring(0, 4); // 最初の4文字
            string lastFour = input.Substring(input.Length - 4); // 最後の4文字

            return firstFour + "..." + lastFour; // 結合
        }
    }
}