using BCrypt.Net;
using EFcoreRepoPractice.Application;
using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.Commands.VerifyEmailCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Application.Queries.MemberQueries;
using EFcoreRepoPractice.Application.Services;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static EFcoreRepoPractice.Application.Queries.EmailQueries.EmailDetailQuery;

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
        private readonly IAuthService _iau;
        private readonly GetMemberDetailHandler _memberGet;
        private readonly CreateMemberHandler _memberCreate;
        private readonly UpdateMemberHandler _memberUpdate;
        private readonly DeleteMemberHandler _memberDelete;
        private readonly LoginHandler _memberLogin;
        private readonly VerifyEmailHandler _emailhandler;

        public MemberController(
            //IRepository<Member> IRepo,
            IAuthService iau,
            //IUnitOfWork unow, 
            GetMemberDetailHandler memberGet,
            CreateMemberHandler memberCreate,
            UpdateMemberHandler memberUpdate,
            DeleteMemberHandler memberDelete,
            LoginHandler memberLogin,
            VerifyEmailHandler emailhandler
            )
        {
            //_context = context;
            //_memberRepository = IRepo;
            //_uow = unow;
            _iau = iau;
            _memberGet = memberGet;
            _memberCreate = memberCreate;
            _memberUpdate = memberUpdate;
            _memberDelete = memberDelete;
            _memberLogin = memberLogin;
            _emailhandler = emailhandler;
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

        #region 忘記密碼

        [HttpGet]
        public ActionResult ForgotPassword()
        {

            return View();

        }

        [HttpPost]
        public async Task<ActionResult> ForgotPasswordSendingEmail(ForgetPasswordViewModel fg)
        {

            string token = Guid.NewGuid().ToString();

            string? url = Url.Action("UpdatePassword", "Member", new { token }, protocol: Request.Scheme);

            await _emailhandler.SendEmailwithTokenAsync(new(Email: fg.Email), url ?? "");

            return RedirectToAction("Login");

        }

        [HttpGet]
        public ActionResult UpdatePassword(string token)
        {
            ViewBag.Token = token;
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> UpdatePasswordAction(UpdatePasswordViewModel up, CancellationToken ct)
        {

            dynamic result = await _emailhandler.UpdatePasswordWithTokenAsync(up, ct);
            
            Type type = result.GetType();
            
            TempData["UpdatePasswordMsg"]  = type == typeof(String)? result:"變更成功";            
            
            return RedirectToAction("Login");
        }



        #endregion

        #region 登入/登出

        /// <summary>
        /// 登入畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// 登入動作與驗證
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lgvm)
        {

            if (!ModelState.IsValid)
                return View();

            try
            {

                MemberDTO? result = await _memberLogin.LoginVerification(new(lgvm.Email, lgvm.Password));


                if (result is null)
                {
                    TempData["LoginMsg"] = "帳號或密碼錯誤";
                    return View(result);

                }


                TempData["LoginMsg"] = "登入成功";

                await _iau.SignInAsync(result);
                return RedirectToAction("GetAll");
            }
            catch (SaltParseException Salt)
            {

                TempData["LoginMsg"] = "資料庫層級錯誤: "+ Salt.Message;
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {

                TempData["LoginMsg"] = "系統錯誤，請聯絡管理員，錯誤類型: " + ex.Message;
                return RedirectToAction("Login");

            }

            


        }

        /// <summary>
        /// 登出動作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Json(new { msg = "登出成功", redirectURL = "/Member/Login" });
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


            await _memberLogin.MemberRegistration(new(rgvm.Email, rgvm.Password));

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



        [Authorize]
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

            var handler = await _memberUpdate.UpdateOneMemberAsync(cmd, ct);
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
            TempData["DeletedMember"] = handler?.Name + " " + handler?.Email;
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






        #region Ajax測試區

        public record TestAjaxModel(int? Id = null, string? Email = null, string? Name = null, int? Age = null, bool? Fact = true);


        [HttpPost]
        public IActionResult AjaxFromBody([FromBody]TestAjaxModel tam) {

            var newTam = tam with { Age=tam.Age+20 };

            return Json(newTam);
        }


        [HttpGet]
        public IActionResult AjaxGet(string Email)
        {

            Email += "加了料";

            return Ok(Email);
        }


        [HttpPost]
        public IActionResult AjaxPostFromForm(TestAjaxModel tam)
        {

            var newTam = tam with { Age = tam.Age + 20 };

            return Json(newTam);
        }



        #endregion
    }


}
