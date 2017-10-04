using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Buoi3
{
    public partial class Form1 : Form
    {
        private SqlConnection cn = null;
      

        public Form1()
        {
            InitializeComponent();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM LoaiSP WHERE MaLoaiSP =" + textBox1.Text;
            SqlCommand cmd = new SqlCommand(sql, cn);
            Connect();
            int numberofRows = 0;
            numberofRows = cmd.ExecuteNonQuery();
            Disconnect();
            MessageBox.Show("số dòng đã xóa: " + numberofRows.ToString());
            dataview.DataSource = GetData();
        }
        private void Connect()
        {
            try 
            {
                if (cn != null && cn.State != ConnectionState.Open)
                {
                    cn.Open();
                }
            }
            catch(InvalidOperationException ex)
                {
                    MessageBox.Show("Cannot open a connection without specifying a data source or server /n" + ex.Message);
                }
            catch(SqlException ex)
                {
                MessageBox.Show("A connection-level error occurred while opening the connection /n"+ ex.Message);
                }
            catch(ConstraintException ex)
                {
                MessageBox.Show("There are two entries with the same name in the <localdbinstances> section \n" + ex.Message);
                }
          
        }
        private void Disconnect()
        {
            if (cn != null && cn.State != ConnectionState.Closed)
            {
                cn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string cnStr = "Server = .; Database = QLBanHang; Integrated security = true;";
            cn = new SqlConnection(cnStr);
            dataview.DataSource = GetData(); // xuất bảng dữ liệu
        }

        private List<object> GetData()
        {
            Connect();
            string sql1 = "SELECT * FROM LoaiSp"; // câu lệnh query chọn bảng Loại SP
            List<object> list = new List<object>(); // tạo 1 list chứa các object
            try
            {
                SqlCommand cmd = new SqlCommand(sql1, cn);
                SqlDataReader dr = cmd.ExecuteReader();

                //dataview.DataSource = GetData();

                string name;
                int id;
                while(dr.Read())
                {
                    id = dr.GetInt32(0);
                    name = dr.GetString(1);

                    var Prod = new
                    {
                        MaLoaiSP = id,
                        TenLoaiSP = name
                    };
                list.Add(Prod);
                
                }
            dr.Close();

            }

            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return list;
            
        }

        
    }
}
