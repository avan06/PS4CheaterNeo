using System.Collections.Generic;
using System.Xml.Serialization;

/// XML to C# Online Converter
/// https://json2csharp.com/code-converters/xml-to-csharp
namespace PS4CheaterNeo
{
    [XmlRoot(ElementName = "Trainer")]
    public class CheatTrainer
    {
        public CheatTrainer() { }
        public CheatTrainer(string name, string id, string version, string process)
        {
            Game = name;
            Cusa = id;
            Version = version;
            Process = process;
            StartUPs = new List<Cheat>();
            Cheats = new List<Cheat>();
            Moder = "PS4CheaterNeo";
        }

        [XmlAttribute(AttributeName = "Game")]
        public string Game { get; set; }

        [XmlAttribute(AttributeName = "Moder")]
        public string Moder { get; set; }

        [XmlAttribute(AttributeName = "Cusa")]
        public string Cusa { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "Process")]
        public string Process { get; set; }

        [XmlElement(ElementName = "StartUP")]
        public List<Cheat> StartUPs { get; set; }

        [XmlElement(ElementName = "Cheat")]
        public List<Cheat> Cheats { get; set; }

        [XmlElement(ElementName = "Genres")]
        public List<Genres> Genress { get; set; }

        [XmlElement(ElementName = "inventory")]
        public Inventory Inventory_ { get; set; }

        [XmlElement(ElementName = "Items")]
        public Items Items_ { get; set; }

        [XmlText]
        public string TextXml { get; set; }

        #region XmlRoot
        [XmlRoot(ElementName = "Genres")]
        public class Genres
        {
            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }
        }

        public class Cheat
        {
            public Cheat() { }
            public Cheat(string text, string description = "")
            {
                Text = text;
                Description = description;
                Cheatlines = new List<Cheatline>();
            }

            [XmlElement(ElementName = "Cheatline")]
            public List<Cheatline> Cheatlines { get; set; }

            [XmlAttribute(AttributeName = "Text")]
            public string Text { get; set; }

            [XmlAttribute(AttributeName = "Description")]
            public string Description { get; set; }

            [XmlText]
            public string TextXml { get; set; }
        }

        [XmlRoot(ElementName = "Cheatline")]
        public class Cheatline
        {
            public Cheatline() { }
            public Cheatline(string offset, int sectionId, string valueOn, string valueOff, bool absolute = false)
            {
                Offset = offset;
                SectionId = sectionId;
                ValueOn = valueOn;
                ValueOff = valueOff;
                Absolute = absolute ? "True" : null;
            }

            [XmlElement(ElementName = "Offset")]
            public string Offset { get; set; }

            [XmlElement(ElementName = "Section")]
            public int SectionId { get; set; }

            [XmlElement(ElementName = "ValueOn")]
            public string ValueOn { get; set; }

            [XmlElement(ElementName = "ValueOff")]
            public string ValueOff { get; set; }

            [XmlElement(ElementName = "Absolute")]
            public string Absolute { get; set; }
        }

        [XmlRoot(ElementName = "inventory")]
        public class Inventory
        {
            [XmlElement(ElementName = "value")]
            public List<Value> Values { get; set; }

            [XmlAttribute(AttributeName = "section")]
            public int Section { get; set; }

            [XmlAttribute(AttributeName = "offset")]
            public string Offset { get; set; }

            [XmlAttribute(AttributeName = "step")]
            public int Step { get; set; }

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "EmptyID")]
            public string EmptyID { get; set; }

            [XmlAttribute(AttributeName = "Length")]
            public int Length { get; set; }

            [XmlAttribute(AttributeName = "Size")]
            public int Size { get; set; }
        }

        [XmlRoot(ElementName = "value")]
        public class Value
        {
            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "offset")]
            public string Offset { get; set; }

            [XmlAttribute(AttributeName = "Type")]
            public string Type { get; set; }
        }

        [XmlRoot(ElementName = "Items")]
        public class Items
        {
            [XmlElement(ElementName = "item")]
            public List<Item> Items_ { get; set; }
        }

        [XmlRoot(ElementName = "item")]
        public class Item
        {
            [XmlElement(ElementName = "id")]
            public string Id { get; set; }

            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
        }
        #endregion
    }

}
