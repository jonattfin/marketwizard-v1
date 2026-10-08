
var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(admin => admin.WithHostPort(5050));
var postgresDb = postgres.AddDatabase("marketwizard");

var redis = builder.AddRedis("redis")
    .WithDataVolume();

var server = builder.AddProject<Projects.MarketWizard_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithReference(postgresDb)
    .WithReference(redis)
    .WaitFor(postgresDb)
    .WaitFor(redis);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();