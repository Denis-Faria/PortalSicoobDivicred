using System;
using SharpRaven;
using SharpRaven.Data;

namespace PortalSicoobDivicred.Aplicacao
{
    public class SentryTracking
    {
        public void GeraLog(Exception exception)
        {
            var ravenClient = new RavenClient("https://71be15d6811411e8b0104201c0a8d02a@sentry.io/1238647");

            ravenClient.Capture(new SentryEvent(exception));
        }
    }

}