# XMLblackboardParser
A simple C# console program to parse blackboard exported quiz file (which is in xml format), it exported relevant data and saves as a Tab Seperated Text file (that can be open in excel) or and Imported to Wizlearn's LMS

## About Version1  Branch
Completed all outstanding task,now code
* can handle MCQ and TNF
* Options are no longer hard coded to 4 choice but dynamic
* Output is in Tab seperated and out as Text file
* Handles the variance in correct option for blackboard that initially caused error
* Added ASCII ART at start
* Handle exception where input file is misisng
* Handles exception if otput file is in use.
* Handles file path to be where the program is located instead of hard coded.

## Limitations
* Works only for for 'True or False' & 'Multiple Choice' question-type only.
* MAX option supported is 9. 

