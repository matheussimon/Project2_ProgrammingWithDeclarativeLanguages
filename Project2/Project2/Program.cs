using System.Xml;           // 
using System;
using System.Xml.XPath;

namespace Project2
{
    class Program
    {
        private const string _XML_FILE = "ghg-canada.xml";

        static void Main(string[] args)
        {

            // Load XML file into the DOM
            XmlDocument doc = new XmlDocument();
            doc.Load(_XML_FILE);

            Report rep = new Report();

            char option;
            int minYear = 2015;
            int maxYear = 2019;

            do
            {
                option = rep.processMenu();


                if (option == 'Y')
                {
                    rep.getYear(out minYear, out maxYear);
                }
                else if (option == 'R')
                {
                    
                    rep.reportAllRegions(doc,minYear,maxYear);
                    
                }
                else if(option =='S')
                {
                    rep.reportAllSources(doc,minYear,maxYear);
                }
            } while (option != 'X');
        }
    }
}
