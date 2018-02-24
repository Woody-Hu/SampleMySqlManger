using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlManger
{
    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class BaseMySqlManger
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectString = string.Empty;

        /// <summary>
        /// 数据库连接器
        /// </summary>
        private MySql.Data.MySqlClient.MySqlConnection thisConnection = null;

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public System.Data.ConnectionState State
        {
            get
            {
                if (null != thisConnection)
                {
                    return thisConnection.State;
                }
                else
                {
                    return System.Data.ConnectionState.Closed;
                }
            }
        }

        /// <summary>
        /// 构件数据库管理器
        /// </summary>
        /// <param name="inputconnectString"></param>
        public BaseMySqlManger(string inputconnectString)
        {
            connectString = inputconnectString;
            thisConnection = new MySql.Data.MySqlClient.MySqlConnection(connectString);
        }

        /// <summary>
        /// 尝试打开数据库连接
        /// </summary>
        /// <returns></returns>
        public bool OpenConnect()
        {
            if (null == thisConnection)
            {
                return false;
            }

            try
            {
                thisConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试关闭数据库连接
        /// </summary>
        /// <returns></returns>
        public bool CloseConnect()
        {
            if (null == thisConnection || thisConnection.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
            else
            {
                try
                {
                    thisConnection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行一个查询并将结果填到DataTable内
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="findValue"></param>
        /// <returns></returns>
        public bool TryQueryToDataTable(string sqlText, out DataTable findValue)
        {
            try
            {
                findValue = new DataTable();
                using (MySql.Data.MySqlClient.MySqlCommand tempCommand =
                    new MySql.Data.MySqlClient.MySqlCommand(sqlText, thisConnection))
                {
                    var tempReader = tempCommand.ExecuteReader();
                    findValue.Load(tempReader);
                    tempReader.Close();
                }
                return true;
            }
            catch (Exception)
            {
                findValue = null;
                return false;
            }

        }

        /// <summary>
        /// 尝试在非事务模式执行数据库操作(非查询）
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public bool TryExecuteNoneQueryNontransaction(string sqlText)
        {
            try
            {
                int count;
                using (MySql.Data.MySqlClient.MySqlCommand tempCommand =
                   new MySql.Data.MySqlClient.MySqlCommand(sqlText, thisConnection))
                {
                    count = tempCommand.ExecuteNonQuery();
                }
                return 0 != count;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
