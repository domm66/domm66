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
using Newtonsoft.Json;

namespace JetafidaScheme
{
    public partial class Form1 : Form
    {
        String[] fileok = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/pics_main");

        

        bool ujfelhasznalo = false;
        bool meglevofelhasznalo = false;



        int locx = 308;
        int locy = -110;

        int felhasznIndex;

        PictureBox[] kepek = new PictureBox[30];

        felhasznalok[] accountok = new felhasznalok[1];
        deserializaltFelhaszn[] deserializalt;
        string jsonstring;

        int[] randomSzamokIndex = new int[36];

        bool letezik = false;

        int[] tempIndex = new int[10];
        int[] generalasHelye = new int[9];
        string jelszoIdeglenes;

        string jelszo;
        int kattintas = 0;

        static Random rnd = new Random();

        
        int bk = 0;

        bool gyakorlas = false;

        bool huzas = false;
        Point elhelyezkedes;

        String megnyomott;

        public Form1()
        {
            
            Array.Sort(fileok);
            this.Size = new Size(375, 670);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string path = Server.MapPath("~/App_Data/");
            // Write that JSON to txt file,  
            this.ActiveControl = panel1;
            textBox4.Visible = false;
            Deserializacio();
            
            
        }

        public void JelszoGeneralas()
        {
            if(pictureBox1.Image!=null)
            {
                pictureBox1.Image.Dispose();
                pictureBox2.Image.Dispose();
                pictureBox3.Image.Dispose();
                pictureBox4.Image.Dispose();
            }

            //jelszo generalas
            int elso= rnd.Next(0, 9);
            int masodik = rnd.Next(0, 9)+9;
            int harmadik = rnd.Next(0, 9)+18;
            int negyedik = rnd.Next(0, 9)+27;
        
            pictureBox1.Image = Bitmap.FromFile(fileok[randomSzamokIndex[elso]]);
            pictureBox2.Image = Bitmap.FromFile(fileok[randomSzamokIndex[masodik]]);
            pictureBox3.Image = Bitmap.FromFile(fileok[randomSzamokIndex[harmadik]]);
            pictureBox4.Image = Bitmap.FromFile(fileok[randomSzamokIndex[negyedik]]);

            jelszo = randomSzamokIndex[elso].ToString() + (randomSzamokIndex[masodik]).ToString() + (randomSzamokIndex[harmadik]).ToString() + (randomSzamokIndex[negyedik]).ToString();
             
        }

        public void RandomSzamokIndexGeneralas()
        {
            //random szamok generalasa ismetlodes nelkul
            Random rand = new Random();
            List<int> possible = Enumerable.Range(0, fileok.Length).ToList();

            for (int i = 0; i < 36; i++)
            {
                int index = rand.Next(0, possible.Count);
                randomSzamokIndex[i] = possible[index];
                possible.RemoveAt(index);
            }
        }

        public void HelyGeneralas()
        {
            Random rand = new Random();
            List<int> possible = Enumerable.Range(0, 9).ToList();

            for (int i = 0; i < 9; i++)
            {
                int index = rand.Next(0, possible.Count);
                generalasHelye[i] = possible[index];
                possible.RemoveAt(index);
                
            }
        }

        public void RandomKep()
        {
            
            HelyGeneralas();
            if (ujfelhasznalo)
            {
                //pictureboxok generalasa
                for (int i = 0; i < 9; i++)
                {
                    if (i % 3 != 0)
                    {
                        locx += 120;
                        
                    }
                    else if (i % 3 == 0)
                    {
                        locx = 308;
                        locy += 150;
                    }
                    
                    kepek[generalasHelye[i]] = new PictureBox
                    {
                        Name = randomSzamokIndex[i].ToString(),
                        Size = new Size(100, 130),
                        Location = new Point(locx, locy),
                        Visible = true,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Image = Image.FromFile(fileok[randomSzamokIndex[i]]),
                    Enabled = true,
                    };

                    kepek[generalasHelye[i]].Click += Form1_Click;
                    Controls.Add(kepek[generalasHelye[i]]);
                    kepek[generalasHelye[i]].Show();
                    
                }
            }
            if(meglevofelhasznalo)
            {
                //pictureboxok generalasa
                for (int i = 0; i < 9; i++)
                {
                    if (i % 3 != 0)
                    {
                        locx += 120;
                    }
                    else if (i % 3 == 0)
                    {
                        locx = 308;
                        locy += 150;
                    }
                    Bitmap bmp = new Bitmap(fileok[deserializalt[felhasznIndex].indexek[i]]);
                    kepek[i] = new PictureBox
                    {
                        Name = deserializalt[felhasznIndex].indexek[i].ToString(),
                        Size = new Size(100, 130),
                        Location = new Point(locx, locy),
                        Visible = true,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Image = bmp,
                        Enabled = true,
                    };

                    kepek[i].Click += Form1_Click;
                    Controls.Add(kepek[i]);
                    kepek[i].Show();
                }
            }
            
            
        }

