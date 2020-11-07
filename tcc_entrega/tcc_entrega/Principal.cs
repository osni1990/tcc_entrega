using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace tcc_entrega
{
    public partial class Principal : Form
    {
        //Atributos, variáveis utilizadas no escopo global
        List<GridLine> linhasGridView = new List<GridLine>();
        List<string> linhasLogTxt = new List<string>();
        HttpClient client = new HttpClient();
        const int tamanhoListaLog = 15;
        const int milisegundosEsperaRequest = 1200;

        /// <summary>
        /// Construtor da classe, chamado durante o start do projeto
        /// </summary>
        public Principal()
        {
            // Método que inicializa os componentes do Windows form
            InitializeComponent();
            // Método que preenche os itens do DropDown contendo as siglas
            carregarDropDownSiglas();
            // Método que carrega a ListBox que vai informar os logs para o usuário
            carregarListBoxLog();
            // Método executado de maneira assíncrona/independente que vai ficar infinitamente
            // chamando o endpoint da api e verificando as siglas do papel preenchidas no GRID
            Task.Run(() => buscaValorCotaWebServiceApi());
        }

        /// <summary>
        /// Método que inicializa o ListBox de logs, para evitar apontamentos de referência nula.
        /// </summary>
        private void carregarListBoxLog() => lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
        
        /// <summary>
        /// Método assíncrono que fica se repetindo infinitamente fazendo o request buscando os
        /// valores pertinentes aos papeis prenchidos no GRID
        /// </summary>
        Task buscaValorCotaWebServiceApi()
        {
            // Condição para que nunca pare de se repetir
            while (true)
            {
                try
                {
                    // Inicialização dos objetos que vão ser utilizados no processo
                    List<GridLine> linhasGridParaAnalisar = new List<GridLine>();
                    List<string> siglas = linhasGridView.Select(s => s.Sigla).Distinct().ToList();

                    // Varredura que vai incluir na lista linhasGridParaAnalisar somente o último item
                    // adicionado no GRID, caso aconteça a inserção de alguma sigla repetida
                    foreach (var sigla in siglas)
                        linhasGridParaAnalisar.Add(linhasGridView.LastOrDefault(l => l.Sigla.Equals(sigla)));

                    // Varredura nas linhas do grid que vão ser feitos request
                    foreach (var gridRaw in linhasGridParaAnalisar)
                    {
                        // URL informando endpoint da API e parâmetro contendo a sigla relacionado a linha atual na varredura
                        string URL = $"http://bvmf.bmfbovespa.com.br/cotacoes2000/FormConsultaCotacoes.asp?strListaCodigos={gridRaw.Sigla.ToUpper()}";
                        // Limpeza e adição de cabeçalhos antes da solicitação do request
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Request contendo os parâmetros adicionados anteriormente solicitando o objeto Result
                        HttpResponseMessage response = client.GetAsync(URL).Result;

                        // Consistência caso receba uma resposta positiva da API, verificando o StatusCode
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Inicializando objeto com base na string devolvida pela API
                            ComportamentoPapeis objDeserializado = XmlDeserialize(response.Content.ReadAsStringAsync().Result);

                            // Consistência que verifica se o preço recuperado da API é menor do que o
                            // indicado como piso no GRID
                            if (Convert.ToDecimal(objDeserializado.Papel.Ultimo) <= gridRaw.Piso)
                            {
                                // Método que invoca a manipulação de componentes da tela, alterando atributos
                                // nesse caso, faz com que o desenho da lâmpada seja EXIBIDO
                                picBoxLampada.Invoke((MethodInvoker)delegate
                                {
                                    picBoxLampada.Visible = true;
                                });

                                // Consistência que faz a remoção do primeiro item da fila inserido na lista de logs
                                // caso ultrapasse o valor estipulado na variável tamanhoListaLog
                                if (linhasLogTxt.Count > tamanhoListaLog)
                                    linhasLogTxt.RemoveAt(0);

                                // Adiciona na lista de log, a mensagem informando o valor recebido pela API
                                // e informando que o papel no momento da consulta está em momento de compra
                                // pois está abaixo do que foi estipulado como valor mínimo para compra.
                                linhasLogTxt.Add($"{gridRaw.Sigla} está para compra. Valor atual {objDeserializado.Papel.Ultimo} em {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.");

                                // Carrega a lista de logs
                                carregarListBoxLog();

                                // Método que invoca a manipulação de componentes da tela, alterando atributos
                                // nesse caso, está atualizando o datasource da lista de logs na tela
                                lstBoxLogEvento.Invoke((MethodInvoker)delegate
                                {
                                    lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
                                    lstBoxLogEvento.Refresh();
                                }); 
                            }
                            else if (Convert.ToDecimal(objDeserializado.Papel.Ultimo) >= gridRaw.Teto)
                            {
                                // Método que invoca a manipulação de componentes da tela, alterando atributos
                                // nesse caso, faz com que o desenho da lâmpada seja EXIBIDO
                                picBoxLampada.Invoke((MethodInvoker)delegate
                                {
                                    picBoxLampada.Visible = true;
                                });

                                // Consistência que faz a remoção do primeiro item da fila inserido na lista de logs
                                // caso ultrapasse o valor estipulado na variável tamanhoListaLog
                                if (linhasLogTxt.Count > tamanhoListaLog)
                                    linhasLogTxt.RemoveAt(0);

                                // Adiciona na lista de log, a mensagem informando o valor recebido pela API
                                // e informando que o papel no momento da consulta está em momento de venda
                                // pois está acima do que foi estipulado como valor mínimo para venda.
                                linhasLogTxt.Add($"{gridRaw.Sigla} está para venda. Valor atual {objDeserializado.Papel.Ultimo} em {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.");

                                // Método que invoca a manipulação de componentes da tela, alterando atributos
                                // nesse caso, está atualizando o datasource da lista de logs na tela
                                lstBoxLogEvento.Invoke((MethodInvoker)delegate
                                {
                                    lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
                                    lstBoxLogEvento.Refresh();
                                });
                            }
                        }

                        // Thread que faz com que os requests não sejam disparados muito depressa
                        // incluido para não correr o risco de sofrer bloqueio de solicitação pelo site da API
                        Thread.Sleep(milisegundosEsperaRequest);

                        // Método que invoca a manipulação de componentes da tela, alterando atributos
                        // nesse caso, faz com que o desenho da lâmpada seja OMITIDO
                        picBoxLampada.Invoke((MethodInvoker)delegate
                        {
                            picBoxLampada.Visible = false;
                        });
                    }

                    // Thread que faz com que os requests não sejam disparados muito depressa
                    // incluido para não correr o risco de sofrer bloqueio de solicitação pelo site da API
                    Thread.Sleep(milisegundosEsperaRequest);

                    // Método que invoca a manipulação de componentes da tela, alterando atributos
                    // nesse caso, faz com que o desenho da lâmpada seja OMITIDO
                    picBoxLampada.Invoke((MethodInvoker)delegate
                    {
                        picBoxLampada.Visible = false;
                    });
                }
                catch (Exception ex)
                { }
            }
        }

        /// <summary>
        /// Método que faz a deserialização do XML recebido pela API
        /// </summary>
        /// <param name="param">XML como string</param>
        /// <returns>Devolve um objeto do tipo ComportamentoPapeis com base no XML de entrada</returns>
        public ComportamentoPapeis XmlDeserialize(string param)
        {
            var serializer = new XmlSerializer(typeof(ComportamentoPapeis));
            ComportamentoPapeis result;

            using (TextReader reader = new StringReader(param))
                result = (ComportamentoPapeis)serializer.Deserialize(reader);

            return result;
        }

        /// <summary>
        /// Método que cria a lista de siglas no DropDown para fazer a inserção
        /// posteriormente no GRID
        /// </summary>
        private void carregarDropDownSiglas()
        {
            // Inicialização do dominio de siglas
            List<string> listaSiglas = new List<string>(){
                "ABEV3", "AZUL4", "B3SA3", "BBAS3", "BBDC3", "BBDC4", "BBSE3", "BEEF3", "BPAC11", "BRAP4", "BRDT3", "BRFS3", "BRKM5",
                "BRML3", "BTOW3", "CCRO3", "CIEL3", "CMIG4", "COGN3", "CPFE3", "CRFB3", "CSAN3", "CSNA3", "CVCB3", "CYRE3",
                "ECOR3", "EGIE3", "ELET3", "ELET6", "EMBR3", "ENBR3", "ENGI11", "EQTL3", "EZTC3", "FLRY3", "GGBR4", "GNDI3", "GOAU4",
                "GOLL4", "HAPV3", "HGTX3", "HYPE3", "IGTA3", "IRBR3", "ITSA4", "ITUB4", "JBSS3", "KLBN11", "LAME4",
                "LREN3", "MGLU3", "MRFG3", "MRVE3", "MULT3", "NTCO3", "PCAR3", "PETR3", "PETR4", "PRIO3", "QUAL3", "RADL3",
                "RAIL3", "RENT3", "SANB11", "SBSP3", "SULA11", "SUZB3", "TAEE11", "TIMS3", "TOTS3", "UGPA3",
                "USIM5", "VALE3", "VIVT4", "VVAR3", "WEGE3", "YDUQ3"
            };

            // Atualização do datasource de maneira ordenada
            cbxCodigoPapel.DataSource = listaSiglas.OrderBy(o => o).ToList();
        }

        /// <summary>
        /// Botão que faz a inserção dos dados preenchidos, no GRID
        /// </summary>
        private void btnAdicionaParametro_Click(object sender, EventArgs e)
        {
            try
            {
                // Inicializando objeto do tipo binding para preencher o datasource do GRID posteriormente
                var binding = new BindingSource();

                // Classe interna utilizada para manuseio dos itens no GRID
                GridLine entidade = new GridLine
                {
                    Sigla = cbxCodigoPapel.Text,
                    Piso = Convert.ToDecimal(txtValorPiso.Text),
                    Teto = Convert.ToDecimal(txtValorTeto.Text)
                };
                linhasGridView.Add(entidade);

                // Binding para fazer a atualização do datasource do GRID
                binding.DataSource = linhasGridView;
                dgvParametros.DataSource = binding;
            }
            catch (Exception ex)
            {
                // Mensagem de alerta em caso de erro
                MessageBox.Show("Erro na inclusão de novo parâmetro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }

    /// <summary>
    /// Classe utilizada para manuseio dos itens inseridos no GRID
    /// </summary>
    public class GridLine
    {
        // Sigla da empresa que vai ser guardado no GRID
        public string Sigla { get; set; }
        // Valor estipulado como mínimo para venda
        public decimal Teto { get; set; }
        // Valor estipulado como o mínimo para compra
        public decimal Piso { get; set; }
    }

    /// <summary>
    /// Class utilizada na deserialização do XML devolvido pela API
    /// A classe foi importada para o código fonte utilizando uma ferramenta de conversão
    /// https://xmltocsharp.azurewebsites.net/
    /// </summary>
    [XmlRoot(ElementName = "Papel")]
    public class Papel
    {
        // Sigla referente ao passado no parâmetro da chamada
        [XmlAttribute(AttributeName = "Codigo")]
        public string Codigo { get; set; }
        // Campo que informa qual o último valor informado pelo IBOVESPA
        [XmlAttribute(AttributeName = "Ultimo")]
        public string Ultimo { get; set; }
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }
        [XmlAttribute(AttributeName = "Ibovespa")]
        public string Ibovespa { get; set; }
        [XmlAttribute(AttributeName = "Data")]
        public string Data { get; set; }
        [XmlAttribute(AttributeName = "Abertura")]
        public string Abertura { get; set; }
        [XmlAttribute(AttributeName = "Minimo")]
        public string Minimo { get; set; }
        [XmlAttribute(AttributeName = "Maximo")]
        public string Maximo { get; set; }
        [XmlAttribute(AttributeName = "Medio")]
        public string Medio { get; set; }
        [XmlAttribute(AttributeName = "Oscilacao")]
        public string Oscilacao { get; set; }
    }

    /// <summary>
    /// Class utilizada na deserialização do XML devolvido pela API
    /// A classe foi importada para o código fonte utilizando uma ferramenta de conversão
    /// https://xmltocsharp.azurewebsites.net/
    /// </summary>
    [XmlRoot(ElementName = "ComportamentoPapeis")]
    public class ComportamentoPapeis
    {
        [XmlElement(ElementName = "Papel")]
        public Papel Papel { get; set; }
    }
}
