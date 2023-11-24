using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Web3Hackathon.ScreenSystem
{
    public class ScreenManager: SingletonMonoBehaviour<ScreenManager>
    {
        // TODO: Add all screens here
        [Header("Screen Prefabs")]
        [SerializeField] private BaseScreen initializeScreenPrefab;
        [SerializeField] private BaseScreen connectScreenPrefab;
        [SerializeField] private BaseScreen inGameScreenPrefab;
        [SerializeField] private BaseScreen nftGameScreenPrefab;
        [Header("Screen to start with")]
        [Tooltip("Select screen to start with, when game starts.")]
        [SerializeField] private ScreenEnum startScreenEnum;
        [Header("place to instantiate screens")]
        [SerializeField] private Transform screenRoot;
        
        private BaseScreen _currentScreen;
        private bool _isTransitioning;

        private void Start()
        {
            ChangeScreen(startScreenEnum).Forget();
        }

        public async UniTask ChangeScreen(ScreenEnum newScreenEnum)
        {
            if (_isTransitioning) return;
            _isTransitioning = true;
            var token = this.GetCancellationTokenOnDestroy();
            if (_currentScreen != null)
            {
                _currentScreen.Close();
                await _currentScreen.StartTransitionAnimation(token);
                await _currentScreen.Dispose(token);
                Destroy(_currentScreen.gameObject);
            }
            
            BaseScreen newScreen = null;
            var resultGettingScreenPrefab = GetScreenPrefabFromEnum(newScreenEnum, out var screenPrefab);
            if (resultGettingScreenPrefab)
            {
                var nextScreenObject = Instantiate(screenPrefab, screenRoot);
                if (!nextScreenObject.TryGetComponent(out newScreen))
                {
                    Debug.LogError("ScreenPrefab does not have BaseScreen component");
                }
            }

            if (newScreen != null)
            {
                _currentScreen = newScreen;
                var newScreenCancellationToken = newScreen.GetCancellationTokenOnDestroy();
                var newScreenCancellationTokens = CancellationTokenSource.CreateLinkedTokenSource(token, newScreenCancellationToken);
                await newScreen.Initialize(newScreenCancellationTokens.Token);
                await newScreen.EndTransitionAnimation(newScreenCancellationTokens.Token);
                newScreen.Open();
            }
            _isTransitioning = false;
        }
        
        private bool GetScreenPrefabFromEnum(ScreenEnum screenEnum, out BaseScreen screenPrefab)
        {
            screenPrefab = null;
            switch (screenEnum)
            {
                case ScreenEnum.Initialize:
                    screenPrefab = initializeScreenPrefab;
                    break;
                case ScreenEnum.Connect:
                    screenPrefab = connectScreenPrefab;
                    break;
                case ScreenEnum.InGame:
                    screenPrefab = inGameScreenPrefab;
                    break;
                case ScreenEnum.NFTGame:
                    screenPrefab = nftGameScreenPrefab;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}