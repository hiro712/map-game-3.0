using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Data.NFT;
using UnityEngine;

namespace Web3Hackathon
{
    public class OwnNFTModel
    {
        private List<ImageData> _ownImageDataList = new List<ImageData>();
        public List<ImageData> OwnImageDataList => _ownImageDataList;
        
        public OwnNFTModel()
        {
            _ownImageDataList.Clear();
        }
        
        public async UniTask UpdateList()
        {
            _ownImageDataList.Clear();
            var data = await APIController.Instance.GetImageDataList(WalletData.Instance.WalletAddress.Value);
            // List<ImageData> data から、status == 2 のものを抽出して、_ownImageDataList に格納する
            foreach (var imageData in data)
            {
                if (imageData.status == 2)
                {
                    _ownImageDataList.Add(imageData);
                }
            }
            // _ownImageDataList の要素数をログに出力する
            Debug.Log("OwnImageDataList Count: " + _ownImageDataList.Count.ToString());
            // _ownImageDataListをidでソートする
            _ownImageDataList.Sort((a, b) => a.id - b.id);
        }
    }
}