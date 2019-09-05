---
title: Ytelse
identifier: ytelse
layout: default
description: Hvordan øke ytelsen på biblioteket
isHome: false
---

Klientbiblioteket benytter en `HttpWebRequest` for å kommunisere med Meldingsformidleren. I en konsollapplikasjon er denne begrenset til maks to samtidige forbindelser om gangen, mens den i en asp.net applikasjon er begrenset til ti. Dersom du ønsker å sende flere brev samtidig kan denne verdien endres f.eks til 3. Mer enn dette anbefales ikke.

``` csharp
System.Net.ServicePointManager.DefaultConnectionLimit = 3;
```

Se [ServicePointManager.DefaultConnectionLimit](http://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.defaultconnectionlimit(v=vs.110).aspx) for mer informasjon.

Klassen `SikkerDigitalPostKlient` fungerer best ved at man oppretter en instans per applikasjon. `SikkerDigitalPostKlient` bruker en og samme instans av [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netcore-2.2) under panseret. Denne klassen er trådsikker og beste måte å bruke denne klassen på er å ha en instans per applikasjon som gjenbrukes for alle http-kall i applikasjons levetid. Av trådsikkerhetshensyn og ytelse er dette også måten brukere av biblioteket bør benytte `SikkerDigitalPostKlient` på.

### Debugging
#### Sette opp logging
Klient biblioteket har evnen til å logge nyttig informasjon som kan bli brukt for debugging.
For å skru på logging, gi `SikkerDigitalPostKlient` en `Microsoft.Extensions.Logging.ILoggerFactory` i konstruktøren.
Dette er Microsoft sitt eget logging API og lar brukeren velge deres egen logging framework.

Om du skrur på logging med nivå `DEBUG` vil output være positive resultater av requests og verre, `WARN` gir bare feilet requests eller verre, mens `ERROR` gir bare feilet requests.
Disse loggerne vil være under `Difi.SikkerDigitalPost.Klient` namespace.

#### Implementing using NLog
Det er flere måter å implementere en logger, men følgene eksempler vil være basert på [NLog dokumentasjonen](https://github.com/NLog/NLog.Extensions.Logging/wiki/Getting-started-with-.NET-Core-2---Console-application).

1. Installer Nuget pakkene `NLog`, `NLog.Extensions.Logging` og `Microsoft.Extensions.DependencyInjection`.
1. Leg en `nlog.config` fil. Den følgende er ett eksempel som logger til både fil og konsol:
``` xml
<?xml version="1.0" encoding="utf-8"?>

<!-- XSD manual extracted from package NLog.Schema: https://www.nuget.org/packages/NLog.Schema-->
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xsi:schemaLocation="NLog NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogFile="c:\temp\console-example-internal.log"
      internalLogLevel="Info">
    <!-- the targets to write to -->
    <targets>
        <!-- write logs to file -->
        <target xsi:type="File"
                name="fileTarget"
                fileName="${specialfolder:folder=UserProfile}/logs/difi-sikker-digital-post-klient-dotnet/difi-sikker-digital-post-klient-dotnet.log"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}"
                archiveEvery="Day"
                archiveNumbering="Date"
                archiveDateFormat="yyyy-MM-dd"/>
        <target xsi:type="Console"
                name="consoleTarget"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}" />
    </targets>

    <!-- rules to map from logger name to target -->
    <rules>
        <logger name="*" minlevel="Trace" writeTo="fileTarget,consoleTarget"/>
    </rules>
</nlog>
```

I din applikasjon, gjør følgende for å lage en logger og gi den til `SikkerDigitalPostKlient`:

``` csharp
private static IServiceProvider CreateServiceProviderAndSetUpLogging()
{
    var services = new ServiceCollection();

    services.AddSingleton<ILoggerFactory, LoggerFactory>();
    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

    var serviceProvider = services.BuildServiceProvider();
    SetUpLoggingForTesting(serviceProvider);

    return serviceProvider;
}

private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
    NLog.LogManager.LoadConfiguration("./nlog.config");
}

static void Main(string[] args)
{
    //Oppsett beskrevet tidligere:
    Klientkonfigurasjon klientKonfig = null;
    DataBehandler dataBehandler = null;
    
    var serviceProvider = CreateServiceProviderAndSetUpLogging();
    var client = new SikkerDigitalPostKlient(dataBehandler, klientKonfig, serviceProvider.GetService<ILoggerFactory>());
}
```


#### Request og Response Logging
Til integrasjon og debugging formål så kan det være nyttig å logge direkte requests og responses som kommer "over the wire". Dette kan oppnåes ved å gjøre følgende:

Sett denne property `Klientkonfigurasjon.LoggForespørselOgRespons = true`.

> <span style="color:red">Advarsel: Man skal aldri skru på request logging i ett produksjonsmiljø. Det vil ha en sterk negativ virkning på ytelse.</span>
