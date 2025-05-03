using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Application.Queries;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EFcoreRepoPractice.Controllers
{

    //[ApiController]               
    //[Route("[controller]")]   
    public class MemberController : Controller
    {

        //private readonly GetMemberDetailHandler _handler;
        //public MemberController(GetMemberDetailHandler handler) => _handler = handler;
        //public MemberController(IMemberRepository memberRepository) => _memberRepository = memberRepository;
        //private readonly AjaxClassContext _context;


        //private readonly IMemberRepository _memberRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IUnitOfWork _uow;
        private readonly GetMemberDetailHandler _memberHandler;

        public MemberController(IRepository<Member> IRepo, IUnitOfWork unow, GetMemberDetailHandler memberHandler   )
        {
            _memberRepository = IRepo;
            _uow = unow;
            _memberHandler = memberHandler;

            //_context = context;
        }



        ////[HttpPost]

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Index()
        //{
        //    var list = _context.Members.AsNoTracking().ToList(); // 不用 async，模仿 ADO.NET 行為
        //    return View(list);
        //}



        //("{id:int}")
        //[HttpGet("{id:int}")]
        [HttpGet]
        public async Task<ActionResult> Get(int id, CancellationToken ct)
        {

            //var dto = await _handler.Handler(new(id), ct);
            //var dto = await _memberRepository.GetAsync(id, ct);
            //var entity = _uow.GetRepository<Member>();
            //var model = await entity.GetAsync(id, ct);
            var handler = await _memberHandler.GetMemberHandler(new(id),ct);           
            

            return handler is null ? NotFound() : Ok(handler);
        }


        [HttpGet]
        public async Task<ActionResult> GetAll(CancellationToken ct)
        {

            //var dto = await _handler.Handler(new(id), ct);
            //var members = await _memberRepository.GetAllAsync(ct);
            var entity = _uow.GetRepository<Member>();
            var members = await entity.GetAllAsync();

            var vm = members.Select(x => new { Id = x.MemberId, x.Name, x.Email, x.Age, x.Password });

            return vm is null ? NotFound() : Ok(vm);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateMemberDTO dto, CancellationToken ct)
        {
            //CreateMemberDTO dto = new CreateMemberDTO { Name = name, Email = email, Age =age  };

            var member = new Member { Name = dto.Name, Email = dto.Email, Age = dto.Age };


            //await _memberRepository.CreateAsync(member, ct);           
            //await _memberRepository.Save();

            var entity = _uow.GetRepository<Member>();
            await entity.CreateAsync(member, ct);
            await entity.Save();


            return CreatedAtAction(nameof(Get), new { id = member.MemberId }, member);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] UpdateMemberDTO dto, CancellationToken ct)
        {


            //var existing = await _memberRepository.GetAsync(dto.Id);
            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetAsync(dto.Id);

            if (existing == null)
            {
                return NotFound();
            }

            //也可以這樣寫
            //var member = new Member { MemberId = dto.Id, Name = dto.Name, Email = dto.Email, Age = dto.Age };

            existing.Name = dto.Name;

            existing.Email = dto.Email;

            existing.Age = dto.Age;

            //await _memberRepository.UpdateAsync(existing, ct);
            //await _memberRepository.Save();
            await entity.UpdateAsync(existing);
            await entity.Save(ct);

            return Ok(existing);
        }



        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] MemberDTO dto, CancellationToken ct)
        {

            //var existing = await _memberRepository.GetAsync(dto.Id);
            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetAsync(dto.Id, ct);

            if (existing == null)
            {
                return NotFound();
            }


            //await _memberRepository.DeleteAsync(existing, ct);
            //await _memberRepository.Save();
            await entity.DeleteAsync(existing);
            await entity.Save(ct);



            return NoContent();
        }

        /// <summary>
        /// 表達式新寫法
        /// 「something 不是 null，而且 something 的型別正確（例如 Member）」
        ///  something is { }
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<ActionResult<MemberDTO>> Delete([FromBody] MemberDTO dto, CancellationToken ct) =>
        //    await _memberRepository.GetAsync(dto.Id) is { } existing ? NotFound() : NoContent();




    }
}
