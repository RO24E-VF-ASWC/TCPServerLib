using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TryMyServerApp.div
{
    public class ReadConfig
    {

        public void Read(string filename)
        {
            XmlDocument configDoc = new XmlDocument(); 
            configDoc.Load(filename);


            XmlNode portNode = configDoc.DocumentElement.SelectSingleNode("Port");
            if (portNode != null) 
            { 
                String portStr = portNode.InnerText.Trim(); 
                int port = Convert.ToInt32(portStr); 
            }


            XmlNode nameNode = configDoc.DocumentElement.SelectSingleNode("Name");
            if (nameNode != null)
            {
                String name = nameNode.InnerText.Trim();
            }


        }
    }
}
