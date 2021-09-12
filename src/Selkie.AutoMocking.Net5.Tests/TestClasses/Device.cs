namespace Selkie.AutoMocking.Net5.Tests.TestClasses
{
    public class Device
    {
        public const string SomeText = "NotAutoPopulated";

        public Device(IDevice device)
        {
            PopulatedByDependency = device.Name;
            NotAutoPopulated = SomeText;
        }

        public string PopulatedByDependency { get; set; }

        public string NotAutoPopulated { get; set; }
    }
}