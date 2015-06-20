# XMLblackboardParser
to parse blackboard quiz xml exported data and extract the base data (question, options, correct answer) 

## About Version1  Branch
Completed all outstanding task,now code
1. can handle MCQ and TNF
2. Options are no longer hard coded to 4 choice but dynamic
3. Output is in Tab seperated and out as Text file
4. Handles the variance in correct option for blackboard that initially caused error
5. Added ASCII ART at start
6. Handle exception where input file is misisng
7. Handles exception if otput file is in use.
8. Handles file path to be where the program is located instead of hard coded.

## Limitations
* Works only for for 'True or False' & 'Multiple Choice' question-type only.
* MAX option supported is 9. 

