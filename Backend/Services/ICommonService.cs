using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface ICommonService<T, TI , TU>
    {
        public List<string> Errors { get; }

        //T : Generic DTO
        //TI : Generic DTOInsert
        //TU: Generic DTOUpdate
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Add(TI InsertDTO);
        Task<T> Update(int id,TU UpdateDTO);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
    }
}
