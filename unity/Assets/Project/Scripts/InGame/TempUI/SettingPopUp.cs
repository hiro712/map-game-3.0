using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Web3Hackathon
{
    public class SettingPopUp: MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private GameObject popUp;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button setButton;
        [SerializeField] private TMP_Text _currentURLText;
        
        public void Initialize()
        {
            closeButton.OnClickAsObservable().Subscribe(_=>Close()).AddTo(gameObject);
            setButton.OnClickAsObservable().Subscribe(_ => SetText()).AddTo(gameObject);
            APIController.Instance.BaseURL.Subscribe(text =>
            {
                _currentURLText.text = text;
            });
            Close();
        }

        public void SetText()
        {
            APIController.Instance.SetBaseURL(inputField.text);
        }
        
        public void Show()
        {
            popUp.SetActive(true);
        }

        public void Close()
        {
            popUp.SetActive(false);
        }
    }
}