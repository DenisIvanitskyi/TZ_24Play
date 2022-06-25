
using System;
using System.Collections.Generic;

namespace Assets.Common.ECS
{
    public class World
    {
        private List<Entity> _entities;
        private List<ISystemInit> _systemsInit;
        private List<ISystemUpdate> _systemUpdate;
        private List<ISystemFixedUpdate> _systemsFixedUpdate;
        private List<ECSSystem> _systems;

        public IReadOnlyList<Entity> Entities => _entities;

        public World()
        {
            _entities = new List<Entity>();
        }

        public virtual void Init()
        {
            if (_systemsInit == null) return;

            for (var i = 0; i < _systemsInit.Count; i++)
                _systemsInit[i].Init();
        }

        public virtual void Update()
        {
            if (_systemUpdate == null) return;

            for (var i = 0; i < _systemUpdate.Count; i++)
                _systemUpdate[i].Update();
        }

        public virtual void FixedUpdate()
        {
            if (_systemsFixedUpdate == null) return;

            for (var i = 0; i < _systemsFixedUpdate.Count; i++)
                _systemsFixedUpdate[i].FixedUpdate();
        }

        public virtual World AddSystem<TSystem>(TSystem system)
            where TSystem : ECSSystem
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            system.World = this;

            if (_systems == null)
                _systems = new List<ECSSystem>();
            _systems.Add(system);

            if (system is ISystemInit systemInit)
            {
                if (_systemsInit == null)
                    _systemsInit = new List<ISystemInit>();
                _systemsInit.Add(systemInit);
            }
            if(system is ISystemUpdate systemUpdate)
            {
                if (_systemUpdate == null)
                    _systemUpdate = new List<ISystemUpdate>();
                _systemUpdate.Add(systemUpdate);
            }
            if (system is ISystemFixedUpdate systemFixedUpdate)
            {
                if (_systemsFixedUpdate == null)
                    _systemsFixedUpdate = new List<ISystemFixedUpdate>();
                _systemsFixedUpdate.Add(systemFixedUpdate);
            }

            return this;
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveSystem<TSystem>()
            where TSystem : ECSSystem
        {
            var systems = _systems.FindAll(e => e is TSystem);
            foreach (var system in systems)
            {
                if (system is IDisposable disposable)
                    disposable.Dispose();
                _systemsInit.RemoveAll(e => e is TSystem);
                _systemUpdate.RemoveAll(e => e is TSystem);
                _systemsFixedUpdate.RemoveAll(e => e is TSystem);
            }
        }

        public virtual Entity CreateEntity(string name = "")
        {
            var entity = new Entity(this, string.IsNullOrEmpty(name) ? $"Entity_{_entities.Count + 1}" : name);
            _entities.Add(entity);
            return entity;
        }
    }
}
