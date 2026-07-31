using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetPayments([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var (items, totalCount) = await _paymentService.GetPagedPaymentsAsync(pageNumber.Value, pageSize.Value);
                var mappedItems = _mapper.Map<IEnumerable<PaymentListDto>>(items);
                return Ok(new PaginatedResultDto<PaymentListDto>
                {
                    Items = mappedItems,
                    TotalCount = totalCount
                });
            }

            var payments = await _paymentService.GetPaymentsAsync();
            return Ok(_mapper.Map<IEnumerable<PaymentListDto>>(payments));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentListDto>> CreatePayment(PaymentListDto dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userIdStr, out var userId);

            var payment = await _paymentService.CreatePaymentAsync(dto, userId);
            if (payment == null) return BadRequest("Could not record payment");

            return CreatedAtAction(nameof(GetPayments), new { id = payment.Id }, _mapper.Map<PaymentListDto>(payment));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(Guid id, PaymentListDto dto)
        {
            var result = await _paymentService.UpdatePaymentAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