        public void ChechMeglevo()
        {
            //letezo felhasznalo kereses
            for (int i = 0; i < deserializalt.Length; i++)
            {
                if (deserializalt[i].felhasznaloNev == textBox1.Text && deserializalt[i].email == textBox2.Text)
                {
                    felhasznIndex = i;
                    meglevofelhasznalo = true;
                    break;
                }
                else { meglevofelhasznalo = false; }
            }
            if (meglevofelhasznalo)
            {
                MessageBox.Show("Felhasználó megtalálva, most adja meg a jelszavát!");
                RandomKep();
                button1.Enabled = false;
                button2.Enabled = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                textBox3.Text = "Üdvözlünk " + deserializalt[felhasznIndex].felhasznaloNev + "!";
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
            else { textBox3.Text = "Adatok nem egyeznek"; }
        }
        public void Serializacio()
        {
            
                accountok[0] = (new felhasznalok
                {
                    felhasznaloNev = textBox1.Text,
                    email = textBox2.Text,
                    jelszo = jelszo
                });
                for(int i=0;i<36;i++)
            {
                accountok[0].indexek[i] = randomSzamokIndex[i];
            }
                button1.Enabled = true;
                if (deserializalt.Length == 0)
                {
                    jsonstring = JsonConvert.SerializeObject(accountok[0], Formatting.Indented);
                    File.AppendAllText(Directory.GetCurrentDirectory() + @"\felhasznalok_Passfaces.json", jsonstring);
                }
                else if (deserializalt.Length > 0)
                {
                    jsonstring = "," + JsonConvert.SerializeObject(accountok[0], Formatting.Indented);
                    File.AppendAllText(Directory.GetCurrentDirectory() + @"\felhasznalok_Passfaces.json", jsonstring);
                }             
        }

        public void Deserializacio()
        {
            var path = Directory.GetCurrentDirectory() + @"\felhasznalok_Passfaces.json";
            if (File.Exists(path))
            {
                string elolvasott = "[" + File.ReadAllText(path) + "]";
                deserializalt = JsonConvert.DeserializeObject<deserializaltFelhaszn[]>(elolvasott);
            } else if(!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Deserializacio();
            ujfelhasznalo = false;
            ChechMeglevo();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RandomSzamokIndexGeneralas();
            Deserializacio();
            meglevofelhasznalo = false;
            this.ActiveControl = label3;
            if (deserializalt.Length > 0)
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
                    JelszoGeneralas();
                    textBox4.Text = "Jegyezze meg a arcokat, ez lesz ezentúl a jelszó amit bejelentkezéshez fog használni. " +
                    "Ha új jelszót szeretne kérni, akkor kattintsot az 'Újat' gombra. Ha úgy érzi hogy megjegyezte eléggé az arcokat, akkor kattintson a 'KÉSZ' gombra, " +
                    "ezután párszor visszakérdezi a program az arcokat, hogy biztosan megjegyezze.";
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    pictureBox3.Visible = true;
                    pictureBox4.Visible = true;
                    textBox4.Visible = true;
                    panel6.Visible = true;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button5.Enabled = true;
                    button6.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    ujfelhasznalo = true;

                }

                letezik = false;
            }
            else if (deserializalt.Length == 0)
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
                textBox4.Visible = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button5.Enabled = true;
                button6.Visible = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                ujfelhasznalo = true;

                panel6.Visible = true;
                JelszoGeneralas();
                textBox4.Text ="Jegyezze meg a arcokat, ez lesz ezentúl a jelszó amit bejelentkezéshez fog használni. " +
                    "Ha új jelszót szeretne kérni, akkor kattintsot az 'Újat' gombra. Ha úgy érzi hogy megjegyezte eléggé az arcokat, akkor kattintson a 'KÉSZ' gombra, " +
                    "ezután párszor visszakérdezi a program az arcokat, hogy biztosan megjegyezze.";
            }

        }

        public void Keveres() 
        {
            Random r = new Random();
            randomSzamokIndex = randomSzamokIndex.OrderBy(x => r.Next()).ToArray();
        }

