using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _looger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> looger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _looger = looger;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge =60)]
        [HttpCacheValidation(MustRevalidate =true)]
        //[ResponseCache(CacheProfileName = "120SecondsDuration")]  
        //[ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            //try
            //{
                var countries = await _unitOfWork.Countries.GetPagedList(requestParams); 
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            //}
            //catch (Exception ex)
            //{
            //    _looger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
            //    return StatusCode(500,"Internal Server Error. Please try again leter");
            //}
        }

        
        [HttpGet("{id:int}", Name = "GetCountry")]
        [ResponseCache(CacheProfileName = "120SecondsDuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            //try
            //{

            //var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels"});
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q=>q.Include(x => x.Hotels));


            var result = _mapper.Map<CountryDTO>(country);
                return Ok(result); 
            //}
            //catch (Exception ex)
            //{
            //    _looger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
            //    return StatusCode(500, "Internal Server Error. Please try again leter");
            //}
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {

            if (!ModelState.IsValid)
            {
                _looger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            //try
            //{
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            //}
            //catch (Exception ex)
            //{
            //    _looger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
            //    return StatusCode(500, "Internal Server Error. Please try again leter");
            //}
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                _looger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            //try
            //{
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);

                if (country == null)
                {
                    _looger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(countryDTO, country);

                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return NoContent();
            //}
            //catch (Exception ex)
            //{
            //    _looger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
            //    return StatusCode(500, "Internal Server Error. Please try again leter");
            //}
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {

            if (id < 1)
            {
                _looger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }

            //try
            //{
                var county = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (county == null)
                {
                    _looger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            //}
            //catch (Exception ex)
            //{
            //    _looger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
            //    return StatusCode(500, "Internal Server Error. Please try again leter");
            //}
        }


    }
}
