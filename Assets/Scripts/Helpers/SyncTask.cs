using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTask 
{
    internal Action<SyncTask> _onDone;
    public Action OnDone;

    public void Done(){
        _onDone?.Invoke(this);
        OnDone?.Invoke();
    }

}

public class SyncTaskGroup{
    public List<SyncTask> tasks;
    public Action onAllDone;
    public SyncTaskGroup(List<SyncTask> tasks){
        this.tasks = tasks;
        
        foreach (var task in this.tasks)
        {
            task._onDone += OnTaskDone;
        }
    }

    public SyncTaskGroup(){
        tasks = new List<SyncTask>();
    }

    public void AddTask(SyncTask task){
        tasks.Add(task);
    }

    void OnTaskDone(SyncTask task){
        tasks?.Remove(task);
        
        if(tasks.Count == 0){
            onAllDone?.Invoke();
        }
    }
}
