using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Z_Scrimmage
{
    public class DbManager
    {
        public static MySqlConnection mySql;

        public static bool Connect(string db, string ip, int port, string user, string pw)
        {
            //创建对象
            mySql = new MySqlConnection();
            //连接参数
            string s = string.Format("Database={0};Data Source={1};port={2};User Id={3};Password={4}", db, ip, port, user, pw);
            mySql.ConnectionString = s;
            try//连接
            {
                mySql.Open();
                Console.WriteLine(DateTime.Now + " 数据库连接成功 ");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库连接失败 " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 是否存在该用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool IsAccountExist(string id)
        {
            if (!IsSafeString(id)) return false;
            string s = string.Format("select * from account where id = '{0}';", id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, mySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return !hasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库 不是安全字符 " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static bool Register(string id, string pw)
        {
            if (!IsSafeString(id))
            {
                Console.WriteLine("数据库注册失败，id不是安全字符");
                return false;
            }
            if (!IsSafeString(pw))
            {
                Console.WriteLine("数据库注册失败，pw不是安全字符");
                return false;
            }
            if (!IsAccountExist(id))
            {
                Console.WriteLine("数据库注册失败，注册id已存在");
                return false;
            }
            string sql = string.Format("insert into account set id ='{0}',pw ='{1}';", id, pw);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库注册失败 " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CreatePlayer(string id,string name)
        {
            if (!IsSafeString(id))
            {
                Console.WriteLine("数据库创建角色失败，id不是安全字符");
                return false;
            }
            PlayerData playerData = new PlayerData();
            //写入数据库
            string sql =
                string.Format("insert into player set id='{0}',userName='{1}',coin='{2}',winNum='{3}',loseNum='{4}';"
                , id, name, playerData.coin, playerData.winNum, playerData.loseNum);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库创建角色失败 " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 账号查验
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static bool CheckPassword(string id, string pw)
        {
            if (!IsSafeString(id))
            {
                Console.WriteLine("数据库账号查验失败，id不是安全字符");
                return false;
            }
            if (!IsSafeString(pw))
            {
                Console.WriteLine("数据库账号查验失败，pw不是安全字符");
                return false;
            }
            string sql = string.Format("select * from account where id='{0}' and pw='{1}';", id, pw);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return hasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库账号查验失败 " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 获得玩家数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PlayerData GetPlayerData(string id)
        {
            if (!IsSafeString(id))
            {
                Console.WriteLine("数据库获得玩家数据失败，id不是安全字符");
                return null;
            }
            string sql = string.Format("select * from player where id='{0}';", id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    return null;
                }
                dataReader.Read();
                PlayerData playerData = new PlayerData()
                {
                    userName = dataReader.GetString("userName"),
                    coin = dataReader.GetInt32("coin"),
                    winNum = dataReader.GetInt32("winNum"),
                    loseNum = dataReader.GetInt32("loseNum")
                };
                dataReader.Close();
                return playerData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库获得玩家数据失败 " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 更新玩家数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerData"></param>
        /// <returns></returns>
        public static bool UpdatePlayerData(string id, PlayerData playerData)
        {
            string sql =
                string.Format("update player set userName='{0}',coin='{1}',winNum='{2}',loseNum='{3}' where id='{4}';"
                , playerData.userName, playerData.coin, playerData.winNum, playerData.loseNum, id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库更新角色数据失败 " + ex.Message);
                return false;
            }
        }
        //判定安全字符
        private static bool IsSafeString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
    }
}
