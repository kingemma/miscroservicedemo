using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    /// <summary>
    /// Coupon API Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto _resposeDto;

        /// <summary>
        /// Coupon API Controller ctor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CouponAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _resposeDto = new ResponseDto();
        }

        /// <summary>
        /// Get All Coupons
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _dbContext.Coupons.ToList();
                _resposeDto.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }

            return _resposeDto;
        }

        /// <summary>
        /// Get by Id
        /// https://localhost:7001/api/CouponAPI/3
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                var couponDto = await _dbContext.Coupons.FirstAsync(x => x.CouponId == id);
                _resposeDto.Result = _mapper.Map<CouponDto>(couponDto);
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }

            return _resposeDto;
        }

        /// <summary>
        /// Get Coupon by code
        /// https://localhost:7001/api/CouponAPI/20OFF
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ResponseDto> GetByCode(string code)
        {
            try
            {
                var couponDto = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() == code.ToLower());
                if (couponDto == null)
                {
                    _resposeDto.IsSuccess = false;
                }
                else
                {
                    _resposeDto.Result = _mapper.Map<CouponDto>(couponDto);
                }
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }
            return _resposeDto;
        }

        /// <summary>
        /// Create Coupon
        /// </summary>
        /// <param name="couponDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Post(CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                await _dbContext.Coupons.AddAsync(coupon);
                await _dbContext.SaveChangesAsync();
                _resposeDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }
            return _resposeDto;
        }

        /// <summary>
        /// Update Coupon
        /// </summary>
        /// <param name="couponDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseDto> Put(CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _dbContext.Coupons.Update(coupon);
                await _dbContext.SaveChangesAsync();
                _resposeDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }
            return _resposeDto;
        }

        /// <summary>
        /// Delete Coupon by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var coupon = await _dbContext.Coupons.FirstAsync(x => x.CouponId == id);
                _dbContext.Coupons.Remove(coupon);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _resposeDto.IsSuccess = false;
                _resposeDto.Message = ex.Message;
            }
            return _resposeDto;
        }
    }
}
