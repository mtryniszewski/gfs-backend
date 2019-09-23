using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Shared;
using GFS.Transfer.User.Commands;
using GFS.Transfer.User.Data;
using GFS.Transfer.User.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class UserService : IUserService
    {
     
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(GfsDbContext context, UserManager<User> userManager, IMapper mapper,
            IEmailService emailService, IOptions<Dictionary> dictionary)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _dictionary = dictionary.Value;
        }

        public async Task<UserDto> GetAsync(GetUserQuery query)
        {
            var user = await _context.Users
                .ProjectTo<UserDto>()
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            return user;
        }

        public async Task<PageListDto<UserBasicDto>> ListAsync(ListUserQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Users
                    .Where(x => !x.IsArchival && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                  || query.SearchBy.Any(y => x.Surname.ToLower()
                                                      .Contains(y.ToLower())) ||
                                                  query.SearchBy.Any(y => x.Email.ToLower().Contains(y.ToLower()))))
                    .ProjectTo<UserBasicDto>()
                    .OrderBy(x => x.Surname)
                    .ToPagedListAsync(query);

            return await _context.Users
                .Where(x => !x.IsArchival)
                .ProjectTo<UserBasicDto>()
                .OrderBy(x => x.Surname)
                .ToPagedListAsync(query);
        }

        public async Task<PageListDto<UserBasicDto>> ListArchivesAsync(ListUserQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Users
                    .Where(x => x.IsArchival && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                 || query.SearchBy.Any(y => x.Surname.ToLower()
                                                     .Contains(y.ToLower())) ||
                                                 query.SearchBy.Any(y => x.Email.ToLower().Contains(y.ToLower()))))
                    .ProjectTo<UserBasicDto>()
                    .OrderBy(x => x.Surname)
                    .ToPagedListAsync(query);

            return await _context.Users
                .Where(x => x.IsArchival)
                .ProjectTo<UserBasicDto>()
                .OrderBy(x => x.Surname)
                .ToPagedListAsync(query);
        }

        public async Task<string> CreateAsync(CreateUserCommand command)
        {
            var userIdentity = _mapper.Map<User>(command);
            userIdentity.Permissions = Permissions.Standard;
            userIdentity.IsActive = false;

            if (_context.Users.Where(x => x.Email == userIdentity.Email).ToList().Count > 0)
                throw new GfsException(ErrorCode.UserExists, _dictionary.UserExists);

           

            var result = await _userManager.CreateAsync(userIdentity, command.Password);
            if (!result.Succeeded)
                throw new GfsException(ErrorCode.UserRegistrationFailed,_dictionary.UserRegistrationFailed);
            await SendActivationLinkAsync(command.Email);
            return userIdentity.Id;
        }

        public async Task UpdateAsync(UpdateUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            if (command.Name != null && command.Name != user.Name)
                user.Name = command.Name;
            if (command.Surname != null && command.Surname != user.Surname)
                user.Surname = command.Surname;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);
            }
          
        }

        public async Task ActivateByLinkAsync(ActivateAccountCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.AccountActivationToken == command.AccountActivationToken);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            if (user.AccountActivationToken != command.AccountActivationToken)
                throw new GfsException(ErrorCode.WrongActivationToken, _dictionary.WrongActivationToken);

            user.IsActive = true;
            user.AccountActivationToken = null;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordInProfileAsync(ChangePasswordCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            if (!await _userManager.CheckPasswordAsync(user, command.OldPassword))
                throw new GfsException(ErrorCode.WrongPassword, _dictionary.WrongPassword);

            if (command.NewPassword == null)
                throw new GfsException(ErrorCode.EmptyPassword, _dictionary.EmptyPassword);

            var hash = _userManager.PasswordHasher.HashPassword(user, command.NewPassword);
            if (hash == null)
                throw new GfsException(ErrorCode.EmptyPassword,_dictionary.EmptyPassword);

            user.PasswordHash = hash;

            await _context.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            var token = Guid.NewGuid();
            if (token == null)
                throw new GfsException(ErrorCode.EmptyToken, _dictionary.EmptyToken);

            var msg = _dictionary.ResetMailMessage + " http://localhost:4200/#/auth/reset?token=" + token;

            user.PasswordResetToken = token.ToString();
            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync(user.Email, _dictionary.ResetMailSubject, msg);
        }

        public async Task ResetPasswordAsync(ResetPasswordCommand command)
        {
            
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.PasswordResetToken == command.Token);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            if (command.NewPassword == null)
                throw new GfsException(ErrorCode.EmptyPassword,_dictionary.EmptyPassword);

            var hash = _userManager.PasswordHasher.HashPassword(user, command.NewPassword);
            if (hash == null)
                throw new GfsException(ErrorCode.ChangePasswordFailed, _dictionary.EmptyPassword);

            user.PasswordHash = hash;
            user.PasswordResetToken = null;

            await _context.SaveChangesAsync();
        }

        public async Task GrantAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.Permissions = Permissions.Admin;
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.Permissions = Permissions.Standard;
            await _context.SaveChangesAsync();
        }

        public async Task ActivateAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.IsActive = true;
            user.IsArchival = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.IsArchival = true;
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task DearchiveAsync(ManageUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            user.IsArchival = false;
            await _context.SaveChangesAsync();
        }

        public async Task<User> ValidateUser(LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new GfsException(ErrorCode.WrongEmail,_dictionary.WrongEmail);

            if (!await _userManager.CheckPasswordAsync(user, command.Password))
                throw new GfsException(ErrorCode.WrongPassword,_dictionary.WrongPassword);

            if (!user.IsActive)
                throw new GfsException(ErrorCode.AccountNotActive,_dictionary.AccountNotActive);

            return user;
        }

        private async Task SendActivationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound,_dictionary.UserNotFound);

            var token = Guid.NewGuid();
            if (token == null)
                throw new GfsException(ErrorCode.EmptyToken,_dictionary.EmptyToken);

            var msg = _dictionary.ActivationMailMessage
                      + "http://localhost:4200/#/auth/activate?token=" + token;
            user.AccountActivationToken = token.ToString();

            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync(user.Email,_dictionary.ActivationMailSubject, msg);
        }
    }
}