using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace App1.Services
{
    public static class ReswSource
    {
        /// <summary>
        /// 获取resw资源文件中的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
            return resourceLoader.GetString(key).ToString();
        }
    }
}
