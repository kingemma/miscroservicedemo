using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            this._couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
        }
        // GET: CouponController
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> couponDtos = new();
            var result = await _couponService.GetAllCouponsAsync();
            if (result != null && result.IsSuccess)
            {
                couponDtos = JsonConvert.DeserializeObject<List<CouponDto>>(result.Result.ToString());
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return View(couponDtos);
        }

        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _couponService.CreateCouponAsync(couponDto);
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully.";

                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = result.Message;
                }
            }
            return View(couponDto);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var result = await _couponService.GetCouponsByIdAsync(couponId);
            if (result != null && result.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDto>(result.Result.ToString());
                return View(coupon);
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            var result = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully.";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return View(couponDto);
        }
    }
}
