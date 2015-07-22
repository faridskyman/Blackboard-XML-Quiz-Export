using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlBlackboardParser
{
    public class ConstantValue
    {

        public QuestionType MultipleChoice { get; set; }
        public QuestionType True_False { get; set; }
        public QuestionType MultipleAnswer { get; set; }

        public ConstantValue()
        {
            MultipleChoice = new QuestionType("Multiple Choice","MCQ");
            True_False = new QuestionType("True/False", "TNF");
            MultipleAnswer = new QuestionType("Multiple Answer", "MRQ");
        }
  
    }


        public class QuestionType
        {
            public string Name { get; set; }
            public string ID { get; set; } 

            public QuestionType(string _Name, string _ID)
            {
                Name = _Name;
                ID = _ID;
            }
        }




}
