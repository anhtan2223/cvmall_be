using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Framework.Core.Helpers.CallAPI
{
    public static class RestfulApi<T>
    {
        /// <summary>
        /// Method Get Async RestFull
        /// </summary>
        /// <param name="apicontroller">Đường dẫn api</param>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync(string apicontroller, object dataObject = null, string apiUrl = null)
        {
            try
            {
                return await RestSharp(apicontroller, dataObject, Method.GET, apiUrl);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Method Post Async RestFull
        /// </summary>
        /// <param name="apicontroller">Đường dẫn api</param>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync(string apicontroller, object dataObject = null, string apiUrl = null)
        {
            try
            {
                return await RestSharp(apicontroller, dataObject, Method.POST, apiUrl);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        /// <summary>
        /// Method Post Async RestFull
        /// </summary>
        /// <param name="apicontroller">Đường dẫn api</param>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static async Task<T> PostAsyncParameter(string apicontroller, object dataObject = null, string apiUrl = null)
        {
            try
            {
                return await RestSharpParameter(apicontroller, dataObject, Method.POST, apiUrl);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        /// <summary>
        /// Method Put Async RestFull
        /// </summary>
        /// <param name="apicontroller">Đường dẫn api</param>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static async Task<T> PutAsync(string apicontroller, object dataObject = null, string apiUrl = null)
        {
            return await RestSharp(apicontroller, dataObject, Method.PUT, apiUrl);
        }

        /// <summary>
        /// Method Delete Async RestFull
        /// </summary>
        /// <param name="apicontroller">Đường dẫn api</param>
        /// <param name="jsonbody"></param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync(string apicontroller, object dataObject = null, string apiUrl = null)
        {
            return await RestSharp(apicontroller, dataObject, Method.DELETE, apiUrl);
        }

        private static async Task<T> RestSharp(string apicontroller, object dataObject, Method method, string apiUrl = null)
        {
            T result = default;
            try
            {
                var client = new RestClient($"{apiUrl}{apicontroller}");
                //string accessToken = SessionControl.GetUserClaims()?.access_token;
                //if (string.IsNullOrEmpty(accessToken))
                //{
                //    return result;
                //}
                //header
                var request = new RestRequest(method);
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", String.Format("Bearer {0}", accessToken));
                //body
                if (dataObject != null)
                {
                    string jsonWithLocalTimeZone = JsonConvert.SerializeObject(dataObject, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    });
                    request.AddJsonBody(jsonWithLocalTimeZone);
                }
                //execute
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<T>(response.Content);
                }
                //else if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    FormsAuthentication.SignOut();
                //}
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RestfulApi error: " + ex.Message);
                return result;
            }
        }

        private static async Task<T> RestSharpParameter(string apicontroller, object dataObject, Method method, string apiUrl = null)
        {
            T result = default;
            try
            {
                var client = new RestClient($"{apiUrl}{apicontroller}");
                //string accessToken = SessionControl.GetUserClaims()?.access_token;
                //if (string.IsNullOrEmpty(accessToken))
                //{
                //    return result;
                //}
                //header
                var request = new RestRequest(method);
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", String.Format("Bearer {0}", accessToken));
                //body
                if (dataObject != null)
                {
                    string jsonWithLocalTimeZone = JsonConvert.SerializeObject(dataObject, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    });
                    request.AddParameter("application/json", jsonWithLocalTimeZone, ParameterType.RequestBody);
                }
                //execute
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<T>(response.Content);
                }
                //else if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    FormsAuthentication.SignOut();
                //}
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RestfulApi error: " + ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Call API not create ticket
        /// </summary>
        /// <param name="apicontroller"></param>
        /// <param name="dataObject"></param>
        /// <param name="method"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<T> RestSharp(string apicontroller, object dataObject, Method method, string accessToken, string apiUrl = null)
        {
            T result = default;
            try
            {
                var client = new RestClient($"{apiUrl}{apicontroller}");

                if (string.IsNullOrEmpty(accessToken))
                {
                    return result;
                }
                //header
                var request = new RestRequest(method);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", string.Format("Bearer {0}", accessToken));
                //body
                if (dataObject != null)
                {
                    string jsonWithLocalTimeZone = JsonConvert.SerializeObject(dataObject, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                    request.AddJsonBody(jsonWithLocalTimeZone);
                }
                //execute
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<T>(response.Content);
                }
                //else if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    FormsAuthentication.SignOut();
                //}
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RestfulApi error: " + ex.Message);
                return result;
            }
        }
    }
}