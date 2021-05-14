using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VatMap.Model.Back.Vatsim.JsonModel;
using VatMap.Model.Front;
using VatMap.Services;

namespace VatMap.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SnapshotController : ControllerBase
  {

    private readonly VatsimProviderService vatsimProvider;
    private readonly SnapshotProviderService snapshotProviderService;

    public SnapshotController([FromServices] VatsimProviderService vatsimProvider,
      [FromServices] SnapshotProviderService snapshotProviderService
      )
    {
      this.vatsimProvider = vatsimProvider;
      this.snapshotProviderService = snapshotProviderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSnapshotAsync()
    {
      Root json = this.vatsimProvider.Obtain();
      this.snapshotProviderService.UpdateByVatsim(json);
      Snapshot ret = this.snapshotProviderService.GetCurrent();
      return this.Ok(ret);
    }
  }
}