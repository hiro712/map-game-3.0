using System.Collections.Generic;
using System.Text;
using Common;
using Cysharp.Threading.Tasks;
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using Niantic.Lightship.Maps.ObjectPools;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Hackathon
{
    public class FramePresenter : SingletonMonoBehaviour<FramePresenter>
    {
        [SerializeField] private FrameView _view;
        [SerializeField] private FrameModel _model;
        
        public void Initialize()
        {
            _model.Initialize();
            _view.Initialize();
        }
        
        public void PlaceFrames()
        {
            _model.PlaceFrames();
        }
    }
}