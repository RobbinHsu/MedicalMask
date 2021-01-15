using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedicalMask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MaskController : ControllerBase
    {
        private readonly MaskContext _maskContext;

        public MaskController(MaskContext maskContext)
        {
            _maskContext = maskContext;
        }

        public IActionResult Get()
        {
            try
            {
                return Ok(_maskContext.MedicalMasks.AsNoTracking());
            }
            catch (HttpRequestException ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}