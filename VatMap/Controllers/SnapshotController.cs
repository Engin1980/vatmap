using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VatMap.Model.Front;

namespace VatMap.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SnapshotController : ControllerBase
  {
    [HttpGet]
    public async Task<IActionResult> GetSnapshotAsync()
    {
      Snapshot s = new Snapshot()
      {
        Atcs = null,
        Date = DateTime.Now,
        Planes = null
      };
      return this.Ok(s);
    }
  }
}