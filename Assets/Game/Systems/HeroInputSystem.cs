﻿using Assets.Common.ECS;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Systems
{
    public class HeroInputSystem : ECSSystem, ISystemInit, ISystemUpdate
    {
        private IEnumerable<Entity> _filter;

        public void Init()
        {
            _filter = 
                World.Entities.Where(e => e.Components.Any(c => c is HeroMovingSystem));
        }

        public void Update()
        {

        }
    }
}