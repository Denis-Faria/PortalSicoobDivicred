using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;

namespace PortalSicoobDivicred.Aplicacao
{
    public class BuscaElasticSearch
    {
        private static readonly Uri Node = new Uri("http://10.11.17.30:9200/");

        private static readonly ConnectionSettings Settings = new ConnectionSettings(Node).DefaultMappingFor<Webdesk>(
            m => m
                .IndexName("webdesks"));

        private readonly ElasticClient _client = new ElasticClient(Settings);

        public List<IHit<Webdesk>> PesquisaBasicaWebdesk(string termoPesquisado, string idSetor)
        {
            var searchResponse = _client.Search<Webdesk>(s => s.Query(q => q.Bool(b => b.Must(mu =>
                mu.Match(m => m.Field(f => f.Textointeracao).Query(HttpUtility.HtmlEncode(termoPesquisado))
                ), mu => mu.Match(m => m.Field(f => f.Idsetor).Query(idSetor))))));
            var teste = searchResponse.Hits.ToList();
            return teste;
        }

        public List<IHit<Webdesk>> PesquisaTotalWebdesk(string termoPesquisado)
        {
            var searchResponse = _client.Search<Webdesk>(s => s.Query(q =>
                q.Match(m => m.Field(f => f.Textointeracao).Query(HttpUtility.HtmlEncode(termoPesquisado)))));
            var teste = searchResponse.Hits.ToList();
            return teste;
        }
    }
}