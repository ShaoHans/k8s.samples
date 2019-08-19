using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace K8s.Samples.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string _secretPath = "";
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _secretPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secret");
            if(!Directory.Exists(_secretPath))
            {
                Directory.CreateDirectory(_secretPath);
            }

            _configuration = configuration;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("ok");
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string user, string pwd)
        {
            var userFile = Path.Combine(_secretPath, "user");
            if(!System.IO.File.Exists(userFile))
            {
                return Ok("不存在user文件");
            }

            var pwdFile = Path.Combine(_secretPath, "pwd");
            if (!System.IO.File.Exists(pwdFile))
            {
                return Ok("不存在pwd文件");
            }

            string secretUser = "";
            using (var fs = new StreamReader(userFile))
            {
                secretUser = await fs.ReadToEndAsync();
            }

            if(string.IsNullOrWhiteSpace(secretUser))
            {
                return Ok("user 文件中的内容为空");
            }
            //secretUser = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(secretUser));

            string secretPwd = "";
            using(var fs = new StreamReader(pwdFile))
            {
                secretPwd = await fs.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(secretPwd))
            {
                return Ok("pwd 文件中的内容为空");
            }
            //secretPwd = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(secretPwd));


            if (secretUser.Equals(user) && secretPwd.Equals(pwd))
            {
                return Ok("login successed");
            }
            else
            {
                return Ok("login fail");
            }
        }

        [HttpGet("settings")]
        public IActionResult GetSettings(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                return Ok("参数key的值不能为空");
            }

            var section = _configuration.GetSection(key);
            if(section.Exists())
            {
                return Ok(section.AsEnumerable());
            }
            else
            {
                return Ok("不存在的key");
            }
        }
    }
}
