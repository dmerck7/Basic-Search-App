//////////////////////////////////////////////////////////////////////
/// Welcome to Basic Search App
///
/// Author - David Merck
/// 5/8/2020
//////////////////////////////////////////////////////////////////////

BasicSearchApp:

This multi-tier web app provides indexing service to resource documents for the purpose of filtering/searching the contents of the documents.

This architecture uses its own internal indexing engine with a produce-consume approach for perdormance.  The software is built with the idea of 
owning the complete soulution and growing it into a viable service.

Notable Alternative (Depenecy) Approaches:
Azure Portal:
https://docs.microsoft.com/en-us/azure/search/search-get-started-dotnet
Microsoft Indexing Service
https://flylib.com/books/en/3.443.1.100/1/


Installation

Resources
The resource folder must be moved to the c:\ drive.  It is important the index.json file be present in the resource folder.

BackEnd
Open and rebuild the solution.  Running will serve up the API at http://localhost:65522

Front-end
The front-end ember project will require "npm install"
"Ember Serve" will compile and make the the UI available at http://localhost:4200


Features:

Multi-tiered architechture abstracts the data source (Alowing for minimumly inavasive future database support)
Full Dependency Injection (Easily Portable to .Net Core)
Produce/Consume pattern for fast indexing
Search for one word or multiple words and phrases(enclosed in quotes)
Filter results by Patient
Sort results by relevancy, date, document name, patient name
Responsive layout based on Material Design
Mobile application support through Apache Cordova
Expandable Design

Recomendation
Move from Ember to to Angular, React, or Vue

Future Independent Task Scheduler (Not fully implemented yet) to index in the background based on a service bus messaging or other event triggering.


Base Requirements:

For the endpoint at /api/patients/search?query=abc , 'query' parameter is supposed to be optional. If it's not present, return all patients with all documents. Right now if you build, run within Visual Studio using IIS Express, and navigate to localhost:65522/api/patients/search using the browser, you will see an error.
However, if you build, run, and navigate to localhost:65522/api/patients/search?query=abc , you will see response with empty objects. This also needs fixing.
Make the 'query' parameter work, for instance enhance the backend to filter the results it returns by a case-insensitive, partial match over the document title and contents.
Clean up the code using better object-oriented design and improving readability.
Add unit tests.
Other possible enhancements to the search.
Other possible general backend enhancements.
Front end: if you have HTML/JavaScript experience, also work on some of the following:

Improve the result rendering in the UI with better styling of snippet results.
Update the search interface to debounce calls to the backend
Perform highlighting of the search term in the text results
Come up with and add a new feature to the UI, this can be anything your creativity can dream up.