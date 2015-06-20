using System;
using System.Collections;
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

            OutProgramASCIIArt();
            parseXML();
        }


        private static void parseXML()
        {
            // load the XML file
            XmlDocument doc = new XmlDocument();
            string path = @"./";
            //doc.Load(path + "doc2.xml");
            try
            {
                doc.Load(path + "res00001.dat");
            }
            catch
            {
                OutputToScreenPause("File not found. Pls Ensure a file named 'res00001.dat' exist in the same loc of this program.");
            }
            

            //go straight to the node where questions are located.
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/questestinterop/assessment/section/item");

            //create LIST of obj to store questions
            List<questionItem> qItems = new List<questionItem>();

            //for each question found add its content to questionItem object
            foreach (XmlNode node in nodes)
            {
                //need to init all objects before we  start using them
                questionItem qitem = new questionItem();

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



            // PARSING PART

                //Question
                qitem.questiontype = itemmetadata.Item(0).SelectSingleNode("bbmd_questiontype").InnerText;
                qitem.question = QN.Item(0).SelectSingleNode("mat_formattedtext").InnerText;


                //Correct Answer 
                int correctNoteIndex = GetCorrectOptionNodeIndex(resprocessing);
                if (correctNoteIndex != -1)
                {
                    qitem.answerID = GetCorrectResponseID(resprocessing.Item(correctNoteIndex));
                }

                
                //Options 
                //  looping and store in Arraylist in object
                int y = 0;
                foreach (XmlNode o in RES)
                {
                    QuestionOption qo = new QuestionOption();
                    y++;
                    qo.optionID = o.SelectSingleNode("response_label").Attributes["ident"].Value.ToString();
                    
                    

                    if (qitem.questiontype == "Multiple Choice")
                    {
                        qo.optionText = o.SelectSingleNode("response_label/flow_mat/material/mat_extension/mat_formattedtext").InnerText; //changed here
                    }
                    else if (qitem.questiontype == "True/False")
                    {
                        qo.optionText = o.SelectSingleNode("response_label/flow_mat/material/mattext").InnerText;
                    }

                    qo.OptionNumber = y;
                    qitem.options.Add(qo);
                }

                //gets the correct answer number
                qitem.answer = GetAnswerNumber(qitem);

                //Add to List
                qItems.Add(qitem);
            }


            //DisplayResponses(qItems);
            DisplayResponsesCSV(qItems, path);


           
        


        }

        //Functions section

        public static void OutProgramASCIIArt()
        {
            string output = @"
  ____  _            _    _                         _            
 |  _ \| |          | |  | |                       | |           
 | |_) | | __ _  ___| | _| |__   ___   __ _ _ __ __| |           
 |  _ <| |/ _` |/ __| |/ / '_ \ / _ \ / _` | '__/ _` |           
 | |_) | | (_| | (__|   <| |_) | (_) | (_| | | | (_| |           
 |____/|_|\__,_|\___|_|\_\_.__/ \___/ \__,_|_|  \__,_|           
   ____        _       ______                       _            
  / __ \      (_)     |  ____|                     | |           
 | |  | |_   _ _ ____ | |__  __  ___ __   ___  _ __| |_ ___ _ __ 
 | |  | | | | | |_  / |  __| \ \/ / '_ \ / _ \| '__| __/ _ \ '__|
 | |__| | |_| | |/ /  | |____ >  <| |_) | (_) | |  | ||  __/ |   
  \___\_\\__,_|_/___| |______/_/\_\ .__/ \___/|_|   \__\___|_|   
                                  | |                            
                                  |_|         
";
            Console.WriteLine(output + "\n");


        }

        private static void OutputToScreenPause(string msg)
        {
            Console.WriteLine(msg + "\nPress any key to exit");
            Console.ReadLine();
        }

        /// <summary>
        /// BB XML does not indicate which option number is the correct answer, this function will return the number
        /// that this code has already tagged to each option.
        /// </summary>
        /// <param name="qitem"></param>
        /// <returns></returns>
        private static string GetAnswerNumber(questionItem qitem)
        {
            foreach (QuestionOption qo in qitem.options)
            {
                if (qo.optionID == qitem.answerID)
                {
                    return qo.OptionNumber.ToString();
                }
            }
            return "0";
        }




        /// <summary>
        /// Output response to screen
        /// </summary>
        /// <param name="qItems"></param>
        private static void DisplayResponses(List<questionItem> qItems)
        {
            //out content of question to screen
            foreach (questionItem a in qItems)
            {
                Console.WriteLine(Environment.NewLine + Environment.NewLine + a.questiontype + Environment.NewLine + "Q:" + a.question);
                Console.WriteLine("ANS ID: " + a.answerID + ". Answer No." + a.answer);
        
                    foreach(QuestionOption qo in a.options)
                    {
                        //qo.OptionNumber
                        Console.WriteLine(Environment.NewLine +
                            qo.OptionNumber + ". Op#: '" + qo.optionID + "'"
                            + Environment.NewLine + "OP:" + qo.optionText);
                    }

            }

            // this is here so when run console it will not end till i press any key
            //Console.ReadLine();
        }

    private static void DisplayResponsesCSV(List<questionItem> qItems, string path)
        {
            string text = "";
            text += "Question\tOption 1\tOption 2\tOption 3\tOption 4\tOption 5\tOption 6\tOption 7\tOption 8\tOption 9\tType\tAnswer" + Environment.NewLine;
            
            foreach (questionItem qi in qItems)
            {
                text += qi.question + tab(1);
                
                //+tab(10) 
                //loop options
                foreach (QuestionOption qo in qi.options)
                {
                    text += qo.optionText + tab(1);
                }

                text += tab(9 - qi.options.Count); // as option support is up to 9, hence this auto add the options that not used as blank
                
                text += RenameQuestionType(qi.questiontype) + tab(1) + qi.answer + Environment.NewLine;
            }

            try { 
                    System.IO.File.WriteAllText("./" + "questionExported.txt", text);
                    OutputToScreenPause("Export Done, open this file in excel 'questionExported.txt'.");
                }
            catch
                {
                    OutputToScreenPause("Cant Export the question, ensure this file 'questionExported.txt' is not open and try again.");
                }


        }

    private static string RenameQuestionType(string questiontype)
    {
        
        if (questiontype == "Multiple Choice")
        {
            return "MCQ";
        }
        else if  (questiontype == "True/False")
        {
            return "TNF";
        }
        else
        {
            return "NONE";
        }
    }

        private static string tab(int no)
        {
            string retVal = "";
            for(int i=0; i < no; i++)
            {
                retVal += "\t";
            }
            return retVal;
        }


       private static void WriteToFileSample()
    {
        FileStream ostrm;
        StreamWriter writer;
        TextWriter oldOut = Console.Out;
        try
        {
            ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot open Redirect.txt for writing");
            Console.WriteLine(e.Message);
            return;
        }
        Console.SetOut(writer);
        Console.WriteLine("This is a line of text");
        Console.WriteLine("Everything written to Console.Write() or");
        Console.WriteLine("Console.WriteLine() will be written to a file");
        Console.SetOut(oldOut);
        writer.Close();
        ostrm.Close();
        Console.WriteLine("Done");

    }


        /// <summary>
        /// returns the correct responseID from the passed XMLNode
        /// </summary>
        /// <param name="_xmlNode"></param>
        /// <returns></returns>
        private static string GetCorrectResponseID(XmlNode _xmlNode)
        {
            if (!string.IsNullOrEmpty(_xmlNode.SelectSingleNode("conditionvar/varequal").InnerText))
                return _xmlNode.SelectSingleNode("conditionvar/varequal").InnerText;
            else
                return "";
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
                    //the option does not title, hence check child node that 'displayfeedback linkrefid="correct"'
                    return GetCorrectOptionBydisplayfeedback(resprocessing);
                }

            }

            return -1;
        }


        private static int GetCorrectOptionBydisplayfeedback(XmlNodeList resprocessing)
        {
            int counter = -1;

            foreach (XmlNode n in resprocessing)
            {
                counter++;
                try
                {
                    if (n["displayfeedback"].Attributes["linkrefid"].Value == "correct")
                    {
                        return counter;
                    }
                }
                catch (NullReferenceException)
                {
                    //no action
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
        public ArrayList options = new ArrayList(); //proto
        public string answer { get; set; } // format of 1, 2, 3..etc for MRQ its '1;3'
        public string answerID { get; set; } // BB answerID
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
