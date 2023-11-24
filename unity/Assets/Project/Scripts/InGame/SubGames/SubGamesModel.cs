using System.Collections.Generic;
using UnityEngine;

namespace Web3Hackathon
{
    public class SubGamesModel: MonoBehaviour
    {
        [SerializeField] private List<BaseSubGameItem> _subGamesPrefabList;
        public List<BaseSubGameItem> SubGamesPrefabList => _subGamesPrefabList;
        
        public void Initialize()
        {
        }
    }
}