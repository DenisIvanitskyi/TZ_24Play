using System.Collections.Generic;

namespace Assets.Common.ECS
{
    public class Entity
    {
        private List<IComponent> _componentns;

        public World World { get; }

        public IReadOnlyList<IComponent> Components => _componentns;

        public string Name { get; }
        public Entity(World world, string name)
        {
            World = world;
            _componentns = new List<IComponent>();
            Name = name;
        }

        public virtual TComponent AddComponent<TComponent>(TComponent component)
            where TComponent : IComponent
        {
            _componentns.Add(component);
            return component;
        }

        public virtual void RemoveComponent(IComponent component)
            => _componentns.Remove(component);
    }
}
