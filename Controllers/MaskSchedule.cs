using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalMask.Controllers
{
    public class MaskSchedule
    {
        private readonly MaskContext _maskContext;
        private readonly MaskService _maskService;

        public MaskSchedule(MaskContext maskContext, MaskService maskService)
        {
            _maskContext = maskContext;
            _maskService = maskService;
        }

        public async Task InitialDb()
        {
            if (_maskContext.MedicalMasks.FirstOrDefault() == null)
            {
                var maskInfos = await _maskService.GetMaskInfo();
                await _maskContext.AddRangeAsync(maskInfos);
                await _maskContext.SaveChangesAsync();
            }
        }

        public async Task MaskDataUpdate()
        {
            var updateTime = this._maskContext.MedicalMasks.First().UpdateTime;
            var nextTime = updateTime.AddSeconds(600);
            var nowTime = DateTime.Now;

            if (nextTime < nowTime)
            {
                var maskInfos = await this._maskService.GetMaskInfo();
                var sourceTime = DateTime.Parse(maskInfos.First().來源資料時間);

                if (sourceTime.CompareTo(updateTime) != 0)
                {
                    _maskContext.RemoveRange(this._maskContext.MedicalMasks);
                    await _maskContext.AddRangeAsync(maskInfos);
                }
            }

            await this._maskContext.SaveChangesAsync();
        }
    }
}