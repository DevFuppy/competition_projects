using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Application.Queries;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace EFcoreRepoPractice.Controllers
{

    //[ApiController]               
    //[Route("[controller]")]   
    public class MemberController : Controller
    {

        //private readonly GetMemberDetailHandler _handler;
        //public MemberController(GetMemberDetailHandler handler) => _handler = handler;


        private readonly IMemberRepository _memberRepository;
        public MemberController(IMemberRepository memberRepository) => _memberRepository = memberRepository;



        //[HttpPost]

        public IActionResult Index()
        {
            return View();
        }

        //("{id:int}")
        //[HttpGet("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<MemberDTO>> Get(int id, CancellationToken ct)
        {

            //var dto = await _handler.Handler(new(id), ct);
            var dto = await _memberRepository.GetAsync(id, ct);

            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<MemberDTO>> Create([FromBody] CreateMemberDTO dto, CancellationToken ct)
        {
            //CreateMemberDTO dto = new CreateMemberDTO { Name = name, Email = email, Age =age  };

            var member = new Member { Name = dto.Name, Email = dto.Email, Age = dto.Age };


            await _memberRepository.CreateAsync(member, ct);

            //var dto = await _memberRepository.CreateAsync(dto., ct);

            return CreatedAtAction(nameof(Get), new { id = member.MemberId }, member);
        }

        [HttpPost]
        public async Task<ActionResult<MemberDTO>> Update([FromBody] UpdateMemberDTO dto, CancellationToken ct)
        {


            var existing = await _memberRepository.GetAsync(dto.Id);

            if (existing == null)
            {
                return NotFound();
            }

            //也可以這樣寫
            //var member = new Member { MemberId = dto.Id, Name = dto.Name, Email = dto.Email, Age = dto.Age };

            existing.Name = dto.Name;

            existing.Email = dto.Email;

            existing.Age = dto.Age;

            await _memberRepository.UpdateAsync(existing, ct);


            return Ok(existing);
        }



        //[HttpPost]
        //public async Task<ActionResult<MemberDTO>> Delete([FromBody] MemberDTO dto, CancellationToken ct)
        //{

        //    var existing = await _memberRepository.GetAsync(dto.Id);

        //    if (existing == null)
        //    {
        //        return NotFound();
        //    }


        //    await _memberRepository.DeleteAsync(existing, ct);

        //    //var dto = await _memberRepository.CreateAsync(dto., ct);

        //    return NoContent();
        //}

        /// <summary>
        /// 表達式新寫法
        /// 「something 不是 null，而且 something 的型別正確（例如 Member）」
        ///  something is { }
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MemberDTO>> Delete([FromBody] MemberDTO dto, CancellationToken ct) =>
            await _memberRepository.GetAsync(dto.Id) is {} existing ? NotFound() : NoContent();
         



    }
}
