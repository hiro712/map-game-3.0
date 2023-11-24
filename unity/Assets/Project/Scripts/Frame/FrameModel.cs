using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using Niantic.Lightship.Maps.ObjectPools;
using UniRx;
using UnityEngine;

namespace Web3Hackathon
{
    public class FrameModel: MonoBehaviour
    {
        [SerializeField] private LayerGameObjectPlacement _objectSpawner;
        [SerializeField] private Camera _camera;
        private Dictionary<int, PooledObject<GameObject>> _frameList = new Dictionary<int, PooledObject<GameObject>>();
        private Subject<int> _onFrameCollision = new Subject<int>();
        private string _urlBase = "http://localhost:3000";
        private List<Vector2> _positions = new List<Vector2>()
        {
            new Vector2(35.59725935544604f, 139.6885613427088f),
            new Vector2(35.594922574019556f, 139.69257495614315f),
            new Vector2(35.59527565105857f, 139.69375264468712f),
            new Vector2(35.592901068747686f, 139.69271148275072f),
            new Vector2(35.59917372221579f, 139.68581990563894f),
            new Vector2(35.59712999012987f, 139.6830061947811f)
        };

        public void Initialize()
        {
        }

        public void PlaceFrames()
        {
            int count = 0;
            foreach (var position in _positions)
            {
                var location = new LatLng(position.x, position.y);
                PlaceFrame(location, count);
                count++;
            }

            _onFrameCollision.Subscribe(id =>
            {
                OnFrameGet(id).Forget();
                PooledObject<GameObject> frame = _frameList[id];
                var location = frame.Value.GetComponent<FrameElement>().Location;
                GeneratingTask(location).Forget();
            });
        }

        private void PlaceFrame(LatLng location, int index)
        {
            var cameraForward = _camera.transform.forward;
            var forward = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            var rotation = Quaternion.LookRotation(forward);
            var pooledObject = _objectSpawner.PlaceInstance(location, rotation, "frame" + index.ToString());
            pooledObject.Value.GetComponent<FrameElement>().SetId(index);
            pooledObject.Value.GetComponent<FrameElement>()
                .SetLocation((float)location.Latitude, (float)location.Longitude);
            // _frameListに追加
            _frameList.Add(index, pooledObject);
            // subjectの登録
            pooledObject.Value.GetComponent<FrameElement>().OnFrameCollision.Subscribe(id =>
            {
                _onFrameCollision.OnNext(id);
            });
        }

        public void RemoveFrame(int id)
        {
            var target = _frameList[id].Value;
            target.SetActive(false);
        }

        private async UniTask OnFrameGet(int id)
        {
            Debug.Log("FrameGot");
            await _frameList[id].Value.GetComponent<FrameElement>()
                .AnimationWhenGot(this.GetCancellationTokenOnDestroy(), () => RemoveFrame(id));
        }

        private async UniTask GeneratingTask(List<float> location)
        {
            TempUIController.Instance.NFTDisplayPopUp.OpenLoading();
            var res1 = await APIController.Instance.UploadImageData(WalletData.Instance.WalletAddress.Value, location[0], location[1], 0);
            // List<ImageDat> res1の、idが最大のものを取得
            res1.Sort((a, b) => b.id - a.id);
            var data = res1[0];
            var res2 = await APIController.Instance.GenerateImage(location[0], location[1]);
            Debug.Log("image path: " + res2.image);
            var path = res2.image;
            if (path == null)
            {
                Debug.Log("path is null");
                return;
            }
            else
            {
                await TempUIController.Instance.NFTDisplayPopUp.SetUri(path);
                await APIController.Instance.UpdateImageData(data.id, WalletData.Instance.WalletAddress.Value, 1,
                    path);
                TempUIController.Instance.NFTDisplayPopUp.OpenPopUp();
                Debug.Log("path: " + path);
            }
        }
    }
}