using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Data.NFT;
using UnityEngine;

namespace Web3Hackathon
{
    public class UnownNFTModel
    {
        private List<ImageData> _unOwnImageDataList = new List<ImageData>();
        public List<ImageData> UnOwnImageDataList => _unOwnImageDataList;
        
        public UnownNFTModel()
        {
            _unOwnImageDataList.Clear();
        }
        
        public async UniTask UpdateList()
        {
            _unOwnImageDataList.Clear();
            var data = await APIController.Instance.GetImageDataList(WalletData.Instance.WalletAddress.Value);
            // List<ImageData> data から、status == 1 のものを抽出して、_ownImageDataList に格納する
            foreach (var imageData in data)
            {
                if (imageData.status == 1)
                {
                    _unOwnImageDataList.Add(imageData);
                }
            }
            // _ownImageDataList の要素数をログに出力する
            Debug.Log("OwnImageDataList Count: " + _unOwnImageDataList.Count.ToString());
            // _ownImageDataListをidでソートする
            _unOwnImageDataList.Sort((a, b) => a.id - b.id);
        }
    }
}