using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TapInMotion.Models.ViewModels;
using TapInMotion.Models;
using TapInMotion.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TapInMotion.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private ApplicationDbContext _context;

        public AccountController(
            UserManager<AppUser> userMgr,
            SignInManager<AppUser> signInMgr,
            ApplicationDbContext context
        )
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _context = context;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            System.Console.WriteLine(ModelState.ErrorCount);
            if (ModelState.IsValid)
            {
                AppUser? user = await userManager.FindByEmailAsync(loginModel.Email.ToUpper());
                // System.Console.WriteLine(user.Email + user.Id);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if (
                        (
                            await signInManager.PasswordSignInAsync(
                                user,
                                loginModel.Password,
                                false,
                                false
                            )
                        ).Succeeded
                    )
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Account/Manage");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        // GET: /<controller>/
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View(userManager.Users);
        }

        [Authorize]
        public async Task<ViewResult> Manage()
        {
            AppUser? user = await userManager.FindByNameAsync(User.Identity?.Name ?? "");
            if (user != null)
            {
                System.Console.WriteLine(user.Id);
                Student? student = _context.Student.FirstOrDefault(s => s.UserID == (user.Id));
                if (student != null)
                    System.Console.WriteLine(student.ToString());
            }
            // System.Console.WriteLine(user.Administrator.ToString());
            return View(user);
        }

        [AllowAnonymous]
        public ViewResult Create()
        {
            ViewData["SchoolID"] = new SelectList(_context.School, "SchoolID", "Name");
            return View();
        }

        //POST: Account/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountCreationViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AccountType = model.AccountType
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    if (model.AccountType == AccountType.Student)
                    {
                        Student student = new Student
                        {
                            UserID = user.Id,
                            Name = model.Name,
                            Email = model.Email,
                            SchoolID = model.SchoolID
                        };
                        _context.Student.Add(student);
                        await _context.SaveChangesAsync();
                    }
                    else if (model.AccountType == AccountType.Administrator)
                    {
                        Administrator admin = new Administrator
                        {
                            UserID = user.Id,
                            Name = model.Name,
                            Email = model.Email,
                            SchoolID = model.SchoolID
                        };
                        _context.Administrator.Add(admin);
                        await _context.SaveChangesAsync();
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ViewData["SchoolID"] = new SelectList(_context.School, "SchoolID", "Name");
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser? user = await userManager.FindByNameAsync(id);
            ViewData["SchoolID"] = new SelectList(
                _context.School,
                "SchoolID",
                "Name",
                user?.Student?.SchoolID ?? user?.Administrator?.SchoolID
            );
            if (user != null)
            {
                Student? student = _context.Student.FirstOrDefault(s => s.UserID.Equals(user.Id));
                Administrator? admin = _context.Administrator.FirstOrDefault(
                    s => s.UserID.Equals(user.Id)
                );
                AccountEditorViewModel model =
                    student != null
                        ? new AccountEditorViewModel(student)
                        : new AccountEditorViewModel(admin!);
                // model.UserName = user.UserName!;
                // model.Name = student?.Name ?? admin?.Name;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("UserName,AccountType,Name,SchoolID")] AccountEditorViewModel model
        )
        {
            ViewData["SchoolID"] = new SelectList(
                _context.School,
                "SchoolID",
                "Name",
                model?.Student?.SchoolID ?? model?.Administrator?.SchoolID
            );
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Student != null)
                        _context.Update(model.Student);
                    if (model.Administrator != null)
                        _context.Update(model.Administrator);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.StackTrace.ToString());
                }
            }
            return View(model);
        }
    }
}