        public void Tisztit()
        {
            for (int i = 0; i < 9; i++)
            {                
                kepek[i].BackColor = Color.Transparent;
                kepek[i].Padding = new Padding(0);
                kepek[i].Enabled = true;
            }
            //MessageBox.Show(jelszoIdeglenes + " " + jelszoIdeglenes.Length + " " + megnyomott.Length + " " + (jelszoIdeglenes.Length - megnyomott.Length));
            jelszoIdeglenes=jelszoIdeglenes.Remove(jelszoIdeglenes.Length - megnyomott.Length);
            kattintas--;
            //MessageBox.Show(jelszoIdeglenes);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            PictureBox temp = sender as PictureBox;

            temp.BackColor = Color.Crimson;
            temp.Padding = new Padding(3);
            button4.Enabled = true;
            button9.Visible = true;

            jelszoIdeglenes += temp.Name;
            kattintas++;
            megnyomott = temp.Name;
            
            for (int i = 0; i < 9; i++)
            {
                kepek[i].Enabled = false;
            }
            
            //gyakorlas resz
            if (ujfelhasznalo && gyakorlas && kattintas == 4 && jelszo == jelszoIdeglenes)
            {
                button7.Enabled = true;
                button8.Enabled = true;
                button4.Enabled = false;
                kattintas = 0;
                
                bk = 0;

                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose();
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }

                for (int k = 0; k < 9; k++)
                {
                    kepek[k].Visible = false;
                    kepek[k].Name = randomSzamokIndex[k].ToString();
                    kepek[k].Image = Image.FromFile(fileok[randomSzamokIndex[k]]);
                    kepek[k].Enabled = true;
                }
                MessageBox.Show("Ha szeretne még gyakorolni akkor kattintson a 'GYAKORLÁS' gombra, ha pedig már eleget gyakorolt akkor a 'VÉGLEGESíT' gombra.");
                button9.Visible = false;
                jelszoIdeglenes = "";
            }
            if (ujfelhasznalo && gyakorlas && kattintas == 4 && jelszo != jelszoIdeglenes)
            {
                button7.Enabled = true;
                button8.Enabled = true;
                button4.Enabled = false;
                kattintas = 0;

                bk = 0;

                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose();
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }

