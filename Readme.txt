Architecture
============
The solution consists of two parts.

A console application that pulls the data from the 3rd party data source. This is because the 3rd party data source
is slow to talk to therefore the API should not do that. Otherwise the consumers of the API will experience poor performance.
The console application pulls all movies from the data source and stores them in a database locally which is
quicker to talk to. This application uses a SQL database to do that but a NoSQL solution can be used as well if
no data operations are being performed on movies.

The 3rd party data source is updated once every 24 hours therefore this console application can be scheduled to run once a day.
The console application also pushes all new movie data to the data source.

The API part of the solution is pretty dumb. It reads / writes data to the SQL database. It can perform basic CRUD operations
in an async fashion.

The application uses repository pattern following DDD. 

Tests
=====
A sample unit test has been provided to demonstrate unit testing using Moq library. 