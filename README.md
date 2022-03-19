# OraEmp
A Blazor Server Project based on the Oracle HR (Emp Dept) schema. I wrote it to help me understand and experiment with concepts described in C# and Blazor Youtube videos

The [Code Wrinkles](https://www.youtube.com/c/Codewrinkles) and [Nick Chapsas](https://www.youtube.com/c/Elfocrash) have been very informative to me.

### Development Notes:

- Short lived EF DB Context (https://www.youtube.com/watch?v=aaQsmkh1BkQ)
  - **TODO**: Switch to using [DbContextScope](https://github.com/mehdime/DbContextScope)
- Use HTTP Context to get authentication details properly (https://www.youtube.com/watch?v=Eh4xPgP5PsM)
- Experiment with custom authentication (https://www.youtube.com/watch?v=b7-BC7VyyLk)
- Dependency injection to set up tests (xunit, xunit.DependencyInjection)
- Serilog for structured logging
- Use OracleConnection.ClientId to track Oracle Database queries by browser tab, associate individual session ids
- Use FluentValidation to validate fields
- [Automapper](https://docs.automapper.org) To map POCOs
- Use Docker friendly secrets to allow easy running in Docker.

### TODO
- Smarter error handling, maybe use ```<ErrorBoundary>```
- Integrate logging with my Elasticsearch or Splunk servers