                for (int k = 0; k < 9; k++)
                {
                    kepek[k].Visible = false;
                    kepek[k].Name = randomSzamokIndex[k].ToString();
                    kepek[k].Image = Image.FromFile(fileok[randomSzamokIndex[k]]);
                    kepek[k].Enabled = true;
                }
                MessageBox.Show("Nem jó jelszót adott meg!");
                jelszoIdeglenes = "";
            }
            //uj felhasznalo resz

            if (ujfelhasznalo && !gyakorlas && kattintas == 4 && jelszo == jelszoIdeglenes)
            {
                MessageBox.Show("Adja meg a jelszavát újra!");
                button4.Enabled = false;
                button9.Visible = false;
                jelszoIdeglenes = "";
                bk = 0;

                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose();
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }

                for (int k = 0; k < 9; k++)
                {
                    kepek[k].Name = randomSzamokIndex[k].ToString();
                    kepek[k].Image = Image.FromFile(fileok[randomSzamokIndex[k]]);
                    kepek[k].Enabled = true;
                    
                }
                
            }
                    
            else if (ujfelhasznalo && !gyakorlas && kattintas == 4 && jelszo != jelszoIdeglenes)
            {
                MessageBox.Show("Jelszó nem egyezik!");     
                ResetKepek();
                Reset();
            }
                
            if (ujfelhasznalo && kattintas == 8 && jelszo == jelszoIdeglenes && !gyakorlas)
            {
                MessageBox.Show("Jelszó egyezik, ha úgy érzi megjegyezte a jelszavát eléggé, akkor a véglegesítéshez kattintson a 'VÉGLEGESíT' gonbra, ha" +
                    "még szeretné gyakorolni a jelszavát, akkor kattintson a 'GYAKORLÁS' gombra.");
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Visible = false;
                button4.Enabled = false;
                
                bk = 0;
                kattintas = 0;
                jelszoIdeglenes = "";
                ResetKepek();
            }
            else if (ujfelhasznalo && kattintas == 8 && jelszo != jelszoIdeglenes)
            {
                MessageBox.Show("Jelszó nem egyezik!");
                ResetKepek();
                Reset();
            }

            //meglevo felhasznalo resz

            if (meglevofelhasznalo && kattintas == 4 && deserializalt[felhasznIndex].jelszo == jelszoIdeglenes)
            {
                MessageBox.Show("Sikeres bejelentkezés!");

                ResetKepek();
                Reset();
            }
            else if (meglevofelhasznalo && kattintas == 4 && deserializalt[felhasznIndex].jelszo != jelszoIdeglenes)
            {
                MessageBox.Show("ROSSZ JELSZÓ!");

                ResetKepek();
                Reset();
            }
        }

        public void Reset()
        {
            textBox1.Clear();
            textBox2.Clear();
            meglevofelhasznalo = false;
            ujfelhasznalo = false;
            kattintas = 0;
            jelszoIdeglenes = "";
            jelszo = "";
            locx = 308;
            locy = -110;
            label3.Text = "";
            button3.Enabled = false;
            bk = 0;
            button4.Enabled = false;
            label4.Text = "";
            button7.Enabled = false;
            button8.Enabled = false;
            button6.Visible = false;
            textBox1.Text = "Felhasználó";
            textBox2.Text = "email@pelda.hu";
            textBox2.ForeColor = Color.Gray;
            textBox1.ForeColor = Color.Gray;
            this.ActiveControl = panel1;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Clear();
            textBox4.Visible = false;
            panel6.Visible = false;
            button9.Visible = false;
        }

        public void ResetKepek()
        {
            for (int i = 0; i < 9; i++)
            {
                kepek[i].Visible = false;
                kepek[i].Image.Dispose(); //ez kellett hogy ne egye meg az osszes ramot       
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.TextLength>5 && textBox2.TextLength > 1 && textBox1.Text!="Felhasználó" && textBox2.Text != "email@pelda.hu")
            {
                button1.Enabled = true;
                button2.Enabled = true;
                textBox3.Text = "";
            } else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                textBox3.Text = "Felhasználónév vagy Email túl rövid!";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 5 && textBox2.TextLength > 1 && textBox1.Text != "Felhasználó" && textBox2.Text != "email@pelda.hu")
            {
                button1.Enabled = true;
                button2.Enabled = true;
                textBox3.Text = "";
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                textBox3.Text = "Felhasználónév vagy Email túl rövid!";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reset();
            ResetKepek();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button5.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            panel6.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            textBox4.Visible = false;
            RandomKep();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button9.Visible = false;
            button4.Enabled = false;
            button3.Enabled = true;
            HelyGeneralas();


            if (gyakorlas)
            {
                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose();
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }
                bk += 9;
                for (int k = 0; k < 9; k++)
                {
                    kepek[generalasHelye[k]].Visible = true;
                    kepek[generalasHelye[k]].Name = randomSzamokIndex[k + bk].ToString();
                    kepek[generalasHelye[k]].Image = Image.FromFile(fileok[randomSzamokIndex[k + bk]]);
                    kepek[generalasHelye[k]].Enabled = true;
                }
              
            }
            if(ujfelhasznalo && !gyakorlas)
            {
                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose(); //ez kellett hogy ne egye meg az osszes ramot
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }

                bk += 9;
                for (int k = 0; k < 9; k++)
                {
                    kepek[k].Name = randomSzamokIndex[k + bk].ToString();
                    kepek[k].Image = Image.FromFile(fileok[randomSzamokIndex[k + bk]]);
                    kepek[k].Enabled = true;
                }
            }
            if(meglevofelhasznalo && !gyakorlas)
            {
                for (int i = 0; i < 9; i++)
                {
                    kepek[i].Image.Dispose(); //ez kellett hogy ne egye meg az osszes ramot
                    kepek[i].BackColor = Color.Transparent;
                    kepek[i].Padding = new Padding(0);
                }

                bk += 9;
                for (int k = 0; k < 9; k++)
                {
                    kepek[k].Name = deserializalt[felhasznIndex].indexek[k + bk].ToString();
                    kepek[k].Image = Image.FromFile(fileok[deserializalt[felhasznIndex].indexek[k + bk]]);
                    kepek[k].Enabled = true;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            JelszoGeneralas();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Serializacio();
            Reset();
            
            MessageBox.Show("Sikeres regisztráció!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HelyGeneralas();
            button4.Enabled = false;
            button3.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = false;
            MessageBox.Show("Adja meg a jelszavát újra!");

            gyakorlas = true;
            for (int i = 0; i < 9; i++)
            {
                kepek[i].Image.Dispose();
                kepek[i].BackColor = Color.Transparent;
                kepek[i].Padding = new Padding(0);
            }
            
            //kepek generalasa
            for (int k = 0; k < 9; k++)
            {
               
                kepek[generalasHelye[k]].Name = randomSzamokIndex[k].ToString();               
                kepek[generalasHelye[k]].Image = Image.FromFile(fileok[randomSzamokIndex[k]]);
                kepek[generalasHelye[k]].Enabled = true;
                kepek[generalasHelye[k]].Visible = true;
            }
            
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Felhasználó")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.White;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Felhasználó";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "email@pelda.hu")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.White;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "email@pelda.hu";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            huzas = true;
            elhelyezkedes.X = e.X;
            elhelyezkedes.Y = e.Y;
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (huzas == true)
            {
                Point formElhelyezkedes = PointToScreen(e.Location);
                Location = new Point(formElhelyezkedes.X - elhelyezkedes.X, formElhelyezkedes.Y - elhelyezkedes.Y);
            }
        }

        private void panel5_MouseUp(object sender, MouseEventArgs e)
        {
            huzas = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Tisztit();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
} 

