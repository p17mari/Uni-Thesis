The context for the Database is a single teacher/user. Because of this the user is not an entity. 

A teacher in their schedgule would have different time-slots in which they would either be:
1. teaching
2. chaperoning students during breaks.

Therefore we have to break down those two states into entities. Starting with teaching, during a teaching slot a teacher would be in a classroom, teaching a lesson between a starting timespan and a finishing timespan. A classroom could be taught multiple lessons and a lesson could be taught to multiple classrooms. So they have a many to many relationship.

Similarly the chaperoning time slot will have similar attributes (i.e. location, name, time it starts and ends)

![erd-revisions-ERD-ENGLISH drawio](https://github.com/user-attachments/assets/e37d03a8-2e66-40ee-9748-8d54823416ef)

From this ERD we can converse it into a relational diagram. The result of that conversion would be the following:



![erd-revisions-Relational Diagram drawio](https://github.com/user-attachments/assets/1d0a7a32-68e5-408a-b8bb-7042d21aa1a5)
