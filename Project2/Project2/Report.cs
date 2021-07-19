using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace Project2
{
    class Report
    {
        public char processMenu()
        {
            char input;
            bool validInput = false;

            do
            {
                Console.Write("Greenhouse Gas Emissions in Canada\n"
                         + "==================================\n"
                         + "'Y' to adjust the range of years  \n"
                         + "'R' to select a region            \n"
                         + "'S' to select a specific GHG source\n"
                         + "'X' to exit the program            \n");
                Console.Write("Your Selection: ");
                input = Console.ReadKey().KeyChar;
                input = Char.ToUpper(input);


                Console.WriteLine();
                if (input == 'Y' || input == 'R' || input == 'S')
                {
                    validInput = true;
                }
                else if (input == 'X')
                {
                    Console.WriteLine("\nPress any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("All Done");
                    break;
                }
                else
                {
                    Console.WriteLine("ERROR: You must key-in a valid item number from the menu.\n");
                }

            } while (!validInput);

            return input;
        } // end proccessMenu() 

        public void getYear(out int minYear, out int maxYear)
        {

            string minNumbStr;
            string maxNumbStr;

            minYear = 2015;
            maxYear = 2019;

            bool valid = false;
            do
            {
                try
                {

                    Console.Write("\nStarting year (1990 to 2019): ");
                    minNumbStr = Console.ReadLine();
                    valid = Int32.TryParse(minNumbStr, out minYear);
                    if (minYear > 2019 || minYear < 1990)
                    {
                        valid = false;
                        continue;
                    }


                    do
                    {

                        Console.Write("\nEnding year (1990 to 2019): ");
                        maxNumbStr = Console.ReadLine();
                        valid = Int32.TryParse(maxNumbStr, out maxYear);
                        if (maxYear > 2019 || maxYear < 1990)
                        {
                            valid = false;
                            continue;
                        }
                        if (minYear > maxYear || maxYear > minYear + 5)
                        {
                            Console.WriteLine($"Error: Ending year must be an integer between {minYear} and {minYear + 4}");
                            valid = false;
                            continue;

                        }


                    } while (!valid);

                }
                catch
                {
                    Console.Write("\nNumber must be an Integer");
                }

            } while (!valid);

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }//end getYear();

        public void reportAllRegions(XmlDocument doc,int minYear,int maxYear)
        {
            int i = 0;
            String choiceStr;
            int choice = 0;
            bool valid = false;

            // Create an XPathNavigator object for performing XPath queries
            XPathNavigator nav = doc.CreateNavigator();

            XmlNodeList allRegions = doc.GetElementsByTagName("region");
            Console.Write("\nSelect a Region by Number as shown Below... \n");

            foreach (XmlElement region in allRegions)
            {
                Console.WriteLine(String.Format("{0,4}. {1,-70}", ++i, region.GetAttribute("name")));
            }
            Console.WriteLine();

            do
            {
                Console.Write("\nEnter a region #:");
                choiceStr = Console.ReadLine();
                valid = Int32.TryParse(choiceStr, out choice) ? true : false;

                if (!valid)
                {
                    Console.WriteLine("\nNumber must be an Integer");
                }
            } while (i < choice || choice < 1 || !valid);


            string queryTextCountry = String.Format("//region[{0}]/@name", choice);

            XPathNodeIterator countryIt = nav.Select(queryTextCountry);


            while (countryIt.MoveNext())
            {
                Console.WriteLine(String.Format("\nEmissions in {0} (Megatonnes)\n" +
                          "================================\n\n", countryIt.Current.Value));
            }

            Console.Write(String.Format("{0,54}", "Source"));

            int yearDif = maxYear - minYear;
            int minYearCopy = minYear;

            for (int j = 0;j<yearDif+1;j++)
            {
                Console.Write(String.Format("{0, 8}", minYearCopy++));
            }
            
            Console.WriteLine();

            string queryTextDesc = String.Format("//region[{0}]/source/@description", choice);

            XPathNodeIterator nodeIt = nav.Select(queryTextDesc);
            XPathNodeIterator nodeYear;

            double valueRounded;
            while (nodeIt.MoveNext())
            {
                Console.Write(String.Format("{0,54}", nodeIt.Current.Value));
                string queryTextForDate = String.Format("//region[{0}]/source[@description=\"{1}\"]/emissions[@year>={2} and @year <= {3}]/text()", choice, nodeIt.Current.Value, minYear, maxYear);
                
                nodeYear = nav.Select(queryTextForDate);
                
                while (nodeYear.MoveNext())
                {
                    valueRounded = Double.Parse(nodeYear.Current.Value);
                    valueRounded = Math.Round(valueRounded, 3);
                    Console.Write(String.Format("{0,8}", valueRounded));
                } 

                Console.WriteLine();

            }
                
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadLine();
        }//report allRegions

        public void reportAllSources(XmlDocument doc, int minYear, int maxYear)
        {
            int i = 0;
            String choiceStr;
            int choice = 0;
            bool valid = false;

            Console.Write("\nSelect a Region by Number as shown Below... \n");

            string queryTextDesc = String.Format("//region[1]/source/@description");
            // Create an XPathNavigator object for performing XPath queries
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator descIt = nav.Select(queryTextDesc);

            while (descIt.MoveNext())
            {
                Console.WriteLine(String.Format("{0,4}. {1,-70}", ++i, descIt.Current.Value));
            }
            Console.WriteLine();

            do
            {
                Console.Write("\nEnter a region #:");
                choiceStr = Console.ReadLine();
                valid = Int32.TryParse(choiceStr, out choice) ? true : false;

                if (!valid)
                {
                    Console.WriteLine("\nNumber must be an Integer");
                }
            } while (i < choice || choice < 1 || !valid);

            string queryTextEmissions = String.Format("//region[1]/source[{0}]/@description",choice);

            XPathNodeIterator emissionIt = nav.Select(queryTextEmissions);

            while (emissionIt.MoveNext())
            {
                Console.WriteLine(String.Format("\nEmissions from {0} (Megatonnes)\n" +
                          "================================\n\n", emissionIt.Current.Value));
            }

            Console.Write(String.Format("{0,54}", "Source"));

            int yearDif = maxYear - minYear;
            int minYearCopy = minYear;

            for (int j = 0; j < yearDif + 1; j++)
            {
                Console.Write(String.Format("{0, 8}", minYearCopy++));
            }

            Console.WriteLine();
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadLine();

        }

    }
}
