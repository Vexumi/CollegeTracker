using AutoMapper;
using CollegeTracker.Business.Interfaces;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess;
using CollegeTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.Business.Services;

public class UserService: IUserService
{
    private readonly CollegeTrackerDbContext dbContext;
    private readonly IMapper mapper;

    public UserService(
        CollegeTrackerDbContext dbContext,
        IMapper mapper
        )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(userViewModel);
        user.PasswordHash = GetPasswordHash(userViewModel.Password);
            
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
    
    private IQueryable<User> GetAllUsers()
        => dbContext.Users.AsNoTracking();

    private string GetPasswordHash(string password)
    {
        return password; //TODO create authservice
    }

}