using TaskManagementSystem.Models.General;

namespace TaskManagementSystem.Repository
{
    public interface ITaskMasterRepository
    {
        RetDataSet GetItemTask();
        RetDataSet GetItemTaskByID(int Id);

        RetDBAction TaskMasterSave(TaskItem task);

        RetDBAction TaskMasterUpdate(TaskItem task);

        RetDBAction TaskMasterPartialUpdate(int taskId, Dictionary<string, object> fieldsToUpdate);

        RetDBAction TaskMasterDelete(TaskItem task);
    }
}
