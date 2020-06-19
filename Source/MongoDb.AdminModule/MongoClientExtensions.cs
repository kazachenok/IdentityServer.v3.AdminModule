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
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace IdentityServer3.Admin.MongoDb.Powershell
{
    internal static class MongoClientExtensions
    {
        public static async Task<bool> DatabaseExistsAsync(this IMongoClient client, string name)
        {
            var cursor = await client.ListDatabasesAsync();
            var databases = await cursor.ToListAsync();
            return databases.Any(x => string.Equals(x["name"].AsString, name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public static class MongoDatabaseExtensions
    {
        internal static async Task<bool> CollectionExistsAsync(this IMongoDatabase db, string name)
        {
            var cursor = await db.ListCollectionsAsync();
            var collections = await cursor.ToListAsync();
            return collections.Any(x => string.Equals(x["name"].AsString, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}