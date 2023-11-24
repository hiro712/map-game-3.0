using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Web3Hackathon.ScreenSystem
{
    public abstract class BaseScreen: MonoBehaviour
    {
        public async UniTask Initialize(CancellationToken token)
        {
            await InitializeOverride(token);
        }

        public async UniTask Dispose(CancellationToken token)
        {
            await DisposeOverride(token);
        }

        public void Open()
        {
            OpenOverride();
        }

        public void Close()
        {
            CloseOverride();
        }
        

        public virtual async UniTask StartTransitionAnimation(CancellationToken token)
        {
            await TransitionAnimationImage.Instance.FadeIn(token);
        }

        public virtual async UniTask EndTransitionAnimation(CancellationToken token)
        {
            await TransitionAnimationImage.Instance.FadeOut(token);
        }

        protected virtual UniTask InitializeOverride(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
        
        protected virtual UniTask DisposeOverride(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
        
        protected virtual void OpenOverride()
        {
        }
        
        protected virtual void CloseOverride()
        {
        }
    }
}