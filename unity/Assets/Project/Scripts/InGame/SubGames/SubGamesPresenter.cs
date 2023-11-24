using UnityEngine;

namespace Web3Hackathon
{
    public class SubGamesPresenter: MonoBehaviour
    {
        [SerializeField] private SubGamesView _view;
        [SerializeField] private SubGamesModel _model;
        
        public void Initialize()
        {   
            _view.Initialize();
            _model.Initialize();
            AddSubGames();
        }

        public void AddSubGames()
        {
            Debug.Log("AddSubGames");
            foreach (var subGamePrefab in _model.SubGamesPrefabList)
            {
                var instance = Instantiate(subGamePrefab, _view.SubGamesRoot);
                instance.GetComponent<BaseSubGameItem>().Initialize();
            }
        }
    }
}