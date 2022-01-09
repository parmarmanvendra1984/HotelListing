﻿using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _looger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> looger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _looger = looger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please try again leter");
            }
        }

        
        [HttpGet("{id:int}",Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                //var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id,new List<string> { "Country"});
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, include: q => q.Include(x => x.Country));
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Server Error. Please try again leter");
            }
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {


            if (!ModelState.IsValid)
            {
                _looger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id},hotel);
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error. Please try again leter");
            }
        }


        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                _looger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);

                if (hotel == null)
                {
                    _looger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(hotelDTO, hotel);

                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server Error. Please try again leter");
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {

            if (id < 1)
            {
                _looger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _looger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server Error. Please try again leter");
            }
        }


    }
}
