using TMPro;
using UnityEngine;

namespace Web3Hackathon
{
    public class StatusView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _addressText;
        [SerializeField] private TMP_Text _balanceText;
        
        public void SetAddressText(string address)
        {
            _addressText.text = address;
        }
        
        public void SetBalanceText(string balance)
        {
            _balanceText.text = balance;
        }
    }
}