using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PS4CheaterNeo
{
    [DataContract]
    public class CheatJson
    {
        public CheatJson(string name, string id, string version, string process)
        {
            Name = name;
            Id = id;
            Version = version;
            Process = process;
            Mods = new List<Mod>();
            Credits = new List<string>();
            Credits.Add("PS4CheaterNeo");
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "id", Order = 2)]
        public string Id { get; set; }

        [DataMember(Name = "version", Order = 3)]
        public string Version { get; set; }

        [DataMember(Name = "process", Order = 4)]
        public string Process { get; set; }

        [DataMember(Name = "mods", Order = 5)]
        public List<Mod> Mods { get; set; }

        [DataMember(Name = "credits", Order = 6)]
        public List<string> Credits { get; set; }

        [DataContract]
        public class Mod
        {
            public Mod(string name, string type)
            {
                Name = name;
                Type = type;
                Memory = new List<Memory>();
            }

            [DataMember(Name = "name", Order = 1)]
            public string Name { get; set; }

            [DataMember(Name = "type", Order = 2)]
            public string Type { get; set; }

            [DataMember(Name = "memory", Order = 3)]
            public List<Memory> Memory { get; set; }
        }

        [DataContract]
        public class Memory
        {
            public Memory(string offset, string on, string off)
            {
                Offset = offset;
                On = on;
                Off = off;
            }

            [DataMember(Name = "offset", Order = 1)]
            public string Offset { get; set; }

            [DataMember(Name = "on", Order = 2)]
            public string On { get; set; }

            [DataMember(Name = "off", Order = 3)]
            public string Off { get; set; }
        }
    }
}
