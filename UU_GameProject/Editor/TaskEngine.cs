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
        public Action<Returner<T>> callback;
        public Task<Returner<T>> task;

        public Work(Task<Returner<T>> task, Action<Returner<T>> callback)
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

    public class Returner<T>
    {
        public T result;
        public string msg;

        public Returner(T result, string msg)
        {
            this.result = result;
            this.msg = msg;
        }
    }

    public class TaskEngine
    {
        private List<_work> tasks, done;
        private static List<TaskEngine> engines;

        static TaskEngine()
        {
            engines = new List<TaskEngine>();
        }

        public TaskEngine()
        {
            tasks = new List<_work>();
            done = new List<_work>();
            engines.Add(this);
        }

        public static void UpdateAll()
        {
            for (int i = 0; i < engines.Count; i++)
                engines[i].Update();
        }

        public void Update()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].Done()) continue;
                tasks[i].CallBack();
                done.Add(tasks[i]);
            }
            for (int i = 0; i < done.Count; i++)
                tasks.Remove(done[i]);
            done.Clear();
        }

        public void Add<T>(Func<Returner<T>> todo, Action<Returner<T>> callback)
        {
            Task<Returner<T>> t = Task.Run(todo);
            Work<T> work = new Work<T>(t, callback);
            tasks.Add(work);
        }
    }
}