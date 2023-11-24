using UnityEngine;

namespace Web3Hackathon
{
    public class SubGamesView: MonoBehaviour
    {
        [SerializeField] private Transform _subGamesRoot;
        public Transform SubGamesRoot => _subGamesRoot;
        
        public void Initialize()
        {
            
        }
    }
}