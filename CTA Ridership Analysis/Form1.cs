//
// CTA Ridership analysis using C# and SQL Server
//
// Rohit Nambiar
// U. of Illinois, Chicago

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CTA_Ridership_Analysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"Select Name from Stations 
                                         order by Name asc;");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox3.Items.Clear();
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox6.Clear();
            this.textBox7.Clear();
            this.textBox8.Clear();
            this.textBox9.Clear();
            foreach (DataRow row in ds.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["NAME"]);
                this.listBox1.Items.Add(msg);
            }
            db.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"Select top 10 Name from Stations
                                         Inner Join Riderships
                                         on Stations.StationID = Riderships.StationID
                                         Group by Name
                                         order by Sum(Riderships.DailyTotal) desc;");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox3.Items.Clear();
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox6.Clear();
            this.textBox7.Clear();
            this.textBox8.Clear();
            this.textBox9.Clear();
            foreach (DataRow row in ds.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["Name"]);
                this.listBox1.Items.Add(msg);
            }
            db.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = listBox1.GetItemText(listBox1.SelectedItem);
            text = text.Replace("'", "''");
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // total ridership
      
            string sql = string.Format(@"Select Sum(DailyTotal) as total from Riderships
                                         Inner join Stations 
                                         On Riderships.StationID = Stations.StationID 
                                         and Stations.Name = '{0}';", text);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            this.textBox1.Clear();
            foreach (DataRow row in ds.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:n0}", row["total"]);
                this.textBox1.AppendText(msg);
            }

            // avg ridership

            string sql2 = string.Format(@"Select Avg(DailyTotal) as avg from Riderships
                                          Inner Join Stations
                                          On Riderships.StationID = Stations.StationID 
                                          and Stations.Name = '{0}';", text);
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = db;
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            cmd2.CommandText = sql2;
            adapter2.Fill(ds2);
            this.textBox2.Clear();
            foreach (DataRow row in ds2.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:n0}", row["avg"]);
                this.textBox2.AppendText(msg + "/day");
            }
           
            // % ridership
            
            string sql3 = string.Format(@"Select (sum2/sum1 * 100) as percnt
                                          From
                                          (Select Sum(cast((DailyTotal) as float)) as sum1 from Riderships) as T1
                                          ,
                                          (Select Sum(DailyTotal) as sum2 from Riderships
                                          Inner Join Stations
                                          On Riderships.StationID = Stations.StationID 
                                          and Stations.Name = '{0}') as T2;", text);
            
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = db;
            SqlDataAdapter adapter3 = new SqlDataAdapter(cmd3);
            DataSet ds3 = new DataSet();
            cmd3.CommandText = sql3;
            adapter3.Fill(ds3);
            this.textBox3.Clear();
            foreach (DataRow row in ds3.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:0.00}", row["percnt"]);
                this.textBox3.AppendText(msg + '%');
            }
            
            // weekday

            string sql4 = string.Format(@"Select Sum(DailyTotal) as sum from Riderships
                                          Inner Join Stations
                                          On Riderships.StationID = Stations.StationID 
                                          and Stations.Name = '{0}'
                                          and Riderships.TypeOfDay = 'W';", text);
            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = db;
            SqlDataAdapter adapter4 = new SqlDataAdapter(cmd4);
            DataSet ds4 = new DataSet();
            cmd4.CommandText = sql4;
            adapter4.Fill(ds4);
            this.textBox4.Clear();
            foreach (DataRow row in ds4.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:n0}", row["sum"]);
                this.textBox4.AppendText(msg);
            }

            // saturday

            string sql5 = string.Format(@"Select Sum(DailyTotal) as sum from Riderships
                                          Inner Join Stations
                                          On Riderships.StationID = Stations.StationID 
                                          and Stations.Name = '{0}'
                                          and Riderships.TypeOfDay = 'A';", text);
            SqlCommand cmd5 = new SqlCommand();
            cmd5.Connection = db;
            SqlDataAdapter adapter5 = new SqlDataAdapter(cmd5);
            DataSet ds5 = new DataSet();
            cmd5.CommandText = sql5;
            adapter5.Fill(ds5);
            this.textBox5.Clear();
            foreach (DataRow row in ds5.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:n0}", row["sum"]);
                this.textBox5.AppendText(msg);
            }

            // sunday or holiday

            string sql6 = string.Format(@"Select Sum(DailyTotal) as sum from Riderships
                                          Inner Join Stations
                                          On Riderships.StationID = Stations.StationID 
                                          and Stations.Name = '{0}'
                                          and Riderships.TypeOfDay = 'U';", text);
            SqlCommand cmd6 = new SqlCommand();
            cmd6.Connection = db;
            SqlDataAdapter adapter6 = new SqlDataAdapter(cmd6);
            DataSet ds6 = new DataSet();
            cmd6.CommandText = sql6;
            adapter6.Fill(ds6);
            this.textBox6.Clear();
            foreach (DataRow row in ds6.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:n0}", row["sum"]);
                this.textBox6.AppendText(msg);
            }

            // stops at this station

            string sql7 = string.Format(@"Select Stops.Name as names from Stops
                                          Inner Join Stations
                                          On Stops.StationID = Stations.StationID
                                          and Stations.Name = '{0}';", text);
            SqlCommand cmd7 = new SqlCommand();
            cmd7.Connection = db;
            SqlDataAdapter adapter7 = new SqlDataAdapter(cmd7);
            DataSet ds7 = new DataSet();
            cmd7.CommandText = sql7;
            adapter7.Fill(ds7);
            this.listBox2.Items.Clear();
            this.textBox7.Clear();
            this.textBox8.Clear();
            this.textBox9.Clear();
            this.listBox3.Items.Clear();
            foreach (DataRow row in ds7.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["names"]);
                this.listBox2.Items.Add(msg);
            }

            db.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = listBox2.GetItemText(listBox2.SelectedItem);
            text = text.Replace("'", "''");
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // handicap accessible
            
            string sql = string.Format(@"Select ADA as handicap from Stops
                                         where Stops.Name = '{0}';", text);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            this.textBox7.Clear();
            foreach (DataRow row in ds.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["handicap"].ToString());
                this.textBox7.AppendText(msg);
            }
            
            // direction of travel
            
            string sql2 = string.Format(@"Select Stops.Direction as travel from Stops
                                          where Stops.Name = '{0}';", text);
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = db;
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            cmd2.CommandText = sql2;
            adapter2.Fill(ds2);
            this.textBox8.Clear();
            foreach (DataRow row in ds2.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["travel"].ToString());
                this.textBox8.AppendText(msg);
            }
            
            // Location
            
            string sql3 = string.Format(@"Select Latitude as lat, Longitude as lon from Stops
                                          where Stops.Name = '{0}';", text);
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = db;
            SqlDataAdapter adapter3 = new SqlDataAdapter(cmd3);
            DataSet ds3 = new DataSet();
            cmd3.CommandText = sql3;
            adapter3.Fill(ds3);
            this.textBox9.Clear();
            foreach (DataRow row in ds3.Tables["Table"].Rows)
            {
                string msg = string.Format("{0:0.0000}", row["lat"]);
                string msg2 = string.Format("{0:0.0000}", row["lon"]);
                this.textBox9.AppendText('(' + msg + ", " + msg2 + ')');
            }
            
            // Lines
            
            string sql4 = string.Format(@"Select Color as col from Lines
                                          Inner Join StopDetails
                                          On Lines.LineID = StopDetails.LineID
                                          Inner Join Stops
                                          On StopDetails.StopID = Stops.StopID
                                          and Stops.Name = '{0}';", text);
            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = db;
            SqlDataAdapter adapter4 = new SqlDataAdapter(cmd4);
            DataSet ds4 = new DataSet();
            cmd4.CommandText = sql4;
            adapter4.Fill(ds4);
            this.listBox3.Items.Clear();
            foreach (DataRow row in ds4.Tables["Table"].Rows)
            {
                string msg = string.Format("{0}", row["col"]);
                this.listBox3.Items.Add(msg);
            }
            
            db.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
