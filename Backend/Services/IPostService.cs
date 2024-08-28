using Backend.DTOs;

namespace Backend.Services
{
    public interface IPostService
    {   
        //Porque IEnumerable y no List?
        //IEnumerable es mas rapido y eficiente que una List
        //Ya que en si, trata de un Enumerable que se puede iterar, mientras que las List es una clase
        public Task<IEnumerable<PostDto>> Get();
    }
}
