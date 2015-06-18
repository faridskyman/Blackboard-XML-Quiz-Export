using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlBlackboardParser
{
    class Program
    {




        static void Main(string[] args)
        {
            //parseXML();
            parseXML2();
        }

        private static void parseXML2()
        {


            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\farid.ANL\Documents\visual studio 2013\Projects\xmlBlackboardParser\xmlBlackboardParser\doc.xml");
            //doc.LoadXml(xmldoc);



 
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/catalog/book");
 
            List<Book> books = new List<Book>();
 
            foreach (XmlNode node in nodes)
            {
                Book book = new Book();
 
                book.author = node.SelectSingleNode("author").InnerText;
                book.title = node.SelectSingleNode("title").InnerText;
                book.id = node.Attributes["id"].Value;
 
                books.Add(book);
            }
 
            System.Console.WriteLine("Total books: " + books.Count);
            
            foreach (Book b in books)
            {
                Console.WriteLine(b.author + " - " + b.title);
            }

            Console.ReadLine();
        }


        

        
        
        private static void parseXML()
        {
            StringBuilder output = new StringBuilder();

            String xmlString =
                @"<bookstore>
                    <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
                        <title>The Autobiography of Benjamin Franklin</title>
                        <author>
                            <first-name>Benjamin</first-name>
                            <last-name>Franklin</last-name>
                        </author>
                        <price>8.99</price>
                    </book>
                </bookstore>";

            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("book");
                reader.MoveToFirstAttribute();
                string genre = reader.Value;
                output.AppendLine("The genre value: " + genre);

                reader.ReadToFollowing("title");
                output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString());
            }
            
            Console.WriteLine(output.ToString());
            Console.ReadLine();
        }
    }


    class Book
    {
        public string id;
        public string title;
        public string author;
    }        
        
}
