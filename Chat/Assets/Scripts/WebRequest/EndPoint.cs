using System;
using System.Text;

public class EndPoint
{
    public string serverAddress;
    public string baseUrl;
    public string login = "";
    public string password = "";

    public string endPoint
    {
        get => serverAddress + baseUrl;
    }

    public string credential
    {
        get
        {
            string credential = $"{login}:{password}";
            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
            byte[] credentialBytes = encoding.GetBytes(credential);
            string credentialBase64 = Convert.ToBase64String(credentialBytes);
            return $"Basic {credentialBase64}";
        }
    }
}