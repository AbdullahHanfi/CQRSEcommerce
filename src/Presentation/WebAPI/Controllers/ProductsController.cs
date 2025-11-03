using Application.DTOs.User;
using Application.Features.Auth.Command.Login;
using Application.Features.Products.Command.AddProduct;
using Application.Features.Products.Query.GetAllProduct;
using Application.Request.Product;
using Domain.Exceptions;
using Domain.Shared.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ApiController
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Index(int pageSize)
        {
            try
            {
                var result = await Sender.Send(new GetAllProductQuery(pageSize));
                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                {
                    var tuple = new ValidationErrorResponse("Exception", [result.Error.Message]);
                    return BadRequest(tuple);
                }
            }
            catch (ValidationException ex)
            {

                var tuples = ex.Errors
                    .Select(err => new ValidationErrorResponse(err.Key, err.Value))
                    .ToArray();

                return BadRequest(tuples);
            }
            catch (Exception ex)
            {
                var tuple = new ValidationErrorResponse("Exception", [ex.Message]);
                return BadRequest(tuple);
            }
        }
        [HttpPost("/api/product")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromForm] ProductRequst request)
        {
            try
            {
                using Stream stream = request.Image.OpenReadStream();
                var result = await Sender.Send(new AddProductCommand(request.ProductCode, request.Name, stream,request.Image.ContentType, request.Price, request.Quantity, request.DiscountRate, request.Category));
                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                {
                    var tuple = new ValidationErrorResponse("Exception", [result.Error.Message]);
                    return BadRequest(tuple);
                }
            }
            catch (ValidationException ex)
            {

                var tuples = ex.Errors
                    .Select(err => new ValidationErrorResponse(err.Key, err.Value))
                    .ToArray();

                return BadRequest(tuples);
            }
            catch (Exception ex)
            {
                var tuple = new ValidationErrorResponse("Exception", [ex.Message]);
                return BadRequest(tuple);
            }
        }
    }

}
