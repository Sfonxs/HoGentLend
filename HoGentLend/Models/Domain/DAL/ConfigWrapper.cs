using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.DAL;

namespace HoGentLend.Models.Domain.DAL
{
    public class ConfigWrapper : IConfigWrapper
    {
        private DbSet<Config> configs;
        private HoGentLendContext ctx;

        public ConfigWrapper(HoGentLendContext ctx)
        {
            this.ctx = ctx;
            this.configs = ctx.Configs;
        }

        public Config GetConfig()
        {
            return configs.First();
        }
    }
}