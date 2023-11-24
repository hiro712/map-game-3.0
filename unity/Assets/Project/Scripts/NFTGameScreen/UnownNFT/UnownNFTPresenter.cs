using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Web3Hackathon
{
    public class UnownNFTPresenter: MonoBehaviour
    {
        [SerializeField] private UnownNFTView _view;
        private UnownNFTModel _model;
        private Subject<Unit> _toOwnSubject = new Subject<Unit>();
        public IObservable<Unit> ToOwnObservable => _toOwnSubject;
        [SerializeField] private UnOwnNFTElement _unOwnNFTElementPrefab;
        private Subject<Unit> _updateListSubject = new Subject<Unit>();
        public IObservable<Unit> UpdateListObservable => _updateListSubject;
        private ReactiveProperty<bool> _isLoading = new ReactiveProperty<bool>(false);
        
        public void Initialize()
        {
            _model = new UnownNFTModel();
            _view.Initialize();
            ObservableExtensions.Subscribe(_view.ToOwnObservable, _ => _toOwnSubject.OnNext(Unit.Default));
            MakeList().Forget();
            ObservableExtensions.Subscribe(_isLoading, isLoading =>
            {
                if (isLoading)
                {
                    _view.DisplayLoading();
                }
                else
                {
                    _view.HideLoading();
                }
            });
        }
        
        public async UniTask MakeList()
        {
            await _model.UpdateList();
            _view.ClearList();
            _model.UnOwnImageDataList.ForEach(imageData =>
            {
                UnOwnNFTElement unOwnNFTElement = Instantiate(_unOwnNFTElementPrefab, _view.NFTListRoot);
                unOwnNFTElement.Initialize(imageData.id, imageData.image);
                ObservableExtensions.Subscribe(unOwnNFTElement.MintObservable, _ =>
                {
                    _updateListSubject.OnNext(Unit.Default);
                });
                ObservableExtensions.Subscribe(unOwnNFTElement.IsMintingObservable, isMinting =>
                {
                    _isLoading.Value = isMinting;
                });
            });
        }
    }
}