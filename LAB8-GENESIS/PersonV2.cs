﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LAB8_GENESIS
{
    class PersonV2: Person
    {
            private string cellphone;
            private string instagram;

            public string CellPhone
            {
                get
                {
                    return cellphone;
                }
                set
                {
                    //cellphone = value;
                    if (ValidationLibrary.ValidatePhoneNumber(value))
                    {
                        cellphone = value;
                    }
                    else
                    {
                    Feedback += "ERROR: Enter the correct format.";
                    }
                }

            }
            public string Instagram
            {
                get
                {
                    return instagram;
                }
                set
                {
                    instagram = value;
                }

            }
            public string AddARecord()
            {
                string strResult = "";

                SqlConnection Conn = new SqlConnection();

                Conn.ConnectionString = @GetConnected();

                string strSQL = "INSERT INTO Person (FirstName, MiddleName, LastName, Street1, City, State, ZipCode, Street2, PhoneNumber, Email, CellPhone, Instagram) VALUES (@FirstName, @MiddleName, @LastName, @Street1, @City, @State, @ZipCode, @Street2, @PhoneNumber, @Email, @CellPhone, @Instagram)";

                SqlCommand comm = new SqlCommand();
                comm.CommandText = strSQL;
                comm.Connection = Conn;

                comm.Parameters.AddWithValue("@FirstName", FirstName);
                comm.Parameters.AddWithValue("@MiddleName", MiddleName);
                comm.Parameters.AddWithValue("@LastName", LastName);

                comm.Parameters.AddWithValue("@Street1", Street1);
                comm.Parameters.AddWithValue("@City", City);
                comm.Parameters.AddWithValue("@State", State);
                comm.Parameters.AddWithValue("@ZipCode", ZipCode);

                comm.Parameters.AddWithValue("@Street2", Street2);

                comm.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                comm.Parameters.AddWithValue("@Email", Email);

                comm.Parameters.AddWithValue("@CellPhone", CellPhone);
                comm.Parameters.AddWithValue("@Instagram", Instagram);

                try
                {
                    Conn.Open();
                    int intRecs = comm.ExecuteNonQuery();
                    strResult = $"SUCCESS: Inserted {intRecs} records.";
                    Conn.Close();
                }
                catch (Exception err)
                {
                    strResult = "ERROR: " + err.Message;
                }
                finally
                {

                }
                return strResult;

            }

        public DataSet SearchPersonV2(String strLastName, String strPhoneNumber, String strCellPhone)
        {
            DataSet ds = new DataSet();


            SqlCommand comm = new SqlCommand();

            String strSQL = "SELECT PersonV2_ID, LastName, PhoneNumber, CellPhone FROM Person WHERE 0=0";

            if (strLastName.Length > 0)
            {
                strSQL += " AND LastName LIKE @LastName";
                comm.Parameters.AddWithValue("@LastName", "%" + strLastName + "%");
            }
            if (strPhoneNumber.Length > 0)
            {
                strSQL += " AND PhoneNumber LIKE @PhoneNumber";
                comm.Parameters.AddWithValue("@PhoneNumber", "%" + strPhoneNumber + "%");
            }
            if (strCellPhone.Length > 0)
            {
                strSQL += " AND CellPhone LIKE @CellPhone";
                comm.Parameters.AddWithValue("@CellPhone", "%" + strCellPhone + "%");
            }

            SqlConnection conn = new SqlConnection();
            string strConn = @GetConnected();
            conn.ConnectionString = strConn;

            comm.Connection = conn;
            comm.CommandText = strSQL;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comm;

            conn.Open();
            da.Fill(ds, "PersonV2_Temp");
            conn.Close();
            return ds;
        }

        public SqlDataReader FindOnePersonV2(int intPersonV2_ID)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand comm = new SqlCommand();

            string strConn = GetConnected();

            string sqlString =
           "SELECT * FROM Person WHERE PersonV2_ID = @PersonV2_ID;";

            conn.ConnectionString = strConn;

            comm.Connection = conn;
            comm.CommandText = sqlString;
            comm.Parameters.AddWithValue("@PersonV2_ID", intPersonV2_ID);

            conn.Open();

            return comm.ExecuteReader();

        }
        private string GetConnected()
        {
            return @"Server=sql.neit.edu\sqlstudentserver, 4500;Database=SE245_GOliva;User Id=SE245_GOliva;Password=005501789;";
        }

        public PersonV2() : base()
            {
                cellphone = "";
                instagram = "";

            }
        }
}
