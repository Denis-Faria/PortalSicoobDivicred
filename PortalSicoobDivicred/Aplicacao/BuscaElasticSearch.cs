using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;

namespace PortalSicoobDivicred.Aplicacao
{
    public class BuscaElasticSearch
    {
        static Uri Node = new Uri("http://10.11.17.30:9200/");

        static ConnectionSettings Settings = new ConnectionSettings(Node).DefaultMappingFor<Webdesk>(m => m
            .IndexName("webdesks"));
        ElasticClient client = new ElasticClient(Settings);

        public void PesquisaBasicaWebdesk(string TermoPesquisado)
        {

            var searchResponse = client.Search<Webdesk>(s => s.Query(q => q.Match(m => m.Field(f => f.textointeracao).Query(HttpUtility.HtmlEncode(TermoPesquisado)))));
            var teste = searchResponse.Hits.ToList();
        }
    }
}