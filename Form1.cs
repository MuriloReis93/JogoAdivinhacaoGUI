using System;
using System.Windows.Forms;
using System.Drawing;

namespace JogoAdivinhacaoGUI
{
    public class Form1 : Form
    {
        private ComboBox cmbDificuldade;
        private Button btnComecar;
        private TextBox txtPalpite;
        private Button btnChutar;
        private Label lblInstrucao;
        private Label lblDica;
        private Label lblTentativas;
        private Label lblPontuacao;

        private Random random = new Random();
        private int numeroSecreto;
        private int tentativasRestantes;
        private int tentativasMax;
        private int dificuldade;
        private int pontuacao = 0;
        private bool jogoAtivo = false;

        public Form1()
        {
            this.Text = "Jogo de Adivinhação de Números";
            this.Width = 420;
            this.Height = 380;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitulo = new Label();
            lblTitulo.Text = "🎯 Jogo de Adivinhação";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);

            Label lblDificuldade = new Label();
            lblDificuldade.Text = "Escolha a dificuldade:";
            lblDificuldade.Location = new Point(20, 70);
            lblDificuldade.AutoSize = true;
            this.Controls.Add(lblDificuldade);

            cmbDificuldade = new ComboBox();
            cmbDificuldade.Location = new Point(20, 95);
            cmbDificuldade.Width = 250;
            cmbDificuldade.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDificuldade.Items.Add("Fácil (1 a 50, 10 tentativas)");
            cmbDificuldade.Items.Add("Médio (1 a 100, 7 tentativas)");
            cmbDificuldade.Items.Add("Difícil (1 a 200, 5 tentativas)");
            cmbDificuldade.SelectedIndex = 0;
            this.Controls.Add(cmbDificuldade);

            btnComecar = new Button();
            btnComecar.Text = "Começar Jogo";
            btnComecar.Location = new Point(280, 94);
            btnComecar.Width = 110;
            btnComecar.Click += BtnComecar_Click;
            this.Controls.Add(btnComecar);

            lblInstrucao = new Label();
            lblInstrucao.Text = "Escolha a dificuldade e clique em Começar.";
            lblInstrucao.Location = new Point(20, 140);
            lblInstrucao.AutoSize = true;
            lblInstrucao.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblInstrucao);

            txtPalpite = new TextBox();
            txtPalpite.Location = new Point(20, 175);
            txtPalpite.Width = 150;
            txtPalpite.Enabled = false;
            this.Controls.Add(txtPalpite);

            btnChutar = new Button();
            btnChutar.Text = "Chutar";
            btnChutar.Location = new Point(180, 173);
            btnChutar.Width = 100;
            btnChutar.Enabled = false;
            btnChutar.Click += BtnChutar_Click;
            this.Controls.Add(btnChutar);

            lblDica = new Label();
            lblDica.Text = "";
            lblDica.Location = new Point(20, 215);
            lblDica.AutoSize = true;
            lblDica.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDica.ForeColor = Color.DarkBlue;
            this.Controls.Add(lblDica);

            lblTentativas = new Label();
            lblTentativas.Text = "Tentativas: -";
            lblTentativas.Location = new Point(20, 255);
            lblTentativas.AutoSize = true;
            this.Controls.Add(lblTentativas);

            lblPontuacao = new Label();
            lblPontuacao.Text = "Pontuação total: 0";
            lblPontuacao.Location = new Point(20, 280);
            lblPontuacao.AutoSize = true;
            lblPontuacao.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblPontuacao);
        }

        private void BtnComecar_Click(object sender, EventArgs e)
        {
            dificuldade = cmbDificuldade.SelectedIndex + 1;

            int limite = dificuldade == 1 ? 50 : dificuldade == 2 ? 100 : 200;
            tentativasMax = dificuldade == 1 ? 10 : dificuldade == 2 ? 7 : 5;
            tentativasRestantes = tentativasMax;

            numeroSecreto = random.Next(1, limite + 1);
            jogoAtivo = true;

            lblInstrucao.Text = $"Pensei em um número entre 1 e {limite}. Boa sorte!";
            lblDica.Text = "";
            lblTentativas.Text = $"Tentativas: {tentativasRestantes}/{tentativasMax}";

            txtPalpite.Enabled = true;
            txtPalpite.Text = "";
            txtPalpite.Focus();
            btnChutar.Enabled = true;
            cmbDificuldade.Enabled = false;
        }

        private void BtnChutar_Click(object sender, EventArgs e)
        {
            if (!jogoAtivo) return;

            int palpite;
            bool valido = int.TryParse(txtPalpite.Text, out palpite);

            if (!valido)
            {
                lblDica.Text = "Digite um número válido.";
                lblDica.ForeColor = Color.Red;
                return;
            }

            tentativasRestantes--;

            if (palpite == numeroSecreto)
            {
                int pontosBase = dificuldade * 100;
                int pontosGanhos = pontosBase + (tentativasRestantes * 10);
                pontuacao += pontosGanhos;

                lblDica.Text = $"🎉 Acertou! Era o {numeroSecreto}. +{pontosGanhos} pontos!";
                lblDica.ForeColor = Color.Green;
                lblPontuacao.Text = $"Pontuação total: {pontuacao}";
                FinalizarRodada();
            }
            else if (tentativasRestantes <= 0)
            {
                lblDica.Text = $"😢 Acabaram as tentativas! Era o {numeroSecreto}.";
                lblDica.ForeColor = Color.Red;
                FinalizarRodada();
            }
            else if (palpite < numeroSecreto)
            {
                lblDica.Text = "📈 Maior que esse!";
                lblDica.ForeColor = Color.DarkBlue;
            }
            else
            {
                lblDica.Text = "📉 Menor que esse!";
                lblDica.ForeColor = Color.DarkBlue;
            }

            lblTentativas.Text = $"Tentativas: {tentativasRestantes}/{tentativasMax}";
            txtPalpite.Text = "";
            txtPalpite.Focus();
        }

        private void FinalizarRodada()
        {
            jogoAtivo = false;
            txtPalpite.Enabled = false;
            btnChutar.Enabled = false;
            cmbDificuldade.Enabled = true;
            lblInstrucao.Text = "Escolha a dificuldade e clique em Começar para jogar de novo.";
        }
    }
}
