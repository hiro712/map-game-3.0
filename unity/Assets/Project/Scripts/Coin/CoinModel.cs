using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;
using Niantic.Lightship.Maps.ObjectPools;
using UniRx;

namespace Web3Hackathon
{
    public class CoinModel: MonoBehaviour
    {
        [SerializeField] private LayerGameObjectPlacement _objectSpawner;
        [SerializeField] private Camera _camera;
        private Dictionary<int, PooledObject<GameObject>> _coinList = new Dictionary<int, PooledObject<GameObject>>();
        private Subject<int> _onCoinCollision = new Subject<int>();
        private List<Vector2> _positions = new List<Vector2>()
        {
            new Vector2(35.594526569045314f, 139.69127600936415f),
            new Vector2(35.595062823074365f, 139.69092577315388f),
            new Vector2(35.59541031769926f, 139.6904416722314f),
            new Vector2(35.595746951680354f, 139.69004771424216f),
            new Vector2(35.60016483965924f, 139.69131582008967f),
            new Vector2(35.60253662272225f, 139.68786177157003f),
            
        };

        public void Initialize()
        {
        }
        
        public void PlaceCoins()
        {
            int count = 0;
            foreach (Vector2 position in _positions)
            {
                var location = new LatLng(position.x, position.y);
                PlaceCoin(location, count);
                count++;
            }
            _onCoinCollision.Subscribe(id => 
            {
                OnCoinGet(id).Forget();
            });
        }
        
        private void PlaceCoin(LatLng location, int index)
        {
            var cameraForward = _camera.transform.forward;
            var forward = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            var rotation = Quaternion.LookRotation(forward);
            var pooledObject = _objectSpawner.PlaceInstance(location, rotation, "coin" + index.ToString());
            pooledObject.Value.GetComponent<CoinElement>().SetId(index);
            // _coinListに追加
            _coinList.Add(index, pooledObject);
            // subjectの登録
            pooledObject.Value.GetComponent<CoinElement>().OnCoinCollision.Subscribe(id =>
            {
                _onCoinCollision.OnNext(id);
            });
        }

        private void RemoveCoin(int id)
        {
            var target = _coinList[id].Value;
            target.SetActive(false);
        }

        private async UniTask OnCoinGet(int id)
        {
            Debug.Log("Get Coin, id: " + id.ToString());
            await _coinList[id].Value.GetComponent<CoinElement>()
                .AnimationWhenGot(this.GetCancellationTokenOnDestroy(), () => RemoveCoin(id));
            await WalletData.Instance.MintERC20(10f);
            WalletData.Instance.AddBalance(10f);
        }
    }
}