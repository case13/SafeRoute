using AutoMapper;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class BaseService<TEntity, TReadDto, TCreateDto, TUpdateDto> : IBaseService<TReadDto, TCreateDto, TUpdateDto>
        where TEntity : BaseEntity, new()
        where TReadDto : class, new()
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(
            IBaseRepository<TEntity> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                if (entities == null || !entities.Any())
                    return Enumerable.Empty<TReadDto>();

                return _mapper.Map<IEnumerable<TReadDto>>(entities);
            }
            catch (Exception)
            {
                return Enumerable.Empty<TReadDto>();
            }
        }

        public virtual async Task<TReadDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new TReadDto();

                return _mapper.Map<TReadDto>(entity);
            }
            catch (Exception)
            {
                return new TReadDto();
            }
        }

        public virtual async Task<(IEnumerable<TReadDto> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            try
            {
                var (entities, totalCount) =
                    await _repository.GetPagedAsync(pageNumber, pageSize);

                if (entities == null || !entities.Any())
                    return (Enumerable.Empty<TReadDto>(), 0);

                var dtos = _mapper.Map<IEnumerable<TReadDto>>(entities);
                return (dtos, totalCount);
            }
            catch (Exception)
            {
                return (Enumerable.Empty<TReadDto>(), 0);
            }
        }

        public virtual async Task<TReadDto> CreateAsync(TCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                if (entity == null)
                    return new TReadDto();

                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                return _mapper.Map<TReadDto>(entity);
            }
            catch (Exception)
            {
                return new TReadDto();
            }
        }

        public virtual async Task<TReadDto> UpdateAsync(int id, TUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new TReadDto();

                _mapper.Map(dto, entity);

                await _repository.UpdateAsync(entity);
                await _repository.SaveChangesAsync();

                return _mapper.Map<TReadDto>(entity);
            }
            catch (Exception)
            {
                return new TReadDto();
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return false;

                await _repository.DeleteAsync(entity);
                await _repository.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
