# AltenCancunResort
## Project description
Create a basic API for room booking.
## Requirements
1. API will be maintained by the hotel’s IT department.
1. As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime /* Async web api */ 
1. For the purpose of the test, we assume the hotel has only one room available
1. To give a chance to everyone to book the room, the stay can’t be longer than 3 days and can’t be reserved more than 30 days in advance.
1. All reservations start at least the next day of booking,
1. To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
1. Every end-user can check the room availability, place a reservation, cancel it or modify it.
1. To simplify the API is insecure.

## Tech documentation
* This project uses C# and .NET 5 since is the last stable version of the framework.
* Using VSCode as IDE, GIT as version control system.
* This solution was started using the `webapi` and `xunit` templates from `dotnet` commands.
* In order to maintain the project data self-contained, in this iteration, the persistence layer uses **in memory** scheme.
* The API Swagger description is in https://localhost:5001/swagger/index.html
* The API code uses async methods to prevent to lock the available threads for multiple requests.
* For this project it has been used a **4-Tier architecture** with the tiers as: **Controller -> Service -> Repository -> Data**, this architecture allows modularity and scalability in case any of the tier's content is required to be replaced.
* No automaper was used for this project since the entities and models were pretty simple.
* To test the functionality of the main project within the solution, a unit testing project was added using **XUnit** as test engine, and **Moq** library to create mock objects. Some tests were added for the purpose of example, it remains to add more tests in order to get better code coverage.