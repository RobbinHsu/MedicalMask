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
                    this._maskContext.RemoveRange(this._maskContext.MedicalMasks);
                    foreach (var maskInfo in maskInfos)
                    {
                        this._maskContext.Add(new MedicalMask()
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

            await this._maskContext.SaveChangesAsync();
        }
    }
}