using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using FluentValidation;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase {

        private IValidator<BeerInsertDTO> _beerInsertValidator;
        private IValidator<BeerUpdateDTO> _beerUpdateValidator;
        private ICommonService<BeerDTO,BeerInsertDTO,BeerUpdateDTO> _beerService;

        public BeerController(IValidator<BeerInsertDTO> beerInsertValidator,
            IValidator<BeerUpdateDTO> beerUpdateValidator,
            [FromKeyedServices("beerService")]ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO> beerService)
        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerService = beerService;
        }

        [HttpGet]
        public async Task<IEnumerable<BeerDTO>> Get() =>
            await _beerService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDTO>> GetById(int id)
        {
            var beerDto = await _beerService.GetById(id);

            return beerDto == null ? NotFound() : Ok(beerDto);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDTO>> Add(BeerInsertDTO beerInsertDTO)
        {
            //Con esto se verifica que sea valido la beer recibida
            //Con la refactorizacion de codigo, la validacion es ideal que siga en el controlador
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDTO);

            //Si es valida, sigue con el flujo, si no, retorna Bad Request
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if(!_beerService.Validate(beerInsertDTO))
            {
                return BadRequest(_beerService.Errors);
            }

            var beerDto = await _beerService.Add(beerInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = beerDto.Id }, beerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDTO>> Update(int id, BeerUpdateDTO beerUpdateDTO)
        {
            var validationResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_beerService.Validate(beerUpdateDTO))
            {
                return BadRequest(_beerService.Errors);
            }

            var beerDto = await _beerService.Update(id, beerUpdateDTO);

            return beerDto == null ? NotFound() : Ok(beerDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDTO>> Delete(int id) 
        {
            var beerDto = await _beerService.Delete(id);

            return beerDto == null ? NotFound() : Ok(beerDto);
        }

    }
}
