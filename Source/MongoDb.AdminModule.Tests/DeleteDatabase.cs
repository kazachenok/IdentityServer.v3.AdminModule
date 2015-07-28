﻿/*
 * Copyright 2014, 2015 James Geall
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Management.Automation;
using System.Threading.Tasks;
using IdentityServer.Admin.MongoDb;
using IdentityServer.Core.MongoDb;
using IdentityServer.MongoDb.AdminModule;
using MongoDB.Driver;
using Xunit;

namespace MongoDb.AdminModule.Tests
{
    public class DeleteDatabase : IClassFixture<PowershellAdminModuleFixture>
    {
        private string _database;
        private IMongoClient _server;
        private PowerShell _ps;

        [Fact]
        public async Task DatabaseShouldBeDeleted()
        {
            Assert.True(await _server.DatabaseExistsAsync(_database));
            _ps.Invoke();
            Assert.False(await _server.DatabaseExistsAsync(_database));
        }

        public DeleteDatabase(PowershellAdminModuleFixture data)
        {
            var admin = data.Factory.Resolve<IAdminService>();
            admin.CreateDatabase();
            _ps = data.PowerShell;
            _database = data.Database;
            _server = data.Client; 
            var script = data.LoadScript(this);
            _ps.AddScript(script).AddParameter("Database", _database);
           
        }
    }
}
