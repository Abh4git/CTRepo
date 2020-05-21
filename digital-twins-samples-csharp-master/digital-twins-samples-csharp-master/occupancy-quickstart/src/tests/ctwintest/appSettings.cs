// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
//using Microsoft.Extensions.Configuration;

namespace ctwintest.IntegrationTests
{
    public class AppSettings {
        // Note: this is a constant because it is the same for every user authorizing
        // against the Digital Twins Apis
        private static string DigitalTwinsAppId = "0b07f429-9f4b-4714-9392-cc5e8e80c8b0";
        private static string[] AadScopes = new string[] { DigitalTwinsAppId + "/Read.Write" };

        public string AADInstance { get; set; }
        public string AadRedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; } = DigitalTwinsAppId;
        public string Tenant { get; set; }
        public string BaseUrl { get; set; }
        public string Authority => AADInstance + Tenant;
        public string[] Scopes { get; set; } = AadScopes;

        public AppSettings()
        {
            /*this.BaseUrl="https://controltwin.australiaeast.azuresmartspaces.net/management/api/v1.0/"; 
            this.AADInstance ="https://login.microsoftonline.com/";
            this.AadRedirectUri="http://localhost:8080/";
            this.ClientId="e5327385-27c4-4dcd-9870-244978f37b64";
            this.Tenant= "00c4aad1-3e0f-4f8e-a3ed-71d5bc01849e";*/

        }

        public void Load()
        {
            //var appSettings = new ConfigurationBuilder()

            this.BaseUrl="https://controltwin.australiaeast.azuresmartspaces.net/management/api/v1.0/"; 
            this.AADInstance ="https://login.microsoftonline.com/";
            this.AadRedirectUri="http://localhost:8080/";
            this.ClientId="e5327385-27c4-4dcd-9870-244978f37b64";
            this.Tenant= "00c4aad1-3e0f-4f8e-a3ed-71d5bc01849e";

            // Sanitize input

            // This is because httpClient will behave differently if the
            // passed in Uri has a trailing slash when using GetAsync.
            this.BaseUrl = EnsureTrailingSlash(this.BaseUrl);

            //return AppSettings;
        }

        private static string EnsureTrailingSlash(string baseUrl)
            => baseUrl.Length == 0 || baseUrl[baseUrl.Length-1] == '/'
                ? baseUrl
                : baseUrl + '/';
    }
}