---
title: Ytelse
identifier: ytelse
layout: default
description: Hvordan øke ytelsen på biblioteket
isHome: false
---

Klientbiblioteket benytter en `HttpWebRequest` for å kommunisere med Meldingsformidleren. I en konsollapplikasjon er denne begrenset til maks to samtidige forbindelser om gangen, mens den i en asp.net applikasjon er begrenset til ti. Dersom du ønsker å sende flere brev samtidig kan denne verdien endres f.eks til 3. Mer enn dette anbefales ikke.

{% highlight csharp%}
System.Net.ServicePointManager.DefaultConnectionLimit = 3;
{% endhighlight %}

Se [ServicePointManager.DefaultConnectionLimit](http://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.defaultconnectionlimit(v=vs.110).aspx) for mer informasjon.