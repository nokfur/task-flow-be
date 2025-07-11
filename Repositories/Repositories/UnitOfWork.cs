using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Repositories.GenericRepositories;

namespace Repositories.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Board> Boards { get; }
        IGenericRepository<Column> Columns { get; }
        IGenericRepository<WorkTask> Tasks { get; }
        IGenericRepository<Label> Labels { get; }
        IGenericRepository<BoardMember> BoardMembers { get; }
        IGenericRepository<TaskLabel> TaskLabels { get; }

        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementContext _context;
        private bool _disposed = false;

        private IGenericRepository<User> userRepository;
        private IGenericRepository<Board> boardRepository;
        private IGenericRepository<Column> columnRepository;
        private IGenericRepository<WorkTask> workTaskRepository;
        private IGenericRepository<Label> labelRepository;
        private IGenericRepository<BoardMember> boardMemberRepository;
        private IGenericRepository<TaskLabel> taskLabelRepository;

        public UnitOfWork(TaskManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IGenericRepository<User> Users => userRepository ??= new GenericRepository<User>(_context);
        public IGenericRepository<Board> Boards => boardRepository ??= new GenericRepository<Board>(_context);
        public IGenericRepository<Column> Columns => columnRepository ??= new GenericRepository<Column>(_context);
        public IGenericRepository<WorkTask> Tasks => workTaskRepository ??= new GenericRepository<WorkTask>(_context);
        public IGenericRepository<Label> Labels => labelRepository ??= new GenericRepository<Label>(_context);
        public IGenericRepository<BoardMember> BoardMembers => boardMemberRepository ??= new GenericRepository<BoardMember>(_context);
        public IGenericRepository<TaskLabel> TaskLabels => taskLabelRepository ??= new GenericRepository<TaskLabel>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
