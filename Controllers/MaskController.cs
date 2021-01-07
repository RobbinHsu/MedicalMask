using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MaskService _maskService;

        public MaskController(MaskContext maskContext, MaskService maskService)
        {
            _maskContext = maskContext;
            _maskService = maskService;
        }

        public async Task<IActionResult> Get()
        {
            try
            {
                if (IsMedicalMasksNull())
                {
                    await InitialDb();
                }

                await MaskDataUpdate();

                return Ok(_maskContext.MedicalMasks.AsNoTracking());
            }
            catch (HttpRequestException ex)
            {
                return Problem(ex.Message);
            }
        }

        private bool IsMedicalMasksNull()
        {
            return _maskContext.MedicalMasks.FirstOrDefault() == null;
        }

        private async Task InitialDb()
        {
            var maskInfos = await _maskService.GetMaskInfo();
            foreach (var maskInfo in maskInfos)
            {
                _maskContext.Add(new MedicalMask()
                {
                    Code = maskInfo.醫事機構代碼,
                    Name = maskInfo.醫事機構名稱,
                    Address = maskInfo.醫事機構地址,
                    Tel = maskInfo.醫事機構電話,
                    AdultNum = int.Parse(maskInfo.成人口罩剩餘數),
                    KidNum = int.Parse(maskInfo.兒童口罩剩餘數),
                    UpdateTime = DateTime.Parse(maskInfo.來源資料時間),
                });
            }

            await _maskContext.SaveChangesAsync();
        }

        private async Task MaskDataUpdate()
        {
            var updateTime = _maskContext.MedicalMasks.First().UpdateTime;
            var nextTime = updateTime.AddSeconds(600);
            var nowTime = DateTime.Now;

            if (nextTime < nowTime)
            {
                var maskInfos = await _maskService.GetMaskInfo();
                var sourceTime = DateTime.Parse(maskInfos.First().來源資料時間);

                if (sourceTime.CompareTo(updateTime) != 0)
                {
                    _maskContext.RemoveRange(_maskContext.MedicalMasks);
                    foreach (var maskInfo in maskInfos)
                    {
                        _maskContext.Add(new MedicalMask()
                        {
                            Code = maskInfo.醫事機構代碼,
                            Name = maskInfo.醫事機構名稱,
                            Address = maskInfo.醫事機構地址,
                            Tel = maskInfo.醫事機構電話,
                            AdultNum = int.Parse(maskInfo.成人口罩剩餘數),
                            KidNum = int.Parse(maskInfo.兒童口罩剩餘數),
                            UpdateTime = DateTime.Parse(maskInfo.來源資料時間),
                        });
                    }
                }
            }

            await _maskContext.SaveChangesAsync();
        }
    }
}