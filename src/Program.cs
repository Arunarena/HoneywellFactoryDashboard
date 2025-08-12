using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Content(@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Honeywell Factory Dashboard</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
    <script src=""https://cdn.jsdelivr.net/npm/chart.js""></script>
    <link rel=""stylesheet"" href=""/style.css"">
    <script src=""/dashboard.js"" defer></script>
</head>
<body class=""bg-light"">
    <nav class=""navbar navbar-expand-lg navbar-dark bg-primary mb-4"">
        <div class=""container-fluid"">
            <a class=""navbar-brand"" href=""/"">Honeywell Factory Dashboard</a>
        </div>
    </nav>
    <div class=""container"">
        <div id=""alertBox""></div>
        <div class=""row mb-4"">
            <div class=""col-md-4"">
                <div class=""card text-center"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Machines Online</h5>
                        <p class=""display-4"" id=""machineCount"">-</p>
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""card text-center"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Active Alarms</h5>
                        <p class=""display-4 text-danger"" id=""activeAlarms"">-</p>
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""card text-center"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Production Rate</h5>
                        <p class=""display-4 text-success"" id=""productionRate"">-</p>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-6 mb-4"">
                <div class=""card"">
                    <div class=""card-header"">Real-time Machine Output</div>
                    <div class=""card-body"">
                        <canvas id=""machineOutputChart""></canvas>
                    </div>
                </div>
            </div>
            <div class=""col-md-6 mb-4"">
                <div class=""card"">
                    <div class=""card-header"">Live Production Trend</div>
                    <div class=""card-body"">
                        <canvas id=""productionTrendChart""></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>", "text/html"));

var rnd = new Random();
int[] machineOutputs = Enumerable.Range(0, 10).Select(_ => rnd.Next(50, 100)).ToArray();
List<double> productionTrend = Enumerable.Range(0, 20).Select(_ => Math.Round(rnd.NextDouble() * 100, 1)).ToList();


app.MapGet("/api/status", () => {
    for (int j = 0; j < machineOutputs.Length; j++) {
        machineOutputs[j] = Math.Max(0, machineOutputs[j] + rnd.Next(-5, 6));
    }
    productionTrend.Add(Math.Round(rnd.NextDouble() * 100, 1));
    if (productionTrend.Count > 20) productionTrend.RemoveAt(0);
    int idleMachines = machineOutputs.Count(x => x < 60);
    return Results.Json(new {
        machineCount = machineOutputs.Length - idleMachines,
        activeAlarms = rnd.Next(0, 5),
        productionRate = Math.Round(productionTrend.Last(), 1),
        machineOutputs = machineOutputs,
        productionTrend = productionTrend,
        idleMachines = idleMachines
    });
});

app.Run();
