using System.Threading.Tasks;
using GFS.Data.Model.Entities;
using GFS.Transfer.Shared;
using GFS.Transfer.User.Commands;
using GFS.Transfer.User.Data;
using GFS.Transfer.User.Queries;

namespace GFS.Domain.Core
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(GetUserQuery query);
        Task<PageListDto<UserBasicDto>> ListAsync(ListUserQuery query);
        Task<PageListDto<UserBasicDto>> ListArchivesAsync(ListUserQuery query);
        Task<string> CreateAsync(CreateUserCommand command);
        Task UpdateAsync(UpdateUserCommand command);
        Task DeleteAsync(DeleteUserCommand command);
        Task ActivateByLinkAsync(ActivateAccountCommand command);
        Task ChangePasswordInProfileAsync(ChangePasswordCommand command);
        Task ForgotPasswordAsync(ForgotPasswordCommand command);
        Task ResetPasswordAsync(ResetPasswordCommand command);
        Task GrantAsync(ManageUserCommand command);
        Task RevokeAsync(ManageUserCommand command);
        Task ActivateAsync(ManageUserCommand command);
        Task DeactivateAsync(ManageUserCommand command);
        Task ArchiveAsync(ManageUserCommand command);
        Task DearchiveAsync(ManageUserCommand command);
        Task<User> ValidateUser(LoginCommand command);
    }
}