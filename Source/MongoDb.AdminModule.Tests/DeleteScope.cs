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
using IdentityServer.Admin.MongoDb;
using IdentityServer.Core.MongoDb;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Xunit;

namespace MongoDb.AdminModule.Tests
{
    public class DeleteScope : IClassFixture<PowershellAdminModuleFixture>
    {
        private IScopeStore _scopeStore;
        private PowerShell _ps;
        private const string ScopeName = "removethisscope";

        [Fact]
        public void ScopeIsRemoved()
        {
            Assert.NotEmpty(_scopeStore.FindScopesAsync(new[] { ScopeName }).Result);
            _ps.Invoke();
            Assert.Empty(_scopeStore.FindScopesAsync(new[] { ScopeName }).Result);
        }

        public DeleteScope(PowershellAdminModuleFixture data)
        {
            var admin = data.Factory.Resolve<IAdminService>();
            Scope scope = TestData.ScopeMandatoryProperties();
            scope.Name = ScopeName;
            admin.Save(scope);
            _ps = data.PowerShell;
            _ps.AddScript(data.LoadScript(this))
                .AddParameter("Database", data.Database)
                .AddParameter("Name", ScopeName);
            _scopeStore = data.Factory.Resolve<IScopeStore>();
        }
    }
}
