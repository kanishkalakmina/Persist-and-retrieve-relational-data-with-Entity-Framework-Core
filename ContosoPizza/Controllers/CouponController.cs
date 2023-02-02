using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class CouponController : ControllerBase
{
    //A PromotionsContext is injected into the constructor.

    PromotionsContext _context;

    public CouponController(PromotionsContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Coupon> Get() //The Get method returns all the coupons.
    {
        return _context.Coupons
            .AsNoTracking()
            .ToList();
    }
}