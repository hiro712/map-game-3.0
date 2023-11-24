using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Web3Hackathon.ScreenSystem
{
    public class TransitionAnimationImage: SingletonMonoBehaviour<TransitionAnimationImage>
    {
        [SerializeField] private GameObject image;
        [SerializeField] private Canvas canvas;

        protected override void Awake()
        {
            // imageのalpha値を0にする
            var imageColor = image.GetComponent<Image>().color;
            image.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
            image.SetActive(false);
        }

        public async UniTask FadeIn(CancellationToken token)
        {
            image.SetActive(true);
            await image.GetComponent<Image>().DOFade(1, 1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public async UniTask FadeOut(CancellationToken token)
        {
            await image.GetComponent<Image>().DOFade(0, 1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
            image.SetActive(false);
        }
    }
}