# Welcome to Basic Search App

A multi-tier web app for indexing, filtering, searching and sorting resource text documents.

<b>David Merck</b>
Living Source
5/8/2020

## A Complete Solution Without Dependencies

This solution implements its own internal indexing engine with a produce-consume approach for performance.  The architecture is built on the idea of owning the complete solution and growing it into a viable service.  The software, built to ultimately migrate to core (cross platform capability), is highly portable, lending itself to light weight container implementations.  This implementation is designed for, and would benefit greatly from, having its own persistent database storage.

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
 * Rwquires .Net 4.6
 * Open and Rebuild the solution
 * Running will serve up the API at http://localhost:65522

<b>Frontend</b>
 * Recoment using VSCode with built in terminal
 * Pull down the Ember Project
 * Requires Node.js (Must be relatively newer version)
 * Use "npm install" to install packages
 * Use "npm install -g ember-cli" to install Ember-Cli  
 * Use "Eember serve" to serve the UI at http://localhost:4200

### Design

Designed to ultimatly work as a service, document/payient metadata would be submitted and the service would store and index the content.  Currently (due to the time restriction) when the service starts, the current documents are seeded into a data context and an index.json file respresenting the current indexed state of the documents is loaded.  The ability to index all, add and remove documents from the index currently exists yet there is no UI for submitting document.  There is curently no database although, due to the abstraction layer, it shouls be easy to add one.

### Recommendations
 * Move from Ember to to Angular, React, or Vue
 * Add database support for persisting repositories and folder index
 * Implement an independent indexing task in the Task Scheduler (task not implemented yet)
 * Tigger indexing through service bus (not implemented yet) based on new doc triggered events
 * Improve information gathering for multi-word and phrase relevancy 
 * Add weights and algoithms to support word imporatance and negative words
 * Add scraping methods to grab meaningful values from docs based on preceding labels and word order.

### Enjoy
David Merck dmerck7@gmail.com
