using AutoMapper;
using Business_Layer.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Reimbursement_Portal_API.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_Portal_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)] // use it after everything works like a charm
    public class ReimbursementsController : ControllerBase
    {
        private readonly IReimbursementRepo repo;
        private readonly IWebHostEnvironment _env;

        public ReimbursementsController(IReimbursementRepo repo, IWebHostEnvironment env)
        {
            this.repo = repo;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequests()
        {
            var result = await repo.GetAll();

            if (result != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ReimbursementDTO, ReimbursementRequestModel>());
                var mapper = config.CreateMapper();
                //var mp = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>();
                var ModelList = mapper.Map<IEnumerable<ReimbursementDTO>, IEnumerable<ReimbursementRequestModel>>(result);

                return Ok(ModelList);
            }
            return NotFound();
        }

        [HttpGet("LookFor/{type:int}")]
        public async Task<ActionResult> GetRequestsByType([FromRoute]int type)
        {
            var receivedDto = await repo.GetRequestByType(type);
            if (receivedDto != null)
            {
                    receivedDto.OrderByDescending(x => x.Date);
                var result = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<IEnumerable<ReimbursementDTO>, IEnumerable<ReimbursementRequestModel>>(receivedDto);
                for (int i = 0; i < result.Count(); i++)
                {
                    for (int j = 0; j < result.Count(); j++)
                    {
                        if (result.ElementAt(i).Date > result.ElementAt(j).Date)
                        {
                            var temp = result.ElementAt(i).Date;
                            //"uploadedfiles" + Filename => File location ..read and convert them tobase 64
                            result.ElementAt(i).Date = result.ElementAt(j).Date;
                            result.ElementAt(j).Date = temp;
                        }
                    }
                }
                
                return Ok(result);
            }
            return NotFound();
        }

        //[Authorize]
        [HttpGet("Search")]//uses query params as we havent use a route parameter
        public async Task<ActionResult> Search([FromQuery]string requestedForEmail)// this will be the search query name
        {

            //filter by travel etc can be done on the frontend too with the existing content on the page.
            var requestedList = await repo.Search(requestedForEmail);

            if (requestedList != null)
            {
                
                var result = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<IEnumerable<ReimbursementDTO>, IEnumerable<ReimbursementRequestModel>>(requestedList);
                for (int i = 0; i < result.Count(); i++)
                {
                    for (int j = 0; j < result.Count(); j++)
                    {
                        if (result.ElementAt(i).Date > result.ElementAt(j).Date)
                        {
                            var temp = result.ElementAt(i).Date;
                            result.ElementAt(i).Date = result.ElementAt(j).Date;
                            result.ElementAt(j).Date = temp;
                        }
                    }
                }
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewRequest(ReimbursementRequestModel ReqResModel)
        {
            // check for the request if its not null

            if (ReqResModel != null)
            {
               
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ReimbursementDTO, ReimbursementRequestModel>().ReverseMap());
                var mapper = config.CreateMapper();

                var mappedToDto = mapper.Map<ReimbursementDTO>(ReqResModel);
                var receivedId = await repo.CreateRequest(mappedToDto);
                //var result = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<ReimbursementRequestModel>(receivedDto);
                //return Created("/api/[controller]/" + result.id,result);
                return CreatedAtAction(nameof(GetRequestById), new { id = receivedId, controller= "Reimbursements" }, receivedId);
            }
            return BadRequest();

        }

        [HttpGet("DataArrays")]
        public async Task<ActionResult> GetDataArrays()
        {
            var result = await repo.GetAll();

            int[] arrayTypes = new int[5]; // send anonymous object with 2 arrays
            int[] arrayMonthwise = new int[12];
            if (result != null)
            {
                
                for (int i = 0; i < result.Count(); i++)
                {
                    //for reimbursement types
                    switch (result.ElementAt(i).ReimbursementType)
                    {
                        case "Medical":
                            arrayTypes[0]++;
                            break;
                        case "Travel":
                            arrayTypes[1]++;
                            break;
                        case "Food":
                            arrayTypes[2]++;
                            break;
                        case "Entertainment":
                            arrayTypes[3]++;
                            break;
                        case "Misc.":
                            arrayTypes[4]++;
                            break;
                        default: break;
                    }

                   
                }
                for (int j = 0; j < result.Count(); j++)
                {
                    //for monthly data

                    switch (result.ElementAt(j).Date.Month)
                    {
                        case 01: arrayMonthwise[0]++;
                            break;
                        case 02:
                            arrayMonthwise[1]++;
                            break;
                        case 03:
                            arrayMonthwise[2]++;
                            break;
                        case 04:
                            arrayMonthwise[3]++;
                            break;
                        case 05:
                            arrayMonthwise[4]++;
                            break;
                        case 06:
                            arrayMonthwise[5]++;
                            break;
                        case 07:
                            arrayMonthwise[6]++;
                            break;
                        case 08:
                            arrayMonthwise[7]++;
                            break;
                        case 09:
                            arrayMonthwise[8]++;
                            break;
                        case 10:
                            arrayMonthwise[9]++;
                            break;
                        case 11:
                            arrayMonthwise[10]++;
                            break;
                        case 12:
                            arrayMonthwise[11]++;
                            break;
                        default: break;
                    }
                }
                //foreach (var item in arrayMonthwise)
                //{
                //    Console.WriteLine(item + " ");

                //}
                return Ok( new { arr1 = arrayTypes, arr2= arrayMonthwise });
            }
            return NotFound(new { arr1 = arrayTypes, arr2 = arrayMonthwise });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRequestById([FromRoute]int id)
        {
            var dto = await repo.GetRequestById(id);
            if (dto != null)
            {
            var model = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<ReimbursementRequestModel>(dto);
                return Ok(model);
            }
            return NotFound();
        }
        
        [HttpPut("{id:int}")] // since it is a post request I have to pass all the values from the frontend side otherwise the values that arent present will be set to default
        public async Task<ActionResult> EditRequest([FromRoute]int id, [FromBody] ReimbursementRequestModel ReqResModel)
        {
            if (ReqResModel != null)
            {
                var mappedToDto = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<ReimbursementDTO>(ReqResModel);
                var receivedId = await repo.EditRequest(id, mappedToDto);
                //var result = MyCfgMap<ReimbursementDTO, ReimbursementRequestModel>().Map<ReimbursementRequestModel>(receivedDto);
                return CreatedAtAction(nameof(GetRequestById), new { id = receivedId, controller = "Reimbursements" }, receivedId);
            }
            return BadRequest();
        }

        //[HttpPost("passjwt")]
        //public ActionResult DeclineOrApproveRequest([FromHeader] object jwt)
        //{
        //    Console.WriteLine(jwt);
        //    return Ok();
        //}


        [HttpPatch("{id:int}")] // since it is a post request I have to pass all the values from the frontend side otherwise the values that arent present will be set to default
        public async Task<ActionResult> PatchRequest([FromRoute] int id, [FromBody] JsonPatchDocument ReqResModel)
        {
            //if request phase > 0 then dont allow editing or deleting
            if (ReqResModel != null)
            {
                await repo.PatchRequest(id, ReqResModel);
                return Ok();
            }
            return BadRequest();
        }



        [HttpDelete("{id}")]
        public async Task<bool> DeleteRequestAsync(int id)
        {
            //if request phase > 0 then dont allow editing or deleting
            return await repo.DeleteRequestAsync(id); 
        }

        //[AllowAnonymous]
        //[HttpPost("file")]
        //public ActionResult SaveFile()
        //{
        //    var httpRequest = Request.Form;
        //    var postedFile = httpRequest.Files[0];
        //    string filename = postedFile.FileName;
        //    var physicalPath = _env.WebRootPath + "/UploadedFiles/" + "_" + Guid.NewGuid() + filename;

        //    using (var stream = new FileStream(physicalPath, FileMode.Create))
        //    {
        //        postedFile.CopyTo(stream);
        //    }
        //    return Ok(new { path = physicalPath });
        //}



        public IMapper MyCfgMap<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>().ReverseMap());
            return config.CreateMapper();
        }
    }
}
