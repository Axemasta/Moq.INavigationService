using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test names are seperated by underscores")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Mock exceptions will be created for tests")]
