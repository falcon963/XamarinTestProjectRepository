﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using SQLite;
using System.Linq;
using MvvmCross;
using TestProject.Core.ViewModels;
using TestProject.Core.DBConnection;
using System.Threading;
using TestProject.Core.Repositories.Interfaces;
using MvvmCross.Logging;
using TestProject.Core.DBConnection.Interfacies;

namespace TestProject.Core.Repositories
{
    public class TasksRepository 
        : BaseRepository<UserTask>, ITasksRepository
    {
        public TasksRepository(IDatabaseConnectionService dbConnection) : base(dbConnection)
        {
        }

        IEnumerable<UserTask> ITasksRepository.GetTasksList(int userId)
        {
            IEnumerable<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == userId);

            return listOfTasks;
        }
    }
}
