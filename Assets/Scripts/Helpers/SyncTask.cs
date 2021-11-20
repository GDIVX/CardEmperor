using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTask 
{
    public Action<SyncTask> onTaskDone;

    public void Done(){
        onTaskDone?.Invoke(this);
    }

}

public class SyncTaskGroup{
    public List<SyncTask> tasks;
    public Action onAllDone;
    public SyncTaskGroup(List<SyncTask> tasks){
        this.tasks = tasks;
        
        foreach (var task in this.tasks)
        {
            task.onTaskDone += OnTaskDone;
        }
    }


    void OnTaskDone(SyncTask task){
        tasks?.Remove(task);
        
        if(tasks.Count == 0){
            onAllDone?.Invoke();
        }
    }
}
