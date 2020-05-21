using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
//using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Microsoft.Identity.Client;
using ctwintest.IntegrationTests.Models;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ctwintest.IntegrationTests
{
    [TestClass]
    public class Provision
    {
        private HttpClient _httpClient;
        private int _expectedCountSpaces=5;
        private int _expectedCountDevices=1;
        private int _expectedCountSensors=2;
        private AppSettings _appSettings;
        private string _spaceId="";
        string spaceName="Control Panel A";
            
        [TestMethod]
        public async Task TestSpaces()
        {
            AppSettings settings= new AppSettings();
            settings.Load();
            _appSettings = settings;
            _httpClient= await SetupHttpClient(_appSettings);
            List<Models.Space> spaces = await GetSpacesAync(_httpClient);
            Assert.AreEqual(_expectedCountSpaces , spaces.Count );
           
        }

        /*private static string GetDisplayValues(Models.Space space)
        {
            var spaceValue = space.Values.First(v => v.Type == "AvailableAndFresh");
            return $"Name: {space.Name}\nId: {space.Id}\nTimestamp: {spaceValue.Timestamp}\nValue: {spaceValue.Value}\n";
        }*/

        [TestMethod]
        public async Task TestDevices()
        {
            AppSettings settings= new AppSettings();
            settings.Load();
            _appSettings = settings;
            _httpClient= await SetupHttpClient(_appSettings);
            string spaceId= await GetSpaceIdAsync(_httpClient,spaceName );
            List<Models.Device> devices = await GetDevicesAyncBySpaceId(_httpClient, spaceId);
            Assert.AreEqual(_expectedCountDevices , devices.Count );

        }

        [TestMethod]
        public async Task TestSensors()
        {
            AppSettings settings= new AppSettings();
            settings.Load();
            _appSettings = settings;
            _httpClient= await SetupHttpClient(_appSettings);
            string spaceId= await GetSpaceIdAsync(_httpClient,spaceName );
            List<Models.Sensor> sensors = await GetSensorsAyncBySpaceId(_httpClient, _spaceId);
            Assert.AreEqual(_expectedCountSensors , sensors.Count );
        }

        //[TearDown]
        public void TestTearDown()
        {

        }

        #region Private Methods

        //SetupHttpClient using AppSettings
        private static async Task<HttpClient> SetupHttpClient( AppSettings appSettings)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(appSettings.BaseUrl),
            };

            var accessToken = await Authentication.GetToken(appSettings);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            return httpClient;
        }
        //Get Spaces Method
        //Returns List of Devices
        public static async Task<List<Models.Space>> GetSpacesAync(
                HttpClient httpClient,
                string includes = null)
        {
            // if (id == Guid.Empty)
            //   throw new ArgumentException("GetSpace requires a non empty guid as id");

            var response = await httpClient.GetAsync($"spaces/" + (includes != null ? $"?includes={includes}" : ""));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var spaces = JsonConvert.DeserializeObject<List<Models.Space>>(content);
                // logger.LogInformation($"Retrieved Space: {JsonConvert.SerializeObject(space, Formatting.Indented)}");
                return spaces;
            }

            return null;
        }

        //Get Spaces Method
        //Returns List of Devices
        public static async Task<String> GetSpaceIdAsync(
                HttpClient httpClient, string SpaceName,
                string includes = null)
        {
            // if (id == Guid.Empty)
            //   throw new ArgumentException("GetSpace requires a non empty guid as id");

            var response = await httpClient.GetAsync($"spaces/" + (includes != null ? $"?includes={includes}" : ""));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var spaces = JsonConvert.DeserializeObject<List<Models.Space>>(content);
                var spaceContainingDevice =  spaces.Where(s => s.Name==SpaceName);
                Models.Space spaceFirst=new Models.Space();
                if (spaceContainingDevice.Any())
                {
                    spaceFirst = spaceContainingDevice.First(); 
                 }
                return spaceFirst.Id; 
                // logger.LogInformation($"Retrieved Space: {JsonConvert.SerializeObject(space, Formatting.Indented)}");
                //return _spaceId;
            }

            return null;
        }

        //Retuns List of Devices by specifying SpaceId        
        public static async Task<List<Models.Device>> GetDevicesAyncBySpaceId(
                HttpClient httpClient,
                string spaceId)
        {
            // if (id == Guid.Empty)
            //   throw new ArgumentException("GetSpace requires a non empty guid as id");

            var response = await httpClient.GetAsync($"devices/" +  $"?spaceId={spaceId}" );
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var devices = JsonConvert.DeserializeObject<List<Models.Device>>(content);
                // logger.LogInformation($"Retrieved Space: {JsonConvert.SerializeObject(space, Formatting.Indented)}");
                return devices;
            }

            return null;
        }

        //Retuns list of Sensors by specifying SpaceId
        public static async Task<List<Models.Sensor>> GetSensorsAyncBySpaceId(
                HttpClient httpClient,
                string spaceId)
        {
            // if (id == Guid.Empty)
            //   throw new ArgumentException("GetSpace requires a non empty guid as id");

            var response = await httpClient.GetAsync($"sensors/" + $"?spaceId={spaceId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sensors = JsonConvert.DeserializeObject<List<Models.Sensor>>(content);
                // logger.LogInformation($"Retrieved Space: {JsonConvert.SerializeObject(space, Formatting.Indented)}");
                return sensors;
            }

            return null;
        }


        #endregion
    }
}
