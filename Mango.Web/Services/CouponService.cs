using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService ?? throw new ArgumentNullException(nameof(baseService));
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = couponDto,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI"
            });

            return result;
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.DELETE,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI/{id}"
            });

            return result;
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI"
            });
            return result;
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI/GetByCode/{couponCode}"
            });
            return result;
        }

        public async Task<ResponseDto?> GetCouponsByIdAsync(int id)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI/{id}"
            });
            return result;
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.PUT,
                Data = couponDto,
                Url = $"{Utility.SD.CouponApiBase}/api/couponAPI"
            });
            return result;
        }
    }
}
