namespace GoodWareMixWebClient.Services
{
    public class HostConnection
    {
        public static HttpClientHandler http
        {
            get
            {
                return new HttpClientHandler();
            }
            set
            {
                http.ClientCertificateOptions = ClientCertificateOption.Manual;
                http.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }; ;
            }
        }
     

    }
}
