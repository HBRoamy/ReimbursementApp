using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reimbursement_Portal_API.Models;
using Shared.User;
using Shared.UserService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Reimbursement_Portal_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;
        private readonly IConfiguration configuration;
        //private readonly ICurrentUserInfoService currentUser;

        public AccountController(UserManager<Employee> userManager, SignInManager<Employee> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration; // to be used for jwt tokens
            //this.currentUser = currentUser;
        }


        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel newUserDetails) // there are fromQuery, fromRoute, fromURI and fromForm too 
        {
            if (ModelState.IsValid)
            {//fails automatically when the email already exists due to the configuration in startup class
                var user = new Employee
                {
                    FullName = newUserDetails.FullName,
                    UserName = newUserDetails.Email.ToLower(),
                    Email = newUserDetails.Email.ToLower(),
                    PanNumber = newUserDetails.PanNumber.ToLower(),
                    BankName = newUserDetails.BankName.ToLower(),
                    BankAccountNumber = newUserDetails.BankAccountNumber,
                    IsApprover =  newUserDetails.IsApprover
                };

                var result = await userManager.CreateAsync(user, newUserDetails.Password);//password added here so that its hashing happens
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, isPersistent: false); // it logs in after signup
                    return Ok(result.Succeeded);
                }
                
            }
            //return Unauthorized();
            return StatusCode(StatusCodes.Status400BadRequest, "Either user data is invalid or the user already exists");
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false); //model.email is matching it to username (PasswordsigninAsync requires username and not email so I copied username to email and added a new Full Name property)
                
                if (!result.Succeeded || result==null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return Unauthorized();  
                }
                var userBody = await userManager.FindByEmailAsync(model.Email);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim("email",model.Email),
                    new Claim("IsApprover", userBody.IsApprover.ToString() ),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
                //var currentUser = await userManager.GetUserAsync(User);
                return Ok(new { token=generatedToken, email = userBody.Email, isApprover= userBody.IsApprover.ToString() });
            }
            return BadRequest();
        }

        //[HttpGet]
        ////make it authorized
        //public ActionResult<string> getCurrentUserEmail()
        //{
        //    return currentUser.GetCurrentUserEmail();
        //}

        //[HttpGet("CurrentRoleUser")]
        //public async Task<ActionResult> getCurrentUserEmailWithRole()
        //{
        //    //if(signInManager.IsSignedIn())
        //    //var userEmail = currentUser.GetCurrentUserEmail();
        //    //if (userEmail != null)
        //    //{
        //    //    var userBody = await userManager.FindByEmailAsync(userEmail);
        //    //    return Ok(new { email = userEmail, isApprover = userBody.IsApprover });

        //    //}
        //    var currentUser = await userManager.GetUserAsync(User);
        //    if (currentUser!=null)
        //    {
        //    var email = currentUser != null ? currentUser.Email : "";
        //    var userBody = await userManager.FindByEmailAsync(email);
        //    return Ok(new { Email=email, isApprover=userBody.IsApprover });

        //    }

            

        //    return NotFound();
            
        //}

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(true);
        }
    }
}
