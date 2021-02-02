using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalMask.Controllers
{
    public static class Helpler
    {
        public static IEnumerable<MedicalMask> ConvertToMedicalMasks(this IEnumerable<MaskInfo> maskInfo)
        {
            return maskInfo.Select(info => new MedicalMask()
                {
                    Code = info.醫事機構代碼,
                    Name = info.醫事機構名稱,
                    Address = info.醫事機構地址,
                    Tel = info.醫事機構電話,
                    AdultNum = int.Parse(info.成人口罩剩餘數),
                    KidNum = int.Parse(info.兒童口罩剩餘數),
                    UpdateTime = DateTime.Parse(info.來源資料時間)
                })
                .ToList();
        }
    }
}