// Copyright 2019 Google LLC

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     https://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Net;

namespace leaderboardapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .ConfigureKestrel((context, options) =>
            {
                options.Listen(IPAddress.Any, GetPort(), listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            });

        private static int GetPort()
        {
            int port = 8080;

            var portStr = Environment.GetEnvironmentVariable("PORT");
            if(!string.IsNullOrEmpty(portStr))
            {
                if(!int.TryParse(portStr, out port))
                {
                    port = 8080;
                }
            }

            return port;
        }
    }
}