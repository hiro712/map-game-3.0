using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Web3Hackathon
{
    public class FrameElement : MonoBehaviour
    {
        [SerializeField] private GameObject _frame;
        private int _id;
        private Subject<int> _onFrameCollision = new Subject<int>();
        public IObservable<int> OnFrameCollision => _onFrameCollision;
        private CancellationTokenSource _cts;
        private List<float> _location = new List<float>();
        public List<float> Location => _location;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Frame Collision");
            _onFrameCollision.OnNext(_id);
        }

        public void SetId(int id)
        {
            _id = id;
        }
        
        public void SetLocation(float latitude, float longitude)
        {
            _location.Add(latitude);
            _location.Add(longitude);
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            Lotate().Forget();
        }

        private async UniTask Lotate()
        {
            await _frame.transform.DOLocalRotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1).ToUniTask(TweenCancelBehaviour.Kill, _cts.Token);
        }
        
        public async UniTask AnimationWhenGot(CancellationToken token, Action removeAction)
        {
            Debug.Log("AnimationWhenGot, id: " + _id.ToString());
            _cts.Cancel();
            // _coin.transform.DOLocalMove(new Vector3(0f, 20f, 0f), 1f).SetEase(Ease.Linear);
            var sequence = DOTween.Sequence()
                .Join(_frame.transform.DOLocalMove(new Vector3(0f, 20f, 0f), 0.5f).SetEase(Ease.OutQuart))
                .Join(_frame.transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 0.3f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear).SetLoops(2))
                .InsertCallback(0.5f, () => removeAction());
            await sequence.Play().ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}