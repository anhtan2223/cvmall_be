using RestSharp;

namespace Framework.Core.Helpers
{
    public static class PdfHelpers
    {
        public static readonly string AppDomain = Environment.GetEnvironmentVariable("ASPNETCORE_API_DOMAIN");
        private static readonly string ServiceConvertApi = Environment.GetEnvironmentVariable("ASPNETCORE_SERVICE_CONVERT");
        public static readonly string ExcelFolder = Environment.GetEnvironmentVariable("ASPNETCORE_CONVERT_DIR_EXCEL");
        public static readonly string PDFFolder = Environment.GetEnvironmentVariable("ASPNETCORE_CONVERT_DIR_PDF");

        public static string ConvertExelToPdf(string fileName)
        {
            // Call api convert
            Uri baseUrl = new Uri($"{ServiceConvertApi}/api/convert/excel2pdf?fileName={fileName}");
            var client = new RestClient(baseUrl);
            RestRequest? request = new RestRequest(baseUrl, Method.POST);

            var response = client.Post(request);
            if (response.Content == "OK")
            {
                return $"{AppDomain}/download/{fileName.Replace(".xlsx", ".pdf")}";
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
