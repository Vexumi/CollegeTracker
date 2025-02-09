using AutoMapper;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class UserService: IUserService
{
    private readonly KSTDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IAuthorizationService authorizationService;

    public UserService(
        KSTDbContext dbContext,
        IMapper mapper,
        IAuthorizationService authorizationService
        )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.authorizationService = authorizationService;
    }

    public async Task<long> CreateAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(userViewModel);
        user.PasswordHash = authorizationService.HashPassword(userViewModel.Password);
            
        var entity = await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task<UserViewModel> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var user = await GetAllUsers().FirstAsync(x => x.Id == id, cancellationToken);
        return mapper.Map<UserViewModel>(user);
    }

    public IEnumerable<UserViewModel> GetAll()
        => GetAllUsers().Select(mapper.Map<UserViewModel>);

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var userEntity = await dbContext.Users.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Users.Remove(userEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserViewModel> UpdateAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(userViewModel);
        dbContext.Attach(user);
        dbContext.Entry(user).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);

        return userViewModel;
    }
    
    public IQueryable<User> GetAllUsers()
        => dbContext.Users.AsNoTracking();
}