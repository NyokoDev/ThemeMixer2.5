using System.Xml;
using System.Xml.Serialization;
using TM;
using UnityEngine;

namespace ThemeMixer.Resources
{
    public class ColorData
    {
        public static readonly Color32 UIColorPurple = new Color32(87, 45, 107, 255);
        public static readonly Color32 UIColorDarkBlue = new Color32(38, 70, 83, 255);
        public static readonly Color32 UIColorRed = new Color32(200, 64, 57, 255);
        public static readonly Color32 UIColorLightBlue = new Color32(52, 152, 219, 255);


        [XmlElement("SelectedColor")]
        public static Color32 UIColor = new Color32(200, 200, 200, 255);
      
        public int UIColorIndex = 0;

        private const string UIColorKey = "UIColor";
        private const string XmlFilePath = "ColorData.xml";

        

        static ColorData()
        {
            // Retrieve the stored UIColor value or use the default value
            Load();
        }

        public static void Save()
        {
            // Save the current UIColor value to XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("ColorData");
            xmlDoc.AppendChild(root);

            XmlElement colorElement = xmlDoc.CreateElement("UIColor");
            colorElement.SetAttribute("r", UIColor.r.ToString());
            colorElement.SetAttribute("g", UIColor.g.ToString());
            colorElement.SetAttribute("b", UIColor.b.ToString());
            colorElement.SetAttribute("a", UIColor.a.ToString());

            root.AppendChild(colorElement);

            xmlDoc.Save(XmlFilePath);
        }


        public static void Load()
        {
            // Load the UIColor value from XML or use the default value
            if (System.IO.File.Exists(XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XmlFilePath);

                XmlNode colorNode = xmlDoc.SelectSingleNode("/ColorData/UIColor");
                if (colorNode != null &&
                    byte.TryParse(colorNode.Attributes["r"].Value, out byte r) &&
                    byte.TryParse(colorNode.Attributes["g"].Value, out byte g) &&
                    byte.TryParse(colorNode.Attributes["b"].Value, out byte b) &&
                    byte.TryParse(colorNode.Attributes["a"].Value, out byte a))
                {
                    UIColor = new Color32(r, g, b, a);
                }
            }
        }
    }
}
