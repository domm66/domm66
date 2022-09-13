using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassPoint
{
    public partial class Form1 : Form
    {

        String[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/pics_passpoint");

        int osztasMerteke = 50;

        bool ujfelhasznalo = false;
        bool meglevofelhasznalo = false;

        int mousePosX; //szelesseg
        int mousePosY; //magassag

        int klikk = 0;
        int[] szelesseg = new int[15];
        int[] magassag = new int[15];
        int[] szelessegmentett = new int[15];
        int[] magassagmentett = new int[15];

        bool ujramegadas;

        int felhasznIndex;

        string felhasznJelszo;

        string ideiglenesJelszo;

        int ideiglenesIndex;

        bool letezik;

        int ujracount=0;

        int tolerancia = 15;

        felhasznalok[] accountok = new felhasznalok[1];
        deserializaltFelhaszn[] deserializalt;
        string jsonstring;

        bool kijeloltTorles = false;
        private bool huzas;
        private Point elhelyezkedes;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            textBox1.Enabled = true;
            button4.Enabled = false;
            textBox2.Enabled = true;
            button6.Visible = false;
            button6.Visible = true;
            Deserialize();
            this.ActiveControl = label3;
            textBox3.Text = "";          
        }

        public void Deserialize()
        {
            var path = Directory.GetCurrentDirectory() + @"/felhasznalok_Passpoint.json";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            else if (File.Exists(path))
            {
                string elolvasott = "[" + File.ReadAllText(path) + "]";
                deserializalt = JsonConvert.DeserializeObject<deserializaltFelhaszn[]>(elolvasott);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Deserialize();
            meglevofelhasznalo = false;
            ujfelhasznalo = true;                     
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            if (ujfelhasznalo && deserializalt.Length > 0)
            {
                for (int i = 0; i < deserializalt.Length; i++)
                {
                    if (deserializalt[i].felhasznaloNev == textBox1.Text)
                    {
                        letezik = true;
                        MessageBox.Show("Ilyen felhasználó már létezik!");
                        Reset();
                        break;
                    }
                }
                if (!letezik)
                {
                    
                    accountok[0] = (new felhasznalok
                    {
                        felhasznaloNev = textBox1.Text,
                        email = textBox2.Text,
                    });
                    RandomKep();
                    MessageBox.Show("Most adjon meg egy jelszavat, ami MINIMUM 5 kattintás, MAXIMUM korlát 8!");
                    pictureBox1.Visible = true;
                    pictureBox1.Enabled = true;
                    button2.Enabled = false;
                    button1.Enabled = false;
                    button5.Enabled = true;
                    Reset2();
                }
                letezik = false;
            }
            else if (ujfelhasznalo && deserializalt.Length == 0)
            {

                MessageBox.Show("Most adjon meg egy jelszavat!");
                pictureBox1.Enabled = true;
                pictureBox1.Visible = true;
                button2.Enabled = false;
                button1.Enabled = false;
                button5.Enabled = true;
                accountok[0] = (new felhasznalok
                {
                    felhasznaloNev = textBox1.Text,
                    email = textBox2.Text,
                });
                RandomKep();
            }
        }

        public void ChechMeglevo()
        {
            for (int i = 0; i < deserializalt.Length; i++)
            {
                if (deserializalt[i].felhasznaloNev == textBox1.Text && deserializalt[i].email == textBox2.Text)
                {
                    
                    felhasznIndex = i;
                    textBox3.Text = "";
                    MessageBox.Show("Felhasználó megtalálva, most adja meg a jelszavát!");
                    button1.Enabled = false;
                    button2.Enabled = false;
                    pictureBox1.Visible = true;
                    button5.Enabled = true;
                    pictureBox1.Image = Bitmap.FromFile(files[deserializalt[i].index]);
                    
                    break;
                }
                else { textBox3.Text = "Rossz felhasználó vagy jelszó!"; }
            }
        }

        public void RandomKep()
        {
            Random rand = new Random();
            int kepIndex = rand.Next(0, files.Length);
            pictureBox1.Image = Bitmap.FromFile(files[kepIndex]);
            accountok[0].index = kepIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Deserialize();
            meglevofelhasznalo = true;
            ujfelhasznalo = false;
           
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            
            pictureBox1.Enabled = true;
            ChechMeglevo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Deserialize();
            pictureBox1.Enabled = true;
            if (meglevofelhasznalo)
            {
                ChechMeglevo();
            }
            if (ujfelhasznalo && deserializalt.Length > 0)
            {
                for (int i = 0; i < deserializalt.Length; i++)
                {
                    if (deserializalt[i].felhasznaloNev == textBox1.Text)
                    {  
                        letezik = true;
                        MessageBox.Show("Ilyen felhasználó már létezik!");
                        Reset();
                        break;
                    }
                }
                if (!letezik)
                {
                   
                    accountok[0] = (new felhasznalok
                    {
                        felhasznaloNev = textBox1.Text,
                        email = textBox2.Text,
                    });
                    RandomKep();
                    MessageBox.Show("Most adjon meg egy jelszavat, ami MINIMUM 5 kattintás, MAXIMUM korlát nincs!");
                    pictureBox1.Visible = true;
                    Reset2();
                }
                letezik = false;
            }
            else if (ujfelhasznalo && deserializalt.Length == 0)
            {

                MessageBox.Show("Most adjon meg egy jelszavat!");
                pictureBox1.Visible = true;
                accountok[0] = (new felhasznalok
                {
                    felhasznaloNev = textBox1.Text,
                    email = textBox2.Text,
                });
                RandomKep();
            } 
        }
        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            kijeloltTorles = false;

            
            
            szelesseg[klikk] = (this.PointToClient(MousePosition).X - pictureBox1.Location.X);
            magassag[klikk] = (this.PointToClient(MousePosition).Y - pictureBox1.Location.Y);

            
            button3.Enabled = true;
            //kattintott helyek
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawRectangle(new Pen(Color.Red, 3), new Rectangle(e.X - tolerancia, e.Y - tolerancia, 30, 30));

            if (kijeloltTorles)
            {
                pictureBox1.Invalidate();
            }

            klikk++;
            
            if (klikk >= 5 && ujfelhasznalo && ujracount==0)
            {
                button6.Visible = true;
                button6.Enabled = true;    
                for(int i = 0; i < klikk; i++)
                {
                    szelessegmentett[i] = szelesseg[i];
                    magassagmentett[i] = magassag[i];
                }
                
            }            
            else if(klikk>=5 && meglevofelhasznalo)
            {
                button4.Enabled = true;
                button4.Visible = true;
            }
            if(ujracount>0)
            {
                button6.Enabled = false;
                button4.Enabled = true;
            }
            if(klikk==8)
            {
                MessageBox.Show("Elérte a maximum hosszát a jelszónak, kérem véglegesítse vagy adja meg újra.");
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        public void Reset()
        {
            button1.Visible = true;
            button2.Visible = true;
            button5.Enabled = false;
            button4.Enabled = false;
            textBox3.Text = "";
            textBox1.Clear();
            textBox1.Enabled = true;
            textBox2.Clear();
            textBox2.Enabled = true;
            textBox1.Text = "Felhasználó";
            textBox2.Text = "Email";
            textBox1.ForeColor = Color.Silver;
            textBox2.ForeColor = Color.Silver;
            pictureBox1.Image = null;
            meglevofelhasznalo = false;
            ujfelhasznalo = false;
            for (int i=0;i<klikk;i++)
            {
                szelessegmentett[i] = 0;
                magassagmentett[i] = 0;
                szelesseg[i] = 0;
                magassag[i] = 0;
            }
            klikk = 0;
            pictureBox1.Enabled = false;
            ujramegadas = false;
            button6.Enabled = false;
            ujracount = 0;
            pictureBox1.Visible = false;
            this.ActiveControl = label3;
            button3.Enabled = false;
        }
        public void Reset2()
        {
            button3.Enabled = false;
            textBox3.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            letezik = false;
            button6.Enabled = false;
            this.ActiveControl = label3;
        }

        public void Serializalas()
        {   
            for(int i=0;i<klikk;i++)
            {
                accountok[0].szelesseg[i] = szelessegmentett[i];
                accountok[0].magassag[i] = magassagmentett[i];
            }
            klikk = 0;

            if (deserializalt.Length == 0)
            {
                jsonstring = JsonConvert.SerializeObject(accountok[0], Formatting.Indented);
                File.AppendAllText(Directory.GetCurrentDirectory() + @"/felhasznalok_Passpoint.json", jsonstring);
            }
            else if (deserializalt.Length > 0)
            {
                jsonstring = "," + JsonConvert.SerializeObject(accountok[0], Formatting.Indented);
                File.AppendAllText(Directory.GetCurrentDirectory() + @"/felhasznalok_Passpoint.json", jsonstring);
            }
            MessageBox.Show("Sikeres regisztráció!");
            Reset();
        }

        private bool CheckHely()
        {
            int talalt=0;
            bool megegyezik = false;
            if(ujfelhasznalo)
            {
                for (int i = 0; i < klikk; i++)
                {
                    if ((szelessegmentett[i] - tolerancia < szelesseg[i] && szelessegmentett[i] + tolerancia > szelesseg[i]) && (magassagmentett[i] - tolerancia < magassag[i] && magassagmentett[i] + tolerancia > magassag[i]))
                    {
                        //MessageBox.Show((szelessegmentett[i] - 25).ToString() + "  " + szelesseg[i].ToString() +" " + (szelessegmentett[i] + 25).ToString() +"               " + (magassagmentett[i] - 25).ToString() + " " + magassag[i].ToString() + " " + (magassagmentett[i] + 25).ToString());
                        talalt++;
                    }

                }
                if (talalt == klikk)
                {
                    megegyezik= true;

                }
                
            }
            if(meglevofelhasznalo)
            {
                for (int i = 0; i < klikk; i++)
                {
                    if ((deserializalt[felhasznIndex].szelesseg[i] - tolerancia < szelesseg[i] && deserializalt[felhasznIndex].szelesseg[i] + tolerancia > szelesseg[i]) && (deserializalt[felhasznIndex].magassag[i] - tolerancia < magassag[i] && deserializalt[felhasznIndex].magassag[i] + tolerancia > magassag[i]))
                    {
                        //MessageBox.Show((deserializalt[felhasznIndex].szelesseg[i] - 25).ToString() + "<" + szelesseg[i].ToString() + "<" + (deserializalt[felhasznIndex].szelesseg[i] + 25).ToString() + "               " + (deserializalt[felhasznIndex].magassag[i] - 25).ToString() + "<" + magassag[i].ToString() + "<" + (deserializalt[felhasznIndex].magassag[i] + 25).ToString());
                        talalt++;
                    } else
                    {
                        //MessageBox.Show((deserializalt[felhasznIndex].szelesseg[i] - 25).ToString() + "<" + szelesseg[i].ToString() + "<" + (deserializalt[felhasznIndex].szelesseg[i] + 25).ToString() + "               " + (deserializalt[felhasznIndex].magassag[i] - 25).ToString() + "<" + magassag[i].ToString() + "<" + (deserializalt[felhasznIndex].magassag[i] + 25).ToString());

                    }

                }
                if (talalt == klikk)
                {
                    megegyezik = true;

                }
                
            }
            //MessageBox.Show(klikk.ToString() + "       " + talalt.ToString());
            return megegyezik ;
            talalt = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (meglevofelhasznalo && CheckHely())
            {
                MessageBox.Show("Sikeres bejelentkezés!");
                Reset();
            }
            else if(meglevofelhasznalo && !CheckHely())
            {
                MessageBox.Show("ROSSZ JELSZÓ!");
                Reset();
            }

            else if (ujfelhasznalo && CheckHely())
            {
                Serializalas();
            } else if ( ujfelhasznalo && !CheckHely())
            {
                MessageBox.Show("A jelszavak nem egyeznek, próbálja meg újra!");
                Reset();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Reset();
            letezik = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            if (ujfelhasznalo)
            {
                kijeloltTorles = true;
                this.Refresh();
                klikk = 0;
                button6.Enabled = false;
                button4.Visible = true;
                button4.Enabled = true;
                ideiglenesJelszo = felhasznJelszo;
                felhasznJelszo = "";
                ujracount++;
                this.ActiveControl = label3;
                MessageBox.Show("Adja meg a jelszavát újra!");   
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            
                int x, y;
                int w = pictureBox1.Size.Width;
                int h = pictureBox1.Size.Height;
                int inc = 50;
                Graphics gr = e.Graphics; 
                
                    for (x = 0; x < w; x += inc)
                        gr.DrawLine(Pens.White, x, 0, x, h);

                    for (y = 0; y < h; y += inc)
                        gr.DrawLine(Pens.White, 0, y, w, y);
                
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length>2 && textBox2.Text.Length >2 && textBox1.Text != "Felhasználó" && textBox2.Text != "Email")
            {
                button1.Enabled = true;
                button2.Enabled = true;
            } else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 2 && textBox2.Text.Length > 2 && textBox1.Text != "Felhasználó" && textBox2.Text != "Email")
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Felhasználó")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Felhasználó";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Email")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Email";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            huzas = true;
            elhelyezkedes.X = e.X;
            elhelyezkedes.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (huzas == true)
            {
                Point formElhelyezkedes = PointToScreen(e.Location);
                Location = new Point(formElhelyezkedes.X - elhelyezkedes.X, formElhelyezkedes.Y - elhelyezkedes.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            huzas = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            button6.Enabled = false;
            pictureBox1.Invalidate();
            felhasznJelszo = "";
            for(int i=0;i<klikk;i++)
            {
                accountok[0].szelesseg[i] = 0;
                accountok[0].magassag[i] = 0;
            }
            klikk = 0;
        }
    }
}
