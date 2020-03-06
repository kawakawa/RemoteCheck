using System;

namespace SystemCommon.Util
{
    public class Config
    {
        /// <summary>
        /// 指定された項目の設定値を取得
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Get(string target)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[target];
            }
            catch (Exception)
            {
                //ignron
            }

            return null;
        }
    }
}
