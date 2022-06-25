using Assets.Common.ECS;
using Assets.Common.Input;
using Assets.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Input = Assets.Common.Input.Input;

namespace Assets.Game.Systems
{
    public class HeroInputSystem : ECSSystem, ISystemInit, ISystemUpdate, IDisposable
    {
        private IEnumerable<Entity> _filter;
        private Input _input;
        private bool _isGameStarted = false;
        private SwipeInputEventArgs _lastSwipe;

        public HeroInputSystem(Input input)
        {
            _input = input;
        }

        public void Init()
        {
            _filter = 
                World.Entities.Where(e => e.Components.Any(c => c is HeroMovingComponent));
        }

        public void Update()
        {
            if (!_isGameStarted)
                GameStartingSetup();
            else
                InputUpdate();
        }

        private void GameStartingSetup()
        {
            var worldEntity = World.Entities.FirstOrDefault(e => e.Components.Any(c => c is GameRunningComponent));
            if(worldEntity != null)
            {
                _isGameStarted = true;
                _input.OnInput += Input_OnInput;
            }
        }

        private void Input_OnInput(object sender, InputEventArgs e)
        {
            if (e is SwipeInputEventArgs swipe)
            {
                foreach (var entity in _filter)
                {
                    var heroMovingComponent = entity.Components.FirstOrDefault(e => e is HeroMovingComponent) as HeroMovingComponent;
                    if (heroMovingComponent != null)
                    {
                        var direction = GetVectorFromSwipe(swipe);
                        var newPosition = heroMovingComponent.Transform.position + direction;
                        heroMovingComponent.TargetPosition = newPosition;
                    }
                }
            }
        }

        private void InputUpdate()
        {
            foreach (var entity in _filter)
            {
                var heroMovingComponent = entity.Components.FirstOrDefault(e => e is HeroMovingComponent) as HeroMovingComponent;
                if (heroMovingComponent != null)
                {
                    var newPosition
                        = Vector3.MoveTowards(heroMovingComponent.Transform.position, heroMovingComponent.TargetPosition, Time.fixedDeltaTime * 10);
                    heroMovingComponent.Transform.position = new Vector3(newPosition.x, heroMovingComponent.Transform.position.y, heroMovingComponent.Transform.position.z);
                }
            }
        }

        private Vector3 GetVectorFromSwipe(SwipeInputEventArgs swipe)
        {
            switch(swipe.Swipe)
            {
                case SwipeInputEventArgs.SwipeDirection.Left:
                    return Vector3.left;
                case SwipeInputEventArgs.SwipeDirection.Right:
                    return Vector3.right;
                default:
                    return Vector3.zero;
            }    
        }

        public void Dispose()
        {
            _isGameStarted = false;
            _input.OnInput -= Input_OnInput;
        }
    }
}
