using Assets.Common.ECS;
using Assets.Common.Input;
using Assets.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Input = Assets.Common.Input.Input;
using Object = UnityEngine.Object;

namespace Assets.Game.Systems
{
    public class GameStartingSystem : ECSSystem, ISystemInit
    {
        private Input _input;
        private IEnumerable<Entity> _filter;
        private GameObject _wrapEffectPrefab;
        private Transform _heroTransfrom;

        public GameStartingSystem(Input input, GameObject wrapEffectPrefab, Transform heroTransfrom)
        {
            _input = input;
            _wrapEffectPrefab = wrapEffectPrefab;
            _heroTransfrom = heroTransfrom;
        }

        public void Init()
        {
            _filter = World.Entities.Where(e => e.Components.Any(c => c is GameSplashScreenComponent));
            _input.OnInput += Input_OnInput;
        }

        private void Input_OnInput(object sender, InputEventArgs e)
        {
            if(e is TapInputEventArgs tap && tap.IsTapEnded)
            {
                _input.OnInput -= Input_OnInput;

                var entities = _filter.ToList();
                foreach (var entity in entities)
                {
                    var component = entity.Components.FirstOrDefault(c => c is GameSplashScreenComponent) as GameSplashScreenComponent;
                    if (component != null)
                    {
                        component.StartToGameSplashScreenController.IsVisible = false;
                        var worldEntity = World.CreateEntity(nameof(World));
                        worldEntity.AddComponent(new GameRunningComponent() { StartTime = DateTime.Now });

                        var wrapEffectObject = Object.Instantiate(_wrapEffectPrefab, _heroTransfrom);
                        wrapEffectObject.transform.localPosition = new Vector3(0, 0, 100);
                    }
                }
            }
        }
    }
}
