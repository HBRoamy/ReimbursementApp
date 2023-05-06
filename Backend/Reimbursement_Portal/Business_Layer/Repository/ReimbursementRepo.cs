using AutoMapper;
using Business_Layer.IRepository;
using Data_Access_Layer.DbContext;
using Data_Access_Layer.Entities;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.RepositoryDAL;
using Microsoft.AspNetCore.JsonPatch;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Repository
{
    public class ReimbursementRepo : IReimbursementRepo
    {

        private readonly AppDbContext _context;
        private readonly IRepositoryDAL<ReimbursementEntity> _repoDAL;
        public ReimbursementRepo(AppDbContext context)
        {
            _context = context;
            _repoDAL = new RepositoryDAL<ReimbursementEntity>(_context);
                // we need dto to entity conversion here  so that is why I am tighly coupling it.
        }
        //private readonly AppDbContext context;

        public async Task<IEnumerable<ReimbursementDTO>> GetAll()
        {
            var entitiesList = await _repoDAL.AllAsync();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReimbursementEntity, ReimbursementDTO>().ReverseMap());
            var mapper = config.CreateMapper();
            var DtoList = mapper.Map<IEnumerable<ReimbursementEntity>, IEnumerable<ReimbursementDTO> >(entitiesList);

            return DtoList;
        }

        public async Task<IEnumerable<ReimbursementDTO>> Search(string requestedForEmail)
        {

            var requestedList = await _repoDAL.FilterAsync(x=> x.CreatedBy==requestedForEmail.ToLower());
            if (requestedList != null)
            {
                var result = MyCfgMap<ReimbursementEntity, ReimbursementDTO>().Map<IEnumerable<ReimbursementEntity>, IEnumerable<ReimbursementDTO>>(requestedList);
                return result;
            }
            return null;
        }

        public async Task<ReimbursementDTO> GetRequestById(int id)
        {
            var entity =  await _repoDAL.GetByIdAsync(id);
            if (entity != null)
            {
                var dto = MyCfgMap<ReimbursementDTO, ReimbursementEntity>().Map<ReimbursementDTO>(entity);
                return dto;
            }
            return null;
        }

        public async Task PatchRequest(int requestId, JsonPatchDocument patchRequestBody)
        {

            var searchedReimbursement = await _repoDAL.GetByIdAsync(requestId);
            if (searchedReimbursement != null)
            {
                patchRequestBody.ApplyTo(searchedReimbursement);
                await SaveAsync();
            }
            //1 all
            //2 medical
            //3 travel
            //4 food
            //5 entertainment
            //6 misc.
        }

        public async Task<int> CreateRequest(ReimbursementDTO newRequestDto) // takes DTO sends DTO
        {
            if (newRequestDto != null)
            {
                var mappedEntity = MyCfgMap<ReimbursementDTO, ReimbursementEntity>().Map<ReimbursementEntity>(newRequestDto);
                var newlyCreated = await _repoDAL.CreateAsync(mappedEntity);
                //await _repoDAL.SaveAsync();

                //var receivedDto = MyCfgMap<ReimbursementDTO, ReimbursementEntity>().Map<ReimbursementDTO>(newlyCreated);
                //return receivedDto;
                return newlyCreated.id;

            }
            return -1;
        }


        public async Task<int> EditRequest(int Id,ReimbursementDTO newRequestDto) // takes DTO sends DTO
        {
            if(Id>0 && newRequestDto != null)
            {
                var item = await _repoDAL.GetByIdAsync(Id);

                if (item != null)
                {
                    item.Date = newRequestDto.Date;
                    item.ReimbursementType = newRequestDto.ReimbursementType;
                    item.RequestedValue = newRequestDto.RequestedValue;
                    item.Currency = newRequestDto.Currency;
                    item.ReceiptFile = newRequestDto.ReceiptFile;
                    
                    var mappedToDto = MyCfgMap<ReimbursementDTO, ReimbursementEntity>().Map<ReimbursementDTO>(item);
                    await _repoDAL.SaveAsync();
                    return mappedToDto.id;
                }

            }
            return -1;
        }


        //below is for pending declined and approved requests
        public async Task<IEnumerable<ReimbursementDTO>> GetRequestByType(int type)
        {
            IEnumerable<ReimbursementDTO> result = new List<ReimbursementDTO>();
            switch (type)
            {
                case 0: var receivedPending = await _repoDAL.FilterAsync(x => x.RequestPhase == 0);
                    result = MyCfgMap<ReimbursementEntity, ReimbursementDTO>().Map<IEnumerable<ReimbursementEntity>, IEnumerable<ReimbursementDTO>>(receivedPending);
                    break;
                case 1: var receivedApproved = await _repoDAL.FilterAsync(x => x.RequestPhase == 1);
                    result = MyCfgMap<ReimbursementEntity, ReimbursementDTO>().Map<IEnumerable<ReimbursementEntity>, IEnumerable<ReimbursementDTO>>(receivedApproved);
                    break;
                case 2: var receivedDeclined = await _repoDAL.FilterAsync(x => x.RequestPhase == 2);
                    result = MyCfgMap<ReimbursementEntity, ReimbursementDTO>().Map<IEnumerable<ReimbursementEntity>, IEnumerable<ReimbursementDTO>>(receivedDeclined);
                    break;
                default: return null;
            }
            return result;
        }


        public async Task<bool> DeleteRequestAsync(int id)
        {
            return await _repoDAL.DeleteAsync(id);
        }

        public async Task SaveAsync()
        {
            await _repoDAL.SaveAsync();
        }


        //get single item by Id, to be used while editing forms

        public IMapper MyCfgMap<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>().ReverseMap());
            return config.CreateMapper();
        }

    }
}
