namespace Assets.Common.ECS
{
    public class ECSSystem : ISystemEnabled
    {
        public World World { get; set; }

        public bool IsEnabled { get; set; }
    }
}
