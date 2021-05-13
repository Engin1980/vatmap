using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VatMap.Model.Back.Vatsim;
using VatMap.Model.Front;
using VatMap.Services;

namespace VatMap.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SnapshotController : ControllerBase
  {

    private readonly VatsimProviderService vatsimProvider;

    public SnapshotController([FromServices] VatsimProviderService vatsimProvider)
    {
      this.vatsimProvider = vatsimProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetSnapshotAsync()
    {
      Snapshot ret = this.vatsimProvider.Obtain();
      return this.Ok(ret);
    }
  }
}