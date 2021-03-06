/*
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

using System;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Text;
using IdentityServer.MongoDb.AdminModule;
using IdentityServer3.Core.Models;

namespace IdentityServer3.Admin.MongoDb.Powershell
{
    [Cmdlet(VerbsCommon.New, "ClientSecret")]
    public class CreateClientSecret : PSCmdlet
    {
        [Parameter, ValidateNotNullOrEmpty]
        public string Value { get; set; }
        [Parameter]
        public string Description { get; set; }
        [Parameter]
        public DateTimeOffset? Expiration { get; set; }
        [Parameter]
        public HashType? Hash { get; set; }
        protected override void ProcessRecord()
        {
            if (Hash != null)
            {
                var hash = Hash == HashType.SHA256 ? (HashAlgorithm) SHA256.Create() : SHA512.Create();
                var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(Value));
                Value = Convert.ToBase64String(bytes);
            }
            WriteObject(new Secret(Value, Description, Expiration));
        }
    }
}