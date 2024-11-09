using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s13p05
{
    public partial class Form1 : Form
    {

        public static readonly Color[] colores = { Color.SkyBlue, Color.White };
        public static List<Nodo> nodos;
        public static int[,] laberinto;
        public Grafo grafo;
        public static Dictionary<string, int> dic;
        List<int> ruta;
        public static int pos;

        public int largo;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nodos = new List<Nodo>();
            btn_solucionar.Enabled = false;
            timer1.Interval = 80;

            largo = 6;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape) {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DibujarLaberinto();
        }

        private void DibujarLaberinto() {

            
            largo = int.Parse(numericUpDown1.Text);

            btn_solucionar.Enabled = true;
            pos = 0;
            timer1.Enabled = false;
            panel1.Controls.Clear();
            nodos = new List<Nodo>();
            laberinto = new int[largo, largo];

            label3.Text = pos.ToString();

            dic = new Dictionary<string, int>();

            int k = 0;

            int f0 = 0;
            int c0 = 0;

            Random rnd = new Random();

            int ancho = panel1.Width / largo;
            int alto = panel1.Height / largo;

            for (int i = 0; i < largo; i++)
            {
                for (int j = 0; j < largo; j++)
                {
                    int f1 = f0 + ancho * i;
                    int c1 = c0 + alto * j;
                    int cl = rnd.Next(0, 2);

                    if (i == 0 && j == 0)
                        cl = 1;
                    if (i == largo-1 && j == largo-1)
                        cl = 1;

                    PictureBox picture = new PictureBox();
                    panel1.Controls.Add(picture);
                    picture.Width = ancho;
                    picture.Height = alto;
                    picture.Location = new Point(f1, c1);
                    picture.BackColor = colores[cl];
                    picture.Name = "pic_" + i + "_" + j;
                    picture.Paint += picture_paint;
                    picture.MouseUp += (s, args) =>
                    {
                        if (args.Button == MouseButtons.Left)
                        {
                            picture_Click_left(s, args);
                        }
                        if (args.Button == MouseButtons.Right)
                        {
                            picture_Click_rigth(s, args);
                        }
                    };

                    laberinto[i, j] = cl;

                    Nodo nodo = new Nodo();
                    nodo.nombre = i + "," + j;
                    nodo.dato = k;
                    nodo.fila = i;
                    nodo.columna = j;
                    nodos.Add(nodo);
                    dic[nodo.nombre] = k;
                    k = k + 1;
                }
            }
        }

        private void MostrarLaberinto() {
           // Console.WriteLine("==============================================");
           // Console.WriteLine();
            for (int i = 0; i < largo; i++)
            {
                Console.Write("\t{0}", i);
            }
           // Console.WriteLine();
            for (int i = 0; i < largo; i++)
            {
                Console.Write(i);
                for (int j = 0; j < largo; j++)
                {
                    Console.Write("\t{0}", laberinto[j,i]);
                }
               // Console.WriteLine();
            }

           // Console.WriteLine("==============================================");
        }

        private void CalcularMAdj() {

            int nodo = 0;
            int inicio = 0;
            int final = (largo* largo)-1;
            int distancia = 0;
            int n = 0;
            int m = 0;
            int cantNodos = largo* largo;

            grafo = new Grafo(largo* largo);

           // Console.WriteLine("==============================================");

            for (int j = 0; j < largo; j++) {
                for (int i = 0; i < largo-1; i++)
                {
                    if((laberinto[i,j]==1) && (laberinto[i+1,j]==1))
                    {
                        //Console.WriteLine("{0}\t{1}\t{2}", i, j, laberinto[i,j]);
                        string nombre = j+","+i;
                        int nodo1 = dic[nombre];
                        nombre = j+","+ (i + 1);
                        int nodo2 = dic[nombre];

                        grafo.AdicionarArista(nodo1, nodo2);
                        grafo.AdicionarArista(nodo2, nodo1);
                    }
                }
            }

            for (int i = 0; i < largo; i++)
            {
                for (int j = 0; j < largo-1; j++)
                {
                    if ((laberinto[i, j] == 1) && (laberinto[i, j+1] == 1))
                    {
                        //Console.WriteLine("{0}\t{1}\t{2}", i, j, laberinto[i, j]);
                        string nombre = j+","+i;
                        int nodo1 = dic[nombre];
                        nombre = (j+1)+","+i;
                        int nodo2 = dic[nombre];

                        grafo.AdicionarArista(nodo1, nodo2);
                        grafo.AdicionarArista(nodo2, nodo1);
                    }
                }
            }



            //for (int j = 0; j < 6; j++)
            //{
            //    for (int i = 5; i > 0; i--)
            //    {
            //        if ((laberinto[i, j] == 1) && (laberinto[i - 1, j] == 1))
            //        {
            //           // Console.WriteLine("{0}\t{1}\t{2}", i, j, laberinto[i,j]);
            //            string nombre = j + "," + i;
            //            int nodo1 = dic[nombre];
            //            nombre = j + "," + (i + 1);
            //            int nodo2 = dic[nombre];

            //            grafo.AdicionarArista(nodo1, nodo2);
            //        }
            //    }
            //}


            //for (int i = 0; i < 6; i++)
            //{
            //    for (int j = 5; j > 0; j--)
            //    {
            //        if ((laberinto[i, j] == 1) && (laberinto[i, j - 1] == 1))
            //        {
            //           // Console.WriteLine("{0}\t{1}\t{2}", i, j, laberinto[i, j]);
            //            string nombre = j + "," + i;
            //            int nodo1 = dic[nombre];
            //            nombre = (j + 1) + "," + i;
            //            int nodo2 = dic[nombre];

            //            grafo.AdicionarArista(nodo1, nodo2);
            //        }
            //    }
            //}



            //grafo.MostrarAdyacencia();

           // Console.WriteLine("==============================================================");
           // Console.WriteLine("inicio\t:\t{0}", inicio);
           // Console.WriteLine("final\t:\t{0}", final);
           // Console.WriteLine("==============================================================");

           // Console.WriteLine();

            int[,] tabla;
            tabla = new int[cantNodos, 3];

            //0 visitado
            //1 distancia
            //2 previo

            for (n = 0; n < cantNodos; n++)
            {
                tabla[n, 0] = 0;
                tabla[n, 1] = int.MaxValue;
                tabla[n, 2] = 0;
            }

            tabla[inicio, 1] = 0;

            //MostrarTabla(tabla);

           // Console.WriteLine("==============================================================");


            for (distancia = 0; distancia < cantNodos; distancia++)
            {
                for (n = 0; n < cantNodos; n++)
                {
                    if ((tabla[n, 0] == 0) && (tabla[n, 1] == distancia))
                    {
                        tabla[n, 0] = 1;
                        for (m = 0; m < cantNodos; m++)
                        {
                            if (grafo.ObtenerAdyacencia(n, m) == 1)
                            {
                                if (tabla[m, 1] == int.MaxValue)
                                {
                                    tabla[m, 1] = distancia + 1;
                                    tabla[m, 2] = n;
                                }
                            }
                        }
                    }
                }
            }

            //MostrarTabla(tabla);

           // Console.WriteLine("==============================================================");

            //obtener la ruta

            ruta = new List<int>();

            nodo = final;

            while (nodo != inicio)
            {
                ruta.Add(nodo);
                nodo = tabla[nodo, 2];
            }

            ruta.Add(inicio);
            ruta.Reverse();

            //foreach (int pos in ruta)
            //{
            //    Console.Write("{0}\t", pos);
            //}

           // Console.WriteLine();
           // Console.WriteLine("==============================================================");

            if (ruta.Count > 2)
            {
                timer1.Enabled = true;
            }
            else
            {
                btn_dibujar.Enabled = true;
            }
            
        }



        public static void MostrarTabla(int[,] tabla)
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
               // Console.WriteLine("{0} --> {1}\t{2}\t{3}", i, tabla[i, 0], tabla[i, 1], tabla[i, 2]);
            }
           // Console.WriteLine();
        }



        void picture_Click_rigth(object sender, EventArgs e)
        {

        }

            void picture_Click_left(object sender, EventArgs e)
        {

            foreach (PictureBox panel in panel1.Controls) {
                if (panel.BackColor == Color.Violet) {
                    panel.BackColor = Color.White;
                }
            }

            label3.Text = "0";

            btn_solucionar.Enabled = true;
            PictureBox picture = (PictureBox)sender;
            string nombre = picture.Name;
            int i = Convert.ToInt32(nombre.Split('_')[1]);
            int j = Convert.ToInt32(nombre.Split('_')[2]);

            if ((i == 0) && (j == 0))
                return;
            if ((i == largo-1) && (j == largo-1))
                return;

            if (picture.BackColor == Color.SkyBlue) {
                picture.BackColor = Color.White;
                laberinto[i, j] = 1;
            }
            else
            {
                picture.BackColor = Color.SkyBlue;
                laberinto[i, j] = 0;
            }
        }

        void picture_paint(object sender, PaintEventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            string nombre = picture.Name;
            int i = Convert.ToInt32(nombre.Split('_')[1]);
            int j = Convert.ToInt32(nombre.Split('_')[2]);

            ControlPaint.DrawBorder(e.Graphics, picture.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);

            if (i == 0 && j == 0) {
                e.Graphics.DrawEllipse( new Pen(Color.Red, 4f), picture.Size.Width / 2-10, picture.Size.Height/2-10, picture.Size.Width/2, picture.Size.Height/2);
            }
            if (i == largo-1 && j == largo-1)
            {
                e.Graphics.DrawEllipse(new Pen(Color.Red, 4f), picture.Size.Width / 2 - 10, picture.Size.Height / 2 - 10, picture.Size.Width / 2, picture.Size.Height / 2);
            }

            //picture.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            btn_solucionar.Enabled = false;
            btn_dibujar.Enabled = false;
            label3.Text = "0";

            //MostrarLaberinto();
            CalcularMAdj();
        }


       
        private void timer1_Tick(object sender, EventArgs e)
        {
           // Console.WriteLine("{0}\t{1}",pos, ruta[pos]);
            
            int nodo = ruta[pos];
            int i = nodos[nodo].fila;
            int j = nodos[nodo].columna;
            string nombre= "pic_" + j + "_" + i;

            foreach (PictureBox picture in panel1.Controls) {
                if (picture.Name == nombre) {
                    picture.BackColor = Color.Violet;
                }
            }

            pos = pos + 1;

            label3.Text = pos.ToString();

            if (pos == ruta.Count) {
                timer1.Enabled = false;
                pos = 0;
                btn_dibujar.Enabled = true;
                
            }

            
                
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
