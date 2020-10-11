using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcc_entrega
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
            carregarDropDownSiglas();
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

            teste.DataSource = 
        }

       public class gridLine{

            public string  sigla { get; set; }
            public string teto { get; set; }
            public string piso { get; set; }

        }
    }
}
