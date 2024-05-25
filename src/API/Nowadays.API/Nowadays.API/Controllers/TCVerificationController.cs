using MernisService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Common.ViewModels;

namespace Nowadays.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TCVerificationController : ControllerBase
    {

        [HttpPost]
        [Route("TC-Verification")]
        public async Task<string> Verification(long TcNo,string Name, string Surname,int Birthyear)
        {
            KPSPublicSoapClient client = new MernisService.KPSPublicSoapClient(MernisService.KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
            TCKimlikNoDogrulaResponse response = await client.TCKimlikNoDogrulaAsync(Convert.ToInt64(TcNo), Name, Surname, Birthyear);
            bool result=response.Body.TCKimlikNoDogrulaResult;

            if (result)
                return "TC Kimlik Doğrulama Başarılı";
            else
                return "TC Kimlik Doğrulama Başarısız"; 
        }
    }
}
