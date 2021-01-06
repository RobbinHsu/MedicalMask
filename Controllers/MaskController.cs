using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalMask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MaskController : ControllerBase
    {
        private readonly MaskService _maskService;

        public MaskController(MaskService maskService)
        {
            _maskService = maskService;
        }

        public async Task<IActionResult> Get()
        {
            try
            {
                var maskCount = await _maskService.GetMaskInfo();
                return Ok(maskCount);
            }
            catch (HttpRequestException ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}