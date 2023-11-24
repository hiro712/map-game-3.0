using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Web3Hackathon.ScreenSystem
{
    public class InitializeScreen: BaseScreen
    {
        [SerializeField] private InitializeScreenPresenter presenter;

        protected override UniTask InitializeOverride(CancellationToken token)
        {
            presenter.Initialize();
            return base.InitializeOverride(token);
        }
    }
}