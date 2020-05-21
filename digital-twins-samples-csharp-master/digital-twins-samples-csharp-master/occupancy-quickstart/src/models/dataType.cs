//Added Abh 2020

namespace Microsoft.Azure.DigitalTwins.Samples.Models
{
    public class SensorDataType
    {
        // TODO: Need to fixup ids in models to be Guids (were approriate)
        // Then we can change upstream apis like DescriptionExtensions
        
            public string Category { get; set; }
            public string SpaceId { get; set; }
            public string Name { get; set; }
            public string FriendlyName { get; set; }
            public string Description { get; set; }
            public bool Disabled { get; set; }
            public int LogicOrder { get; set; }

    }
}
