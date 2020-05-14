using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TunZhou.Core.Redis
{
    public class RedisConfig
    {
        private IConfiguration _configuration;
        public RedisConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            //ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();//创建ConfigurationBuilder对象 
            //configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true);
            //_configuration = configurationBuilder.Build();
            RedisIP = _configuration["Redis:RedisIP"];
            RedisPort = int.Parse(_configuration["Redis:RedisPort"]);
            Password = _configuration["Redis:Password"];
            ExpiredMinute = int.Parse(_configuration["Redis:ExpiredMinute"]);
            DBIndex = int.Parse(_configuration["Redis:DBIndex"]);
        }

        /// <summary>
        /// ip
        /// </summary>
        public string RedisIP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int RedisPort { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int ExpiredMinute { get; set; }

        /// <summary>
        /// redis索引
        /// </summary>
        public int DBIndex { get; set; }

    }
}
