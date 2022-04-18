using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectX.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly IProjectXRepository<Tenant, int> _repository;
        private readonly IMapper _mapper;

        public TenantController(IProjectXRepository<Tenant, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<TenantDto> Get()
        {
            var tenants = _repository.GetAll().ToList();
            return _mapper.Map<List<TenantDto>>(tenants);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<TenantDto> Get(int id)
        {
            var tenant = await _repository.GetByPrimaryKeyAsync(id);
            return _mapper.Map<TenantDto>(tenant);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task PostAsync([FromBody] CreateTenantDto input)
        {
            if (input == null)
                throw new HttpRequestException("Model is empty");

            var tenant = _mapper.Map<Tenant>(input);
            await _repository.InsertAsync(tenant);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] UpdateTenantDto input)
        {
            var tenant = await _repository.GetByPrimaryKeyAsync(id);
            _mapper.Map<UpdateTenantDto, Tenant>(input, tenant);
            await _repository.UpdateAsync(tenant);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
