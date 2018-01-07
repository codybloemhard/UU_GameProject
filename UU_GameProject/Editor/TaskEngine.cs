using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace UU_GameProject
{
    public interface _work
    {
        bool Done();
        void CallBack();
    }

    public struct Work<T> : _work
    {
        public Action<T> callback;
        public Task<T> task;

        public Work(Task<T> task, Action<T> callback)
        {
            this.task = task;
            this.callback = callback;
        }

        public bool Done()
        {
            return task.IsCompleted;
        }

        public void CallBack()
        {
            callback(task.Result);
        }
    }

    public class TaskEngine
    {
        private List<_work> tasks, done;

        public TaskEngine()
        {
            tasks = new List<_work>();
            done = new List<_work>();
        }

        public void Update()
        {
            for(int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].Done()) continue;
                tasks[i].CallBack();
                done.Add(tasks[i]);
            }
            for (int i = 0; i < done.Count; i++)
                tasks.Remove(done[i]);
            done.Clear();
        }

        public void Add<T>(Func<T> todo, Action<T> callback)
        {
            Task<T> t = Task.Run(todo);
            Work<T> work = new Work<T>(t, callback);
            tasks.Add(work);
        }
    }
}