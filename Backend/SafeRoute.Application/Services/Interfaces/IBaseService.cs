using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IBaseService<TReadDto, TCreateDto, TUpdateDto>
        where TReadDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        Task<IEnumerable<TReadDto>> GetAllAsync();

        Task<TReadDto> GetByIdAsync(int id);

        Task<(IEnumerable<TReadDto> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<TReadDto> CreateAsync(TCreateDto dto);

        Task<TReadDto> UpdateAsync(int id, TUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
