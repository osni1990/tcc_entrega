using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcc_entrega
{
    public partial class Principal : Form
    {
        List<GridLine> linhasGridView = new List<GridLine>();
        HttpClient client = new HttpClient();

        public Principal()
        {
            InitializeComponent();
            carregarDropDownSiglas();
            Task.Run(() => buscaValorCotaWebServiceApi());
        }

        Task buscaValorCotaWebServiceApi()
        {
            //for(int i = 0; i < 30; i++)
            while (true)
            {
                foreach (var gridRaw in linhasGridView)
                {
                    //string URL = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={gridRaw.Sigla}&apikey=9LMD1QIJT3NY8W65";
                    string URL = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol=IBM&apikey=9LMD1QIJT3NY8W65";
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Application", "suhdiqwhdiuqhwdiuahisudh12iuhi12h4i1hri1h24iu12h4iu");
                    HttpResponseMessage response = client.GetAsync(URL).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string respString = response.Content.ReadAsStringAsync().Result;
                        PrecoEmpresaMomentoOUT objJson = JsonConvert.DeserializeObject<PrecoEmpresaMomentoOUT>(respString);

                        //Random random = new Random();
                        //int ponteiro = random.Next(0, 10);

                        //if (ponteiro <= 5)
                        if (objJson.EPS <= 5)
                        {
                            pictureBox1.Invoke((MethodInvoker)delegate
                            {
                                pictureBox1.Visible = false;
                                label4.Text = $"{gridRaw.Sigla} está pronta para venda";
                            });
                        }
                        else
                        {
                            pictureBox1.Invoke((MethodInvoker)delegate
                            {
                                pictureBox1.Visible = true;
                                label4.Text = $"{gridRaw.Sigla} está pronta para compra";
                            });
                        }
                    }
                    
                    Thread.Sleep(1200);
                }

                
                Thread.Sleep(1200);
            }
            return null;
        }

        private void carregarDropDownSiglas()
        {
            List<string> listaSiglas = new List<string>() {
                "PETR4",
                "Sigla2",
                "Sigla3",
                "Sigla4"
            };
            comboBox1.DataSource = listaSiglas;
        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var teste = new BindingSource();

            GridLine entidade = new GridLine
            {
                Sigla = comboBox1.Text,
                Piso = textBox1.Text,
                Teto = textBox2.Text
            };
            linhasGridView.Add(entidade);


            teste.DataSource = linhasGridView;
            dataGridView1.DataSource = teste;
        }


    }
    public class GridLine
    {

        public string Sigla { get; set; }
        public string Teto { get; set; }
        public string Piso { get; set; }

    }

    public class PrecoEmpresaMomentoOUT
    {
        public string Symbol { get; set; }
        public decimal EPS { get; set; }
    }
}
