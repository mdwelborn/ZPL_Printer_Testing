﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using ZPL_Print_Testing.Models;

namespace ZPL_Print_Testing.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        //<summary>
        // Returns all app config data 
        //</summary>
        public AppConfig GetAppConfig()
        {
            using (var conn = new SqliteConnection("Data Source=" +System.Environment.CurrentDirectory + "/zpl.db"))
            {
                var sql = "SELECT * FROM AppConfig " +
                          "left join LabelFormats on AppConfig.id = LabelFormats.AppConfigId;";

                var appConfig = conn.Query<AppConfig, LabelFormat, AppConfig>(sql, (appConfig, labelFormat) =>
                {
                    if (labelFormat != null)
                    {
                        labelFormat.AppConfigId = appConfig.Id;
                        appConfig.LabelFormats.Add(labelFormat);
                    }

                    return appConfig;
                });

                return appConfig.FirstOrDefault();
            }

        }

        //<summary>
        // Save or update app config data.  
        //</summary>
        public void SaveAppConfig(AppConfig appConfig)
        {
            using (var conn = new SqliteConnection("Data Source=" + System.Environment.CurrentDirectory + "/zpl.db"))
            {
                var sql =
                    "UPDATE AppConfig SET IpAddress = @IpAddress, Port = @Port, SaveLabels = @SaveLabels, SaveLabelPath = @SaveLabelPath WHERE Id = @Id ";

                var result = conn.Execute(sql, new
                {
                    appConfig.IpAddress,
                    appConfig.Port,
                    SaveLabels = appConfig.SaveLabels ? 1 : 0,
                    appConfig.SaveLabelPath,
                    appConfig.Id
                });

            }
        }

        public void SaveLabelFormat(LabelFormat labelFormat)
        {
            using (var conn = new SqliteConnection("Data Source=" + System.Environment.CurrentDirectory + "/zpl.db"))
            {
                if (labelFormat.Id > 0)
                {
                    var sql =
                        "UPDATE LabelFormats SET Name = @Name, Height = @Height, Width = @Width, PrintDensity = @PrintDensity, IsDefault = @IsDefault WHERE Id = @Id ";

                    var result = conn.Execute(sql, new
                    {
                        labelFormat.Name,
                        labelFormat.Height,
                        labelFormat.Width,
                        labelFormat.PrintDensity,
                        labelFormat.IsDefault,
                        labelFormat.Id
                    });
                }
                else
                {
                    var sql =
                        "insert into LabelFormats (AppConfigId, Name, Height, Width, PrintDensity, IsDefault) VALUES" +
                        "(@AppConfigId, @Name, @Height, @Width, @PrintDensity, @IsDefault)";
                    
                    var result = conn.Execute(sql, new
                    {
                        labelFormat.AppConfigId,
                        labelFormat.Name,
                        labelFormat.Height,
                        labelFormat.Width,
                        labelFormat.PrintDensity,
                        labelFormat.IsDefault
                    });
                }
            }
        }
    }
}
