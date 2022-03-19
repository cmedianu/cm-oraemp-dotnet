- modals for editing
- elasticsearch / splunk logging
- Scoped DB Factory
- Logging 
  - Remove  from DI 
  - var myLog = Log.ForContext<MyClass>();
  - var jobLog = Log.ForContext("JobId", job.Id);
  - when logging to sinks that use a text format, such as Serilog.Sinks.Console, you can include {Properties} in the output template to print out all contextual properties not otherwise included.
    using (logger.PushProperty("OperationType", "update"))
    {
    // UserId and OperationType are set for all logging events in these brackets
    }
- add <ErrorBoundary>, error handling
- straight sql/plsq/select */ over EF
- [Parameter, SupplyParameterFromQuery]  
- MediatR