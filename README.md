# RVezyWebAPI

My considerations:

- I built POST endpoints responsible for uploading CSV files but I was unable to test them because I couldn't run the web API inside the limit time.

- I couldn't test connecting to a database for the same reason even though the DbContext was configured.

- All layers (API, Infra, Domain and Tests) were integrated.

- Only unit tests involving Listing (according to the Level 1 tasks) were made by mocking the repository's results.


In case of running the project, the .sln file is inside the "1 - API" folder.