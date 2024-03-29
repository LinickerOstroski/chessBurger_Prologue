using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace chessBurger
{
    public partial class FormJanelaPrincipal : Form
    {
        int idAlterar;
        public int IDUsuario;
        public FormJanelaPrincipal(int idUsuario)
        {
            InitializeComponent();
            tabControl1.ItemSize = new System.Drawing.Size(0, 01); // deixando os bot�es invis�veis
            this.IDUsuario = idUsuario;
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        void limpaCampos()
        {
            txt_nomeCliente.Clear();
            cob_lancheEscolhido.Text = "";
            txt_nomeCliente.Focus();
            txt_nomeLanche.Clear();
            txt_precoLanche.Clear();
            txt_igredientes.Clear();
            txt_nomeLanche.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConectaBanco con = new ConectaBanco();
            Pedido novoPedido = new Pedido();
            novoPedido.NomeCliente = txt_nomeCliente.Text;
            novoPedido.LancheEscolhido = Convert.ToInt32(cob_lancheEscolhido.SelectedValue.ToString());

            bool retorno;

            if (txt_nomeCliente.Text != "" && cob_lancheEscolhido.Text != "")
            {
                retorno = con.inserePedido(novoPedido, IDUsuario);
                MessageBox.Show("Pedido registrado!");
                txt_nomeCliente.Focus();
            }
            else
            {
                MessageBox.Show("Digite os dados!", "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txt_nomeCliente.Focus();
            }

            limpaCampos();
            listaGridPedidos();
            listaGridLanches();

        }

        private void btn_finalizarPedido_Click(object sender, EventArgs e)
        {
            int linha = dgPedidos.CurrentRow.Index;
            int id = Convert.ToInt32(
                    dgPedidos.Rows[linha].Cells["idpedido"].Value.ToString());
            DialogResult resp = MessageBox.Show("O pedido foi entregue?",
                "Finalizar Pedido", MessageBoxButtons.YesNo);
            if (resp == DialogResult.Yes)
            {
                ConectaBanco con = new ConectaBanco();
                bool retorno = con.deletaPedidos(id);
                if (retorno == true)
                {
                    MessageBox.Show("Pedido finalizado!");
                    listaGridPedidos();
                }// fim if retorno true
                else
                    MessageBox.Show(con.mensagem);
            }// fim if Ok Cancela
            else
                MessageBox.Show("Opera��o cancelada!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormJanelaPrincipal_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Bem vindo");
            txt_filtrarPedido.Focus();
            listaCOBLanches();
            listaGridPedidos();
            listaGridLanches();
        }

        public void listaCOBLanches()
        {
            ConectaBanco con = new ConectaBanco();
            DataTable tabelaDados = new DataTable();
            tabelaDados = con.listaLanches(IDUsuario); ;
            cob_lancheEscolhido.DataSource = tabelaDados;
            cob_lancheEscolhido.DisplayMember = "nomeLanche"; //O que vai mostrar
            cob_lancheEscolhido.ValueMember = "idLanche"; //O que vai buscar do Banco
        }

        public void listaGridPedidos()
        {
            ConectaBanco con = new ConectaBanco();
            dgPedidos.DataSource = con.listaPedidos(IDUsuario);
            dgPedidos.Columns["idPedido"].Visible = false;
            dgPedidos.Columns["nomeCliente"].HeaderText = "Nome do Cliente";
        }

        public void listaGridLanches()
        {
            ConectaBanco con = new ConectaBanco();
            dgLanches.DataSource = con.listaLanches(IDUsuario);
            dgLanches.Columns["idLanche"].Visible = false;
            dgLanches.Columns["idUsuarioLanche"].Visible = false;
            dgLanches.Columns["nomeLanche"].HeaderText = "Nome do Lanche";
            dgLanches.Columns["precoLanche"].HeaderText = "Pre�o do Lanche R$";
            dgLanches.Columns["ingredientes"].HeaderText = "Ingredientes Utilizados";
        }

        private void txt_filtrarPedido_TextChanged(object sender, EventArgs e)
        {
            (dgPedidos.DataSource as DataTable).DefaultView.RowFilter = string.Format("nomeCliente like '{0}%'", txt_filtrarPedido.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ConectaBanco con = new ConectaBanco();
            Lanche novoLanche = new Lanche();


            bool retorno;

            if (txt_nomeLanche.Text != "" && txt_igredientes.Text != "" && txt_precoLanche.ToString() != "")
            {
                novoLanche.NomeLanche = txt_nomeLanche.Text;
                novoLanche.Ingredientes = txt_igredientes.Text;
                novoLanche.Preco = float.Parse(txt_precoLanche.Text);
                retorno = con.insereLanche(novoLanche, IDUsuario);
                MessageBox.Show("Lanche registrado com sucesso!");
                listaCOBLanches();
                listaGridPedidos();
                listaGridLanches();
                txt_nomeLanche.Focus();
            }
            else
            {
                MessageBox.Show("Digite os dados!", "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txt_nomeLanche.Focus();
            }

            limpaCampos();
            listaCOBLanches();
            listaGridLanches();

        }

        private void cob_lancheEscolhido_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int linha = dgLanches.CurrentRow.Index;
            int id = Convert.ToInt32(
                    dgLanches.Rows[linha].Cells["idlanche"].Value.ToString());
            DialogResult resp = MessageBox.Show("Deseja mesmo remover esse Lanche?",
                "Remover lanche", MessageBoxButtons.OKCancel);
            if (resp == DialogResult.OK)
            {
                ConectaBanco con = new ConectaBanco();
                bool retorno = con.deletaLanches(id);
                if (retorno == true)
                {
                    MessageBox.Show("Lanche removido com sucesso!");
                    listaGridLanches();
                }// fim if retorno true
                else
                    MessageBox.Show("Tem um pedido cadastrado com esse lanche!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// fim if Ok Cancela
            else
                MessageBox.Show("Opera��o cancelada");

            listaCOBLanches();
            listaGridPedidos();
            listaGridLanches();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            lbl_subtitulos.Text = "Cadastrar Clientes";
        }

        private void btnLanches_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private void btnAlterarLanches_Click(object sender, EventArgs e)
        {
            DialogResult respos = MessageBox.Show("Para alterar um lanche, selecione um lanche e clique em alterar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (respos == DialogResult.OK)
            {

                tabControl1.SelectedTab = tabControl1.TabPages[1];
            }

        }

        private void btnSobre_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            lbl_subtitulos.Text = "Cadastrar Lanches";
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            int linha = dgLanches.CurrentRow.Index;// pega a linha selecionada
            idAlterar = Convert.ToInt32(
              dgLanches.Rows[linha].Cells["idLanche"].Value.ToString());
            txt_alteraNomeLanche.Text =
                 dgLanches.Rows[linha].Cells["nomeLanche"].Value.ToString();
            txt_alteraIgredienteLanche.Text =
                dgLanches.Rows[linha].Cells["ingredientes"].Value.ToString();
            txt_alteraPrecoLanche.Text =
                dgLanches.Rows[linha].Cells["precoLanche"].Value.ToString();
            tabControl1.SelectedTab = Alterar;// muda aba
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txt_alteraNomeLanche.Text != "" && txt_alteraPrecoLanche.Text != "" && txt_alteraIgredienteLanche.Text != "")
            {
                ConectaBanco con = new ConectaBanco();
                Lanche novoLanche = new Lanche();
                novoLanche.NomeLanche = txt_alteraNomeLanche.Text;
                novoLanche.Preco = float.Parse(txt_alteraPrecoLanche.Text);
                novoLanche.Ingredientes = (txt_alteraIgredienteLanche.Text);
                bool retorno = con.alteraLanche(novoLanche, idAlterar);
                if (retorno == false)
                    MessageBox.Show(con.mensagem);
                else
                    MessageBox.Show($"Altera��o de {novoLanche.NomeLanche} realizada com sucesso!");

                listaCOBLanches();
                listaGridPedidos();
                listaGridLanches();
                txt_alteraNomeLanche.Clear();
                txt_alteraPrecoLanche.Clear();
                txt_alteraIgredienteLanche.Clear();
                tabControl1.SelectedTab = Lanches;
            }
            else
            {
                MessageBox.Show("Digite os dados!", "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void btn_limpar2_Click(object sender, EventArgs e)
        {


            DialogResult resutado = MessageBox.Show("Deseja cancelar a opera��o?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resutado == DialogResult.Yes)
            {
                tabControl1.SelectedTab = Lanches;
                txt_alteraNomeLanche.Clear();
                txt_alteraPrecoLanche.Clear();
                txt_alteraIgredienteLanche.Clear();
            }
            else
            {
                txt_alteraNomeLanche.Focus();
            }

        }

        private void btn_limpar1_Click(object sender, EventArgs e)
        {
            txt_nomeLanche.Clear();
            txt_precoLanche.Clear();
            txt_igredientes.Clear();
            txt_nomeLanche.Focus();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            txt_nomeCliente.Clear();
            cob_lancheEscolhido.Text = "";
            txt_nomeCliente.Focus();
        }

        private void marcador_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgLanches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbl_teste_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[4];
            lbl_subtitulos.Text = "Pedidos Cadastrados";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void Pedidos_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[5];
            lbl_subtitulos.Text = "Lanches Cadastrados";
        }

        private void txt_alteraPrecoLanche_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
