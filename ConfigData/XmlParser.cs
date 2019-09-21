using System;
using System.Xml;
using System.IO;
using RDLog;

namespace ConfigData
{
    public class XmlParser : AConfigParser
    {
        public override DataList Parse(string filename, string text = null)
        {
            this.filename = filename;

            // XML
            var doc = new XmlDocument();
            try
            {
                if (string.IsNullOrEmpty(text))
                    doc.Load(filename);
                else doc.LoadXml(text);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }

            // Begin parsing
            var dataList = ParseHeader(doc);

            return ParseData(dataList, doc) ? dataList : null;
        }

        public override ValueType ParseValue(string input, out object outValue)
        {
            ValueType ret;

            if (int.TryParse(input, out var outInt))
            {
                ret = ValueType.INT;
                outValue = outInt;
            }
            else if (float.TryParse(input, out var outFloat))
            {
                ret = ValueType.FLOAT;
                outValue = outFloat;
            }
            else
            {
                ret = ValueType.STRING;
                outValue = input;
            }

            return ret;
        }

        private void ParseAttributes(Data data, XmlAttributeCollection attributes)
        {
            foreach (XmlAttribute attr in attributes)
            {
                var valueType = ParseValue(attr.Value, out var outValue);

                var attribute = new Attribute(valueType, attr.Name, outValue);
                data.SetAttribute(attribute);
            }
        }

        private DataList ParseHeader(XmlDocument doc)
        {
            var key = doc.DocumentElement.GetAttribute("key");
            var dataList = ConfigDataManager.Inst.GetDataList(key);
            if (dataList != null) return dataList;
            dataList = new DataList();
            dataList.Init(key);
            return dataList;
        }

        private bool ParseData(DataList dataList, XmlDocument doc)
        {
            var configDatas = doc.GetElementsByTagName("data");

            foreach (XmlNode xmlNode in configDatas)
            {
                var data = new Data();
                // id should be unique
                int id = 0;
                var idAttribute = xmlNode.Attributes["id"];
                if (idAttribute != null)
                {
                    string idString = idAttribute.Value;
                    id = int.Parse(idString);
                    xmlNode.Attributes.Remove(idAttribute);
                }

                data.SetId(id);
                // Name should be unique too, only if it exists
                var nameAttribute = xmlNode.Attributes["name"];
                if (nameAttribute != null && nameAttribute.Value.Length != 0)
                {
                    var name = nameAttribute.Value;
                    data.SetName(name);
                    xmlNode.Attributes.Remove(xmlNode.Attributes["name"]);
                }

                var typeAttribute = xmlNode.Attributes["type"];
                if (typeAttribute != null && typeAttribute.Value.Length != 0)
                {
                    var type = typeAttribute.Value;
                    data.SetType(type);
                    xmlNode.Attributes.Remove(xmlNode.Attributes["type"]);
                }

                ParseAttributes(data, xmlNode.Attributes);
                dataList.AddData(data);
            }

            return true;
        }
    }
}