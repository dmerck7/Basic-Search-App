# Welcome to Basic Search App

A multi-tier web app for indexing, filtering, searching and sorting resource text documents.

<b>David Merck</b>
Living Source
5/8/2020

## A Complete Solution Without Dependencies

This solution uses its own internal indexing engine with a produce-consume approach for performance.  The architecture is built on the idea of owning the complete solution and growing it into a viable service.  It is built with going to core (Cross Platform Capability) in mind and being able to spin up container instances without dependency restrictions.  The software is designed for and would benefit greatly in having its own database for persistent storage.

<b>Here are some notable dependency driven alternative approaches</b>

* Azure Portal - https://docs.microsoft.com/en-us/azure/search/search-get-started-dotnet
* Microsoft Indexing Service - https://flylib.com/books/en/3.443.1.100/1/

### Features

 * Multi-tiered architecture abstracts the data source (Allowing for minimally invasive future database support)
 * Full Dependency Injection - easily port to .Net Core and apply unit testing
 * Produce/Consume pattern for fast indexing
 * Search for one word or multiple words or phrases(enclosed in quotes)
 * Filter results by Patient
 * Sort results by relevancy, date, document name, or patient name
 * Responsive layout based on Material Design
 * Mobile application support through Apache Cordova
 * Expandable Design

### Installation

<b>Resources</b>
 * The resource folder must be moved to the root of the c:\ drive
 * Verify the index.json file is present in the resource folder.

<b>BackEnd</b>
 * Pulldown the repo
 * Open and Rebuild the solution
 * Running will serve up the API at http://localhost:65522

<b>Frontend</b>
 * Pull down the Ember Project
 * Use "npm install" to install packages (Relies on Ember Cli)
 * Use "Eember serve" to serve the UI at http://localhost:4200

### Recommendations
 * Move from Ember to to Angular, React, or Vue
 * Add database support for persisting repositories and folder index
 * Implement independent indexing task in the Task Scheduler (task not implemented yet)
 * Tigger indexing through service bus (not implemented ye) based on events for new docs
 * Improve indexing/querying to span multiple page documents and phrases across multiple lines

### Enjoy
David Merck dmerck7@gmail.com

