using Microsoft.AspNetCore.JsonPatch;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.IRepository
{
    public interface IReimbursementRepo
    {
        public Task<IEnumerable<ReimbursementDTO>> GetAll();
        public Task<IEnumerable<ReimbursementDTO>> Search(string requestedForEmail);
        public Task<int> CreateRequest(ReimbursementDTO newRequestDto);
        public Task<ReimbursementDTO> GetRequestById(int id);
        public Task<bool> DeleteRequestAsync(int id);
        public Task<int> EditRequest(int Id, ReimbursementDTO newRequestDto);
        public Task<IEnumerable<ReimbursementDTO>> GetRequestByType(int type);
        public Task PatchRequest(int Id, JsonPatchDocument patchRequestBody);
        public Task SaveAsync();

    }
}
