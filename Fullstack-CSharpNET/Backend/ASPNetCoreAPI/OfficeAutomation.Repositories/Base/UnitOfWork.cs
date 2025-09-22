// Implements the Unit of Work pattern to coordinate repositories and commit changes to the database
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Base;
using OfficeAutomation.Repositories.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly OfficeAutomationDbContext _context;

    // Injects DbContext and repository instances
    public UnitOfWork(
        OfficeAutomationDbContext context,
        ITaskRepository taskRepository,
        ICartableRepository cartableRepository,
        IAuthRepository authRepository,
        IUserSessionRepository userSessionRepository)
    {
        _context = context;
        Tasks = taskRepository;
        Cartables = cartableRepository;
        Auth = authRepository;
        UserSessions = userSessionRepository;
    }

    public ITaskRepository Tasks { get; }
    public ICartableRepository Cartables { get; }
    public IAuthRepository Auth { get; }
    public IUserSessionRepository UserSessions { get; }

    // Commits all changes in a single transaction
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Disposes the DbContext
    public void Dispose()
    {
        _context.Dispose();
    }
}
