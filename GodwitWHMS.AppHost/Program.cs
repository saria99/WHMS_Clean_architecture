var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.GodwitWHMS_BFF>("godwitwhms-bff");

builder.Build().Run();
