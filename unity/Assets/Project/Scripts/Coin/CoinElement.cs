using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Web3Hackathon
{
    public class CoinElement : MonoBehaviour
    {
        [SerializeField] private GameObject _coin;
        private int _id;
        private Subject<int> _onCoinCollision = new Subject<int>();
        public IObservable<int> OnCoinCollision => _onCoinCollision;
        private CancellationTokenSource _cts;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Coin Collision");
            _onCoinCollision.OnNext(_id);
        }

        public void SetId(int id)
        {
            _id = id;
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            Rotate().Forget();
        }

        private async UniTask Rotate()
        {
            await _coin.transform.DOLocalRotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1).ToUniTask(TweenCancelBehaviour.Kill, _cts.Token);
        }
        
        public async UniTask AnimationWhenGot(CancellationToken token, Action removeAction)
        {
            Debug.Log("AnimationWhenGot, id: " + _id.ToString());
            _cts.Cancel();
            // _coin.transform.DOLocalMove(new Vector3(0f, 20f, 0f), 1f).SetEase(Ease.Linear);
            var sequence = DOTween.Sequence()
                .Join(_coin.transform.DOLocalMove(new Vector3(0f, 20f, 0f), 0.5f).SetEase(Ease.OutQuart))
                .Join(_coin.transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 0.3f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear).SetLoops(2))
                .InsertCallback(0.5f, () => removeAction());
            await sequence.Play().ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}