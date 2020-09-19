using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports; // necessario para as portas
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedVoice
{
    public partial class Form1 : Form
    {
        //Variaveis globais
        // variaveis para voz
        static CultureInfo ci = new CultureInfo("pt-BR");// linguagem utilizada
        static SpeechRecognitionEngine reconhecedor; // reconhecedor de voz
        SpeechSynthesizer resposta = new SpeechSynthesizer();// sintetizador de voz

        // Palavras aceitas
        public string[] listaPalavras = { "acender", "apagar" };


        public Form1()
        {
            InitializeComponent();
            timerCOM.Enabled = true;

            Init();
        }

        private void AtualizaListaCOMs()
        {
            int i;
            bool quantDiferente;    //flag para sinalizar que a quantidade de portas mudou

            i = 0;
            quantDiferente = false;

            //se a quantidade de portas mudou
            if (comboBox1.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (comboBox1.Items[i++].Equals(s) == false)
                    {
                        quantDiferente = true;
                    }
                }
            }
            else
            {
                quantDiferente = true;
            }

            //Se não foi detectado diferença
            if (quantDiferente == false)
            {
                return;                     //retorna
            }

            //limpa comboBox
            comboBox1.Items.Clear();

            //adiciona todas as COM diponíveis na lista
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
            //seleciona a primeira posição da lista
            comboBox1.SelectedIndex = 0;
        }

        // ********* RECONHECIMENTO DE VOZ *********
        public void Gramatica()
        {
            try
            {
                //reconhecedor = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao integrar lingua escolhida:" + ex.Message);
            }

            // criacao da gramatica simples que o programa vai entender
            // usando um objeto Choices
            var gramatica = new Choices();
            gramatica.Add(listaPalavras); // inclui a gramatica criada

            // cria o construtor gramatical
            // e passa o objeto criado com as palavras
            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            // cria a instancia e carrega a engine de reconhecimento
            // passando a gramatica construida anteriomente
            try
            {
                var g = new Grammar(gb);

                try
                {
                    // carrega o arquivo de gramatica
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);

                    // registra a voz como mecanismo de entrada para o evento de reconhecimento
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;

                    reconhecedor.SetInputToDefaultAudioDevice(); // microfone padrao
                    resposta.SetOutputToDefaultAudioDevice(); // auto falante padrao
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple); // multiplo reconhecimento
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramática: " + ex.Message);
            }
        }

        public void Init()
        {
            resposta.Volume = 100; // controla volume de saida
            resposta.Rate = 2; // velocidade de fala

            Gramatica(); // inicialização da gramatica
        }

        // funcao para reconhecimento de voz
        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;

            if (serialPort1.IsOpen)
            {
                if (frase.Equals("acender"))
                {
                    //resposta.SpeakAsync("acende luz");
                    AcenderLED();
                }

                if (frase.Equals("apagar"))
                {
                    //resposta.SpeakAsync("apaga luz");
                    ApagarLED();
                }
            }
            else
            {
                if(frase.Equals("acender") || frase.Equals("apagar"))
                {
                    resposta.SpeakAsync("nenhuma porta conectada");
                }
            }

        }

        private void AcenderLED()
        {
            if (serialPort1.IsOpen)
                serialPort1.Write("1");
        }

        private void ApagarLED()
        {
            if (serialPort1.IsOpen)
                serialPort1.Write("0");
        }

        // *********** EVENTOS ***************
        private void timerCOM_Tick(object sender, EventArgs e)
        {
            AtualizaListaCOMs();
        }

        private void btnConecta_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {
                try
                {
                    serialPort1.PortName = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                    serialPort1.Open();

                }
                catch
                {
                    return;

                }
                if (serialPort1.IsOpen)
                {
                    btnConecta.Text = "Desconectar";
                    comboBox1.Enabled = false;

                }
            }
            else
            {

                try
                {
                    serialPort1.Close();
                    comboBox1.Enabled = true;
                    btnConecta.Text = "Conectar";
                }
                catch
                {
                    return;
                }

            }

            if (btnConecta.Text.Equals("Desconectar"))
            {
                lblStatus.Text = "PODE FALAR";
            }
            else
            {
                lblStatus.Text = "CONECTE A UMA PORTA";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // se a porta estiver aberta, fecha ela
            if (serialPort1.IsOpen == true)
                serialPort1.Close();
        }
    }
}
