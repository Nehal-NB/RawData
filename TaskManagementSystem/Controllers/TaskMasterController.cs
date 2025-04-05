using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.General;
using TaskManagementSystem.Repository;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskMasterController : ControllerBase
    {
        [HttpGet]
        public RetDataSet GetItemTask()
        {
            RetDataSet ret = new RetDataSet();

            try
            {
                ITaskMasterRepository ast = new TaskMasterRepository();

                ret = ast.GetItemTask();

                ast = null;

            }
            catch (Exception ex)
            {

                ret.ErrorDescription = ex.ToString();

                ret.Status = false;
            }
            return ret;

        }


        [HttpGet]
        public RetDataSet GetItemTaskByID(int Id)
        {
            RetDataSet ret = new RetDataSet();

            try
            {
                ITaskMasterRepository ast = new TaskMasterRepository();

                ret = ast.GetItemTaskByID(Id);

                ast = null;

            }
            catch (Exception ex)
            {

                ret.ErrorDescription = ex.ToString();

                ret.Status = false;
            }
            return ret;

        }


        [HttpPost]
        public RetDBAction TaskMasterSave(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();

            try
            {
                ITaskMasterRepository ast = new TaskMasterRepository();

                ret = ast.TaskMasterSave(task);

                ast = null;

            }
            catch (Exception ex)
            {

                ret.ErrorDescription = ex.ToString();

                ret.Status = false;
            }
            return ret;

        }


        [HttpPut]
        public RetDBAction TaskMasterUpdate(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();

            try
            {
                ITaskMasterRepository ast = new TaskMasterRepository();

                ret = ast.TaskMasterUpdate(task);

                ast = null;

            }
            catch (Exception ex)
            {

                ret.ErrorDescription = ex.ToString();

                ret.Status = false;
            }
            return ret;

        }


        [HttpPatch]
        public ActionResult<RetDBAction> TaskMasterPatch([FromBody] TaskItem task)
        {
            RetDBAction ret = new RetDBAction();

            try
            {
                ITaskMasterRepository repository = new TaskMasterRepository();
                ret = repository.TaskMasterUpdate(task);
            }
            catch (Exception ex)
            {
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
            }

            return Ok(ret);
        }

        [HttpDelete]
        public RetDBAction TaskMasterDelete(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();

            try
            {
                ITaskMasterRepository ast = new TaskMasterRepository();

                ret = ast.TaskMasterDelete(task);

                ast = null;

            }
            catch (Exception ex)
            {

                ret.ErrorDescription = ex.ToString();

                ret.Status = false;
            }
            return ret;

        }
    }
}
