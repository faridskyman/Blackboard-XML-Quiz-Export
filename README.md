# XMLblackboardParser
to parse blackboard quiz xml exported data and extract the base data (question, options, correct answer) 

## About Version1  Branch
Currently in base added a sample BB Quiz XML file and code currently does is able to to extract the 
* question type
* question
* first 4 options 

## Limitations
* does not work for True or False questions
* does not work if options is more then 4 (will definately throw error) as it hard coded to assume all question has 4 options only
* not getting the option which is correct answer (next release)

