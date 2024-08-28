using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Backend.Services
{
    public class BeerService : ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO>
    {
        private IRepository<Beer> _beerRepository;
        private IMapper _mapper;

        public List<string> Errors { get; }

        public BeerService(IRepository<Beer> beerRepository,
            IMapper mapper)
        {
            _beerRepository = beerRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<BeerDTO>> Get()
        {
            var beers = await _beerRepository.Get();

            return beers.Select(b => _mapper.Map<BeerDTO>(b));
        }

        public async Task<BeerDTO> GetById(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null){

                var beerDto = _mapper.Map<BeerDTO>(beer);

                return beerDto;
            }

            return null;
        }

        public async Task<BeerDTO> Add(BeerInsertDTO beerInsertDTO)
        {
            var beer = _mapper.Map<Beer>(beerInsertDTO);

            //Primero se le "avisa" al Entity FRMWk que habra un insert
            await _beerRepository.Add(beer);
            //Luego se coloca esto, para realizar ese insert en la BD
            await _beerRepository.Save();

            var beerDto = _mapper.Map<BeerDTO>(beer);

            return beerDto;
        }

        public async Task<BeerDTO> Update(int id, BeerUpdateDTO beerUpdateDTO)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                beer = _mapper.Map<BeerUpdateDTO, Beer>(beerUpdateDTO, beer);

                _beerRepository.Update(beer);
                await _beerRepository.Save();

                var beerDto = _mapper.Map<BeerDTO>(beer);

                return beerDto;
            }

            return null;
        }

        public async Task<BeerDTO> Delete(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                //Se coloca antes del remove para no perder su referencia
                var beerDto = _mapper.Map<BeerDTO>(beer);

                _beerRepository.Delete(beer);
                await _beerRepository.Save();

                return beerDto;
            }

            return null;
        }

        public bool Validate(BeerInsertDTO beerInsertDTO)
        {
            if(_beerRepository.Search(b => b.BeerName == beerInsertDTO.Name).Count() > 0)
            {
                Errors.Add("No puede existir una verceza con un nombre ya existente");
                return false;
            }
            return true;
        }

        public bool Validate(BeerUpdateDTO beerUpdateDTO)
        {
            if (_beerRepository.Search(b => b.BeerName == beerUpdateDTO.Name 
            && beerUpdateDTO.Id != b.BeerID).Count() > 0)
            {
                Errors.Add("No puede existir una verceza con un nombre ya existente");
                return false;
            }
            return true;
        }
    }
}
