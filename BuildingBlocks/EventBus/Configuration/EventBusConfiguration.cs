namespace HexMaster.BuildingBlocks.EventBus.Configuration
{
    public class EventBusConfiguration
    {

        public const string SettingName = "EventBus";

        public string SubscriptionClientName { get; set; }
        public string EventBusConnection { get; set; }
        public int EventBusRetryCount { get; set; }

    }
}
