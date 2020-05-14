---
title: Logging
identifier: logging
layout: default
description: Integrer SDP.NET mot din loggplattform
isHome: false
---

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
1. Legg en `nlog.config` fil. Den følgende er ett eksempel som logger til både fil og konsol:

{% highlight xml %}
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
{% endhighlight %}

I din applikasjon, gjør følgende for å lage en logger og gi den til `SikkerDigitalPostKlient`:

``` csharp
private static IServiceProvider CreateServiceProviderAndSetUpLogging()
{
    var services = new ServiceCollection();

    services.AddSingleton<ILoggerFactory, LoggerFactory>();
    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    services.AddLogging((builder) =>
    {
        builder.SetMinimumLevel(LogLevel.Trace);
        builder.AddNLog(new NLogProviderOptions
            {CaptureMessageTemplates = true, CaptureMessageProperties = true});
        NLog.LogManager.LoadConfiguration("./nlog.config");
    });

    return services.BuildServiceProvider();
}

static void Main(string[] args)
{
    //Oppsett beskrevet tidligere:
    Klientkonfigurasjon klientKonfig = null;
    Databehandler dataBehandler = null;
    
    var serviceProvider = CreateServiceProviderAndSetUpLogging();
    var client = new SikkerDigitalPostKlient(dataBehandler, klientKonfig, serviceProvider.GetService<ILoggerFactory>());
}
```


#### Request og Response Logging
Til integrasjon og debugging formål så kan det være nyttig å logge direkte requests og responses som kommer "over the wire". Dette kan oppnåes ved å gjøre følgende:

Sett denne property `Klientkonfigurasjon.LoggForespørselOgRespons = true`.

> Merk at logging av forespørsel og respons kan gi betraktlig dårligere ytelse.


#### Prosessere dokumentpakke som sendes

Når man logger forespørsel og respons, så logges  bare XML som sendes, ikke selve dokumentpakken. Det er to måter å logge denne på:
1. Aktiver logging til disk vha `Klientkonfigurasjon.AktiverLagringAvDokumentpakkeTilDisk`.
2. Implementer `IDokumentPakkeProsessor` og legg til i `Klientkonfigurasjon.Dokumentpakkeprosessorer`
