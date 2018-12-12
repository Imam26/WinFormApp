using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form1 = new Form1();
            form1.ShowDialog();
            string serverName = form1.TextBox.Text;

            SampleDAL sampleDAL = new SampleDAL();

            string cnStrsFirstSectionName = "Master";
            string cnStrsSecondSectionName = "Sample";
            string dbName = "Sample";

            List<object[]> insertedDataSellers = new List<object[]>
            {
                new object[3]{1, "Игорь", "Мамаев"},
                new object[3]{2, "Валерий", "Ходорков"},
                new object[3]{3, "Евгений", "Викторов"},
                new object[3]{4, "Борис", "Сидоров"},
                new object[3]{5, "Иван", "Иванов"},
                new object[3]{6, "Максим", "Кокорин"},
                new object[3]{7, "Марат", "Балаев"},
                new object[3]{8, "Ислам", "Махачев"},
            };

            List<object[]> insertedDataCostumers = new List<object[]>
            {
                new object[3]{1, "Вадим", "Токарев"},
                new object[3]{2, "Брюс", "Хлебников"},
                new object[3]{3, "Максим", "Галкин"},
                new object[3]{4, "Чак", "Норрис"},
                new object[3]{5, "Михей", "Михалков"},
                new object[3]{6, "Гарик", "Мартиросян"},
                new object[3]{7, "Нурлан", "Сабуров"},
                new object[3]{8, "Диас", "Маженов"},
            };

            List<object[]> insertedDataSales = new List<object[]>
            {
                new object[4]{1,1,1000, new DateTime(2016,12,12)},
                new object[4]{1,2,3000, new DateTime(2018,11,10)},
                new object[4]{1,3,2000, new DateTime(2013,09,25)},
                new object[4]{2,4,5000, new DateTime(2011,07,26)},
                new object[4]{2,5,9000, new DateTime(2010,05,12)},
                new object[4]{2,6,11000, new DateTime(2009,03,14)},
                new object[4]{3,7,12000, new DateTime(2005,01,16)},
                new object[4]{3,8,134000, new DateTime(2012,02,17)},
            };

            DbProviderFactory factory = DbProviderFactories.
                        GetFactory(ConfigurationManager.
                        ConnectionStrings[cnStrsFirstSectionName].
                        ProviderName);

            DbConnection cnStr = factory.CreateConnection();

            DbConnectionStringBuilder firstConnectionStr = factory.CreateConnectionStringBuilder();
            firstConnectionStr.ConnectionString = ConfigurationManager.ConnectionStrings[cnStrsFirstSectionName].ConnectionString;
            firstConnectionStr.Add("Server", serverName);

            DbConnectionStringBuilder secondConnectionStr = factory.CreateConnectionStringBuilder();
            secondConnectionStr.ConnectionString = ConfigurationManager.ConnectionStrings[cnStrsSecondSectionName].ConnectionString;
            secondConnectionStr.Add("Data Source", serverName);

            try
            {
                cnStr.ConnectionString = firstConnectionStr.ConnectionString;    
                CreateDb(cnStr, dbName);

                cnStr.ConnectionString = secondConnectionStr.ConnectionString;
                CreateTablesToSampleDb(cnStr);

                sampleDAL.OpenConnection(factory, secondConnectionStr.ConnectionString);
                sampleDAL.InsertToSellersAndCostumers(insertedDataSellers, "Sellers");
                sampleDAL.InsertToSellersAndCostumers(insertedDataCostumers, "Costumers");
                sampleDAL.InsertToSales(insertedDataSales);

                MessageBox.Show("База данных успешно создана!!! Данные добавлены");

            }
            catch (DbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cnStr.ConnectionString = secondConnectionStr.ConnectionString;
                Application.Run(new Form2(cnStr));
                sampleDAL.CloseConnection();
            }
        }

        static void CreateDb(DbConnection cnStr, string dbName)
        {
            try
            {
                cnStr.Open();
                using (DbCommand cmd = cnStr.CreateCommand())
                {
                    cmd.CommandText = $"Create database {dbName}";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                cnStr.Close();
            }
        }

        static void CreateTablesToSampleDb(DbConnection cnStr)
        {
            string createCostumers = "Create table Costumers(id int not null primary key,"
                + "name nvarchar(30) not null, surName nvarchar(30) not null)";

            string createSellers = "Create table Sellers(id int not null primary key,"
                + "name nvarchar(30) not null, surName nvarchar(30) not null)";

            string createSales = "Create table Sales(id int not null primary key identity(1,1),"
                + "costId int not null foreign key references Costumers(id), selId int not null foreign key "
                + "references Sellers(id), sum int not null, salesDate date not null)";
            try
            {
                cnStr.Open();
                using (DbCommand cmd = cnStr.CreateCommand())
                {
                    cmd.CommandText = $"{createCostumers}\n{createSellers}\n{createSales}";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                cnStr.Close();
            }
        }
    }
}
