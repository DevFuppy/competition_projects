using EFcoreRepoPractice.Application;
using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Application.Queries.MemberQueries;
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
        //private readonly IRepository<Member> _memberRepository;
        //private readonly IUnitOfWork _uow;
        private readonly GetMemberDetailHandler _memberGet;
        private readonly CreateMemberHandler _memberCreate;
        private readonly UpdateMemberHandler _memberUpdate;
        private readonly DeleteMemberHandler _memberDelete;

        public MemberController(
            IRepository<Member> IRepo,
            //IUnitOfWork unow, 
            GetMemberDetailHandler memberGet,
            CreateMemberHandler memberCreate,
            UpdateMemberHandler memberUpdate,
            DeleteMemberHandler memberDelete
            )
        {
            //_context = context;
            //_memberRepository = IRepo;
            //_uow = unow;
            _memberGet = memberGet;
            _memberCreate = memberCreate;
            _memberUpdate = memberUpdate;
            _memberDelete = memberDelete;
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
        #region 登入
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lgvm)
        {
            
            await _memberCreate.MemberRegistration(new(lgvm.Email, lgvm.Password));

            return RedirectToAction("GetAll");

        }




        #endregion


        #region 註冊
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rgvm)
        {

            //Model.state
            //要記得寫trycatch
            //ViewData丟出去可顯示新增哪一筆
            

            await _memberCreate.MemberRegistration(new(rgvm.Email,rgvm.Password));

            return RedirectToAction("GetAll");

        }
        #endregion

        #region 查詢
        //("{id:int}")
        //[HttpGet("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<MemberDTO?>> Get(GetDetailQueryById request, CancellationToken ct)
        {

            //var dto = await _handler.Handler(new(id), ct);
            //var dto = await _memberRepository.GetAsync(id, ct);
            //var entity = _uow.GetRepository<Member>();
            //var model = await entity.GetAsync(id, ct);
            var handler = await _memberGet.GetMemberHandler(request, ct);


            return handler is null ? NotFound() : Ok(handler);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {

            //var dto = await _handler.Handler(new(id), ct);
            //var members = await _memberRepository.GetAllAsync(ct);

            //var entity = _uow.GetRepository<Member>();
            //var members = await entity.GetAllAsync();
            //var vm = members.Select(x => new { Id = x.MemberId, x.Name, x.Email, x.Age, x.Password });

            ViewBag.Test = "測試用";

            IEnumerable<MemberDTO?>? handler = await _memberGet.GetAllMemberHandler(ct);
            return handler is null ? NotFound() : View(handler);


        }
        #endregion

        #region CUD範例
        [HttpPost]
        public async Task<ActionResult<MemberDTO?>> Create([FromBody] CreateMemberCommand cmd, CancellationToken ct)
        {


            //var member = new Member { Name = dto.Name, Email = dto.Email, Age = dto.Age };
            //await _memberRepository.CreateAsync(member, ct);           
            //await _memberRepository.Save();

            //var entity = _uow.GetRepository<Member>();
            //await entity.CreateAsync(member, ct);
            //await entity.Save();

            var handler = await _memberCreate.CreateOneMember(cmd, ct);
            return CreatedAtAction(nameof(Get), new { id = handler?.Id }, handler);
        }

        [HttpPost]
        public async Task<ActionResult<MemberDTO?>> Update([FromBody] UpdateMemberCommand cmd, CancellationToken ct)
        {


            //var existing = await _memberRepository.GetAsync(dto.Id);

            //var entity = _uow.GetRepository<Member>();
            //var existing = await entity.GetAsync(dto.Id);

            //if (existing == null)
            //{
            //    return NotFound();
            //}

            ////也可以這樣寫
            ////var member = new Member { MemberId = dto.Id, Name = dto.Name, Email = dto.Email, Age = dto.Age };

            //existing.Name = dto.Name;

            //existing.Email = dto.Email;

            //existing.Age = dto.Age;

            ////await _memberRepository.UpdateAsync(existing, ct);
            ////await _memberRepository.Save();
            //await entity.UpdateAsync(existing);
            //await entity.Save(ct);

            var handler = await _memberUpdate.UpdateOneMember(cmd, ct);
            return handler is null ? NotFound() : Ok(handler);
        }



        [HttpGet]
        public async Task<ActionResult<MemberDTO>> Delete(DeleteMemberCommand dcmd, CancellationToken ct)
        {

            ////var existing = await _memberRepository.GetAsync(dto.Id);
            //var entity = _uow.GetRepository<Member>();
            //var existing = await entity.GetAsync(dto.Id, ct);

            //if (existing == null)
            //{
            //    return NotFound();
            //}


            ////await _memberRepository.DeleteAsync(existing, ct);
            ////await _memberRepository.Save();
            //await entity.DeleteAsync(existing);
            //await entity.Save(ct);


            MemberDTO? handler = await _memberDelete.DeleteOneMember(dcmd, ct);
            TempData["DeletedMember"] = handler?.Name+" "+handler?.Email;
            //return handler is null ? NoContent() : Ok(handler);
            return RedirectToAction(nameof(GetAll));
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


        #endregion

    }


}
