using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace xmlBlackboardParser
{
    /// <summary>
    /// Action for 19-6-2015
    /// store question options and its ID (for use in getting correct option)
    /// when store question option, do not hard code to 4 options.
    /// </summary>
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

            // load the XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\farid.ANL\Documents\visual studio 2013\Projects\xmlBlackboardParser\xmlBlackboardParser\doc2.xml");
            int x; //option No

            //go straight to the node where questions are located.
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/questestinterop/assessment/section/item");

            //create obj to store questions
            List<questionItem> qItems = new List<questionItem>();

            //for each question found add its content to questionItem object
            foreach (XmlNode node in nodes)
            {
                x = 0; 

                //need to init all objects before we  start using them
                questionItem qitem = new questionItem();
                qitem.option1 = new QuestionOption();
                qitem.option2 = new QuestionOption();
                qitem.option3 = new QuestionOption();
                qitem.option4 = new QuestionOption();

                //for each questions
                XmlNodeList itemmetadata = node.SelectNodes("itemmetadata");
                
                //get the question and response/ option
                XmlNodeList presentation = node.SelectNodes("presentation/flow/flow");

                //get the correct answer (this returs all nodes where each response has its own marks but first node has correct answer
                XmlNodeList resprocessing = node.SelectNodes("resprocessing/respcondition");
                
                //questions block
                XmlNodeList QUESTION_BLOCK = presentation.Item(0).ChildNodes;
                XmlNodeList QN = QUESTION_BLOCK.Item(0).SelectNodes("material/mat_extension");

                //options block
                XmlNodeList RESPONSE_BLOCK = presentation.Item(1).ChildNodes;

                //all options are here incl option ID 'ident'
                XmlNodeList RES = RESPONSE_BLOCK.Item(0).SelectNodes("render_choice/flow_label ");







                //prototype to get //prototype to get correct Answer

                int correctNoteIndex = GetCorrectOptionNodeIndex(resprocessing);
                string correctOptionID = "";
                string correctResponseID = "";
                if (correctNoteIndex != -1)
                {
                    correctOptionID = resprocessing.Item(correctNoteIndex).Attributes["title"].Value.ToString();
                    //get the ID
                    correctResponseID = GetCorrectResponseID(resprocessing.Item(correctNoteIndex));

                }
                

                



                string a = "a";










                //QUESTION
                qitem.questiontype = itemmetadata.Item(0).SelectSingleNode("bbmd_questiontype").InnerText;
                qitem.question = QN.Item(0).SelectSingleNode("mat_formattedtext").InnerText;
                
                //OPTION 1
                x++; // init for option 1
                qitem.option1.optionText = RES.Item(0).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option1.optionID = RES.Item(0).SelectSingleNode("response_label").Attributes["ident"].Value.ToString();
                qitem.option1.OptionNumber = x;
                
                //OPTION 2
                x++;
                qitem.option2.optionText = RES.Item(1).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option2.optionID = RES.Item(1).SelectSingleNode("response_label").Attributes["ident"].Value.ToString();
                qitem.option2.OptionNumber = x;
                
                //OPTION 3
                x++;
                qitem.option3.optionText = RES.Item(2).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option3.optionID = RES.Item(2).SelectSingleNode("response_label").Attributes["ident"].Value.ToString();
                qitem.option3.OptionNumber = x;

                //OPTION 4
                x++;
                qitem.option4.optionText = RES.Item(3).SelectSingleNode("response_label/flow_mat/material/mat_extension").InnerText;
                qitem.option4.optionID = RES.Item(3).SelectSingleNode("response_label").Attributes["ident"].Value.ToString();
                qitem.option4.OptionNumber = x;

                qItems.Add(qitem);
            }


            //out content of question to screen
            foreach (questionItem a in qItems)
            {
                Console.WriteLine(a.questiontype
                    + Environment.NewLine + "Q:" + a.question
                    + Environment.NewLine + a.option1.OptionNumber + "." + "OPTIONID (" + a.option1.optionID + ") " + a.option1.optionText 
                    + Environment.NewLine + a.option2.OptionNumber + "." + "OPTIONID (" + a.option2.optionID + ") " + a.option2.optionText 
                    + Environment.NewLine + a.option3.OptionNumber + "." + "OPTIONID (" + a.option3.optionID + ") " + a.option3.optionText 
                    + Environment.NewLine + a.option4.OptionNumber + "." + "OPTIONID (" + a.option4.optionID + ") " + a.option4.optionText 
                    );
                Console.WriteLine();
            }

            // this si here so when run console it will not end till i press any key
            Console.ReadLine();
        }

        private static string GetCorrectResponseID(XmlNode _xmlNode)
        {
            return _xmlNode.SelectSingleNode("conditionvar/varequal").InnerText;
        }


        /// <summary>
        /// the BB Quiz XML 'resprocessing' node is passed here
        /// the child are 'respcondition'
        /// each option has one child and 2 more child to indicate correct and incorrect option
        /// so if a MCQ has 4 options, there will be childs
        /// this will parse through all nodes locating the one that has 'title=correct' in its attrib
        /// and it returns the object index no as an INT
        /// usually it will always be 0, but i wanted to be sure we did get the correct response then 
        /// hard-coding to always get the first object
        /// </summary>
        /// <param name="resprocessing"></param>
        /// <returns></returns>
        private static int GetCorrectOptionNodeIndex(XmlNodeList resprocessing)
        {
            string optID = "";
            int counter = -1;

            foreach (XmlNode n in resprocessing)
            {
                counter++;
                try
                {
                    optID = n.Attributes["title"].Value.ToString();
                    if (optID == "correct")
                    {
                        return counter;
                    }
                }
                catch (NullReferenceException)
                {
                    //the option does not title, hence do nothing
                    return -1;
                }

            }

            return -1;
        }

    }

    class questionItem
    {
        public string questiontype { get; set; } //MCQ, MRQ, TNF, NUM
        public string question { get; set; }
        public QuestionOption option1;
        public QuestionOption option2;
        public QuestionOption option3;
        public QuestionOption option4;
        public string answer { get; set; } // format of 1, 2, 3..etc for MRQ its '1;3'
        public string correctFeedback { get; set; }
        public string wrongFeedback { get; set; }

    }

    class QuestionOption
    {
        public string optionID { get; set; }
        public string optionText { get; set; }
        public int OptionNumber { get; set; } //to indicate option is 1, 2, 3,.. (as answer will also be in this format too.)
    }


        
}
