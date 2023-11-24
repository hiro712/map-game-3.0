using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Web3Hackathon
{
    public class OwnNFTPresenter: MonoBehaviour
    {
        [SerializeField] private OwnNFTView _view;
        private OwnNFTModel _model;
        private Subject<Unit> _toUnOwnSubject = new Subject<Unit>();
        public IObservable<Unit> ToUnOwnObservable => _toUnOwnSubject;
        [SerializeField] private OwnNFTElement _ownNFTElementPrefab;
        
        public void Initialize()
        {
            _model = new OwnNFTModel();
            _view.Initialize();
            ObservableExtensions.Subscribe(_view.ToUnOwnObservable, _ => _toUnOwnSubject.OnNext(Unit.Default));
            MakeList().Forget();
        }

        public async UniTask MakeList()
        {
            await _model.UpdateList();
            _view.ClearList();
            _model.OwnImageDataList.ForEach(imageData =>
            {
                OwnNFTElement ownNFTElement = Instantiate(_ownNFTElementPrefab, _view.NFTListRoot);
                ownNFTElement.Initialize(imageData.id, imageData.image);
            });
        }
    }
}