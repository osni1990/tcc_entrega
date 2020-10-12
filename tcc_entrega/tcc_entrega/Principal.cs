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
        List<GridLine> linhasGridView = new List<GridLine>();
        List<string> linhasLogTxt = new List<string>();
        HttpClient client = new HttpClient();
        const int tamanhoListaLog = 15;
        const int milisegundosEsperaRequest = 1200;

        public Principal()
        {
            InitializeComponent();
            carregarDropDownSiglas();
            carregarListBoxLog();
            Task.Run(() => buscaValorCotaWebServiceApi());
        }

        private void carregarListBoxLog() => lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
        
        Task buscaValorCotaWebServiceApi()
        {
            while (true)
            {
                try
                {
                    List<GridLine> linhasGridParaAnalisar = new List<GridLine>();
                    List<string> siglas = linhasGridView.Select(s => s.Sigla).Distinct().ToList();
                    foreach (var sigla in siglas)
                        linhasGridParaAnalisar.Add(linhasGridView.LastOrDefault(l => l.Sigla.Equals(sigla)));
                    foreach (var gridRaw in linhasGridParaAnalisar)
                    {
                        //string URL = $"http://bvmf.bmfbovespa.com.br/cotacoes2000/FormConsultaCotacoes.asp?strListaCodigos={gridRaw.Sigla.ToUpper()}";
                        string URL = $"http://bvmf.bmfbovespa.com.br/cotacoes2000/FormConsultaCotacoes.asp?strListaCodigos=PETR4";
                        //string URL = $"http://bvmf.bmfbovespa.com.br/cotacoes2000/FormConsultaCotacoes.asp?strListaCodigos=PETR4|VALE5";
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Add("Application", "suhdiqwhdiuqhwdiuahisudh12iuhi12h4i1hri1h24iu12h4iu");
                        HttpResponseMessage response = client.GetAsync(URL).Result;

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //string respString = response.Content.ReadAsStringAsync().Result;
                            ComportamentoPapeis objDeserializado = XmlDeserialize(response.Content.ReadAsStringAsync().Result);

                            //PrecoEmpresaMomentoOUT objJson = JsonConvert.DeserializeObject<PrecoEmpresaMomentoOUT>(respString);

                            //Random random = new Random();
                            //int ponteiro = random.Next(0, 10);

                            //if (ponteiro <= 5)
                            //if (objJson.EPS <= 5)
                            if (Convert.ToDecimal(objDeserializado.Papel.Ultimo) <= gridRaw.Piso)
                            {
                                picBoxLampada.Invoke((MethodInvoker)delegate
                                {
                                    picBoxLampada.Visible = true;
                                });

                                if (linhasLogTxt.Count > tamanhoListaLog)
                                    linhasLogTxt.RemoveAt(0);

                                linhasLogTxt.Add($"{gridRaw.Sigla} está para compra. Valor atual {objDeserializado.Papel.Ultimo} em {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.");
                                carregarListBoxLog();
                                lstBoxLogEvento.Invoke((MethodInvoker)delegate
                                {
                                    lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
                                    lstBoxLogEvento.Refresh();
                                }); 
                            }
                            else if (Convert.ToDecimal(objDeserializado.Papel.Ultimo) >= gridRaw.Teto)
                            {
                                picBoxLampada.Invoke((MethodInvoker)delegate
                                {
                                    picBoxLampada.Visible = true;
                                });

                                if (linhasLogTxt.Count > tamanhoListaLog)
                                    linhasLogTxt.RemoveAt(0);

                                linhasLogTxt.Add($"{gridRaw.Sigla} está para venda. Valor atual {objDeserializado.Papel.Ultimo} em {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.");
                                lstBoxLogEvento.Invoke((MethodInvoker)delegate
                                {
                                    lstBoxLogEvento.DataSource = linhasLogTxt.ToArray();
                                    lstBoxLogEvento.Refresh();
                                });
                            }
                        }

                        Thread.Sleep(milisegundosEsperaRequest);

                        picBoxLampada.Invoke((MethodInvoker)delegate
                        {
                            picBoxLampada.Visible = false;
                        });
                    }
                    
                    Thread.Sleep(milisegundosEsperaRequest);

                    picBoxLampada.Invoke((MethodInvoker)delegate
                    {
                        picBoxLampada.Visible = false;
                    });
                }
                catch (Exception ex)
                { }
            }
        }

        public ComportamentoPapeis XmlDeserialize(string param)
        {
            var serializer = new XmlSerializer(typeof(ComportamentoPapeis));
            ComportamentoPapeis result;

            using (TextReader reader = new StringReader(param))
                result = (ComportamentoPapeis)serializer.Deserialize(reader);

            return result;
        }

        private void carregarDropDownSiglas()
        {
            List<string> listaSiglas = new List<string>(){
                "ABEV3", "AZUL4", "B3SA3", "BBAS3", "BBDC3", "BBDC4", "BBSE3", "BEEF3", "BPAC11", "BRAP4", "BRDT3", "BRFS3", "BRKM5",
                "BRML3", "BTOW3", "CCRO3", "CIEL3", "CMIG4", "COGN3", "CPFE3", "CRFB3", "CSAN3", "CSNA3", "CVCB3", "CYRE3",
                "ECOR3", "EGIE3", "ELET3", "ELET6", "EMBR3", "ENBR3", "ENGI11", "EQTL3", "EZTC3", "FLRY3", "GGBR4", "GNDI3", "GOAU4",
                "GOLL4", "HAPV3", "HGTX3", "HYPE3", "IGTA3", "IRBR3", "ITSA4", "ITUB4", "JBSS3", "KLBN11", "LAME4",
                "LREN3", "MGLU3", "MRFG3", "MRVE3", "MULT3", "NTCO3", "PCAR3", "PETR3", "PETR4", "PRIO3", "QUAL3", "RADL3",
                "RAIL3", "RENT3", "SANB11", "SBSP3", "SULA11", "SUZB3", "TAEE11", "TIMS3", "TOTS3", "UGPA3",
                "USIM5", "VALE3", "VIVT4", "VVAR3", "WEGE3", "YDUQ3"
            };

            cbxCodigoPapel.DataSource = listaSiglas.OrderBy(o => o).ToList();
        }

        private void btnAdicionaParametro_Click(object sender, EventArgs e)
        {
            try
            {
                var binding = new BindingSource();

                GridLine entidade = new GridLine
                {
                    Sigla = cbxCodigoPapel.Text,
                    Piso = Convert.ToDecimal(txtValorPiso.Text),
                    Teto = Convert.ToDecimal(txtValorTeto.Text)
                };
                linhasGridView.Add(entidade);

                binding.DataSource = linhasGridView;
                dgvParametros.DataSource = binding;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na inclusão de novo parâmetro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }

    public class GridLine
    {
        public string Sigla { get; set; }
        public decimal Teto { get; set; }
        public decimal Piso { get; set; }
    }

    public class PrecoEmpresaMomentoOUT
    {
        public string Symbol { get; set; }
        public decimal EPS { get; set; }
    }


    [XmlRoot(ElementName = "Papel")]
    public class Papel
    {
        [XmlAttribute(AttributeName = "Codigo")]
        public string Codigo { get; set; }
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
        [XmlAttribute(AttributeName = "Ultimo")]
        public string Ultimo { get; set; }
        [XmlAttribute(AttributeName = "Oscilacao")]
        public string Oscilacao { get; set; }
    }

    [XmlRoot(ElementName = "ComportamentoPapeis")]
    public class ComportamentoPapeis
    {
        [XmlElement(ElementName = "Papel")]
        public Papel Papel { get; set; }
    }
}
