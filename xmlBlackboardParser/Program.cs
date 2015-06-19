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
//            parseXML2();
            parseXML3();
        }


        private static void parseXML3()
        {


            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\farid.ANL\Documents\visual studio 2013\Projects\xmlBlackboardParser\xmlBlackboardParser\doc2.xml");
            //doc.LoadXml(xmldoc);




            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/questestinterop/assessment/section/item");

            List<Book> books = new List<Book>();
            List<questionItem> qItems = new List<questionItem>();

            foreach (XmlNode node in nodes)
            {
                //Book book = new Book();
                questionItem qitem = new questionItem();

                XmlNodeList itemmetadata = node.SelectNodes("itemmetadata");
                XmlNodeList presentation = node.SelectNodes("presentation/flow/flow");
                XmlNodeList QUESTION_BLOCK = presentation.Item(0).ChildNodes;
                XmlNodeList RESPONSE_BLOCK = presentation.Item(1).ChildNodes;

                XmlNodeList QN = QUESTION_BLOCK.Item(0).SelectNodes("material/mat_extension");
                XmlNodeList RES = RESPONSE_BLOCK.Item(0).SelectNodes("render_choice/flow_label "); //all choices are  in here

                XmlNodeList RES1 = RES.Item(0).SelectNodes("response_label/flow_mat/material/mat_extension");

                    
                    //n1.Item(0).SelectSingleNode
                //nodeList = root.SelectNodes("/bookstore/book/@bk:ISBN", nsmgr);

                qitem.questiontype = itemmetadata.Item(0).SelectSingleNode("bbmd_questiontype").InnerText;
                qitem.question = QN.Item(0).SelectSingleNode("mat_formattedtext").InnerText;
                qitem.option1 = RES.Item(0).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option2 = RES.Item(1).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option3 = RES.Item(2).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option4 = RES.Item(3).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                //book.author = node.SelectSingleNode("author").InnerText;
                //book.title = node.SelectSingleNode("title").InnerText;
                //book.id = node.Attributes["id"].Value;

                //books.Add(book);
                qItems.Add(qitem);
            }

            //System.Console.WriteLine("Total books: " + books.Count);

            foreach (questionItem a in qItems)
            {
                Console.WriteLine(a.questiontype
                    + Environment.NewLine + "Q:" + a.question 
                    + Environment.NewLine + "a." + a.option1
                    + Environment.NewLine + "b." + a.option2
                    + Environment.NewLine + "c." + a.option3
                    + Environment.NewLine + "d." + a.option4
                    );
                Console.WriteLine();
            }

            Console.ReadLine();
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

    class questionItem
    {
        public string questiontype;
        public string question;
        public string option1;
        public string option2;
        public string option3;
        public string option4;
        public string answer;
    }


        
}
