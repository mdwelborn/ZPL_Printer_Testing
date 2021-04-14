﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZPL_Print_Testing.Models;
using ZPL_Print_Testing.Repositories;

namespace ZPL_Print_Testing.Services
{
    public class ConfigService : IConfigService
    {
        private IConfigRepository _configRepo;

        public ConfigService()
        {
            _configRepo = new ConfigRepository();
        }

        public AppConfig GetAppConfig()
        {
            return _configRepo.GetAppConfig();
        }

        public void SaveOrUpdateAppConfig(AppConfig appConfig)
        {
            _configRepo.SaveOrUpdateAppConfig(appConfig);
        }
    }
}
