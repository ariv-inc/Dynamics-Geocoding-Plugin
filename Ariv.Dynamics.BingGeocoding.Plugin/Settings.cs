using System.Runtime.Serialization;

namespace Ariv.Dynamics.BingGeocoding.Plugin
{
    [DataContract]
    public class Settings
    {
        [DataMember(Name="key", IsRequired=true)]
        public string Key { get; set; }
    }
}
