using ContentCenter.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ContentCenter.Test
{
    public class ServiceStart
    {
        //IOptions<Advenced> _options;
        Advenced _advenced;
        public ServiceStart()
        {
            _advenced = GetSettings();

            //_options = Options.Create<Advenced>(new Advenced
            //{
            //    Scheme = _advenced.Scheme,
            //    Host = _advenced.Host,
            //    Port = _advenced.Port,                
            //});

        }
        [Fact(DisplayName = "Проверка настроек для запуск сервиса - Scheme")]
        public void ChekScheme()
        {
            bool result = false;

            if (_advenced.Scheme.ToLower() == "http"
                || _advenced.Scheme.ToLower() == "https")
                result = true;
            else
                result = false;

            Assert.True(result);
        }
        [Fact(DisplayName = "Проверка настроек для запуск сервиса - Host")]
        public void ChekHost()
        {
            bool result = false;
            
            if (_advenced.Host.ToLower() == "localhost")
                result = true;
            else
            {
                Match match = Regex.Match(_advenced.Host, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                    result = true;
                else
                    result = false;
            }            

            Assert.True(result);
        }

        [Fact(DisplayName = "Проверка настроек для запуск сервиса - Port")]
        public void ChekPort()
        {
            bool result = false;

            int _port = -1;
            int.TryParse(_advenced.Port, out _port);

            if (_port < 0 && _port > 65535)
                result = false;
            else
                result = true;

            Assert.True(result);
        }

        #region private methods
        private Advenced GetSettings()
        {
            string path = @"D:\new\ContentCenter\ContentCenter\ContentCenter\appsettings.json";
            Settings settings = JsonConvert.DeserializeObject<Settings>(
                File.ReadAllText(path));
            return settings.Logging.Advenced;
        }
        #endregion
    }
}
