using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using System.Linq;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using iSoft.Common.Enums;
using SourceBaseBE.CommonFunc.CommonFuncNS;
using NPOI.Util;
using Sprache;
using SourceBaseBE.CommonFunc.DataService;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using iSoft.Common.ExtensionMethods;
using System.Net.Http.Headers;
using iSoft.Database.Entities;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using iSoft.Common.Utils;
using PRPO.MainService.CustomAttributes;
using SourceBaseBE.MainService.CustomAttributes;
using Nest;

namespace SourceBaseBE.MainService.Controllers
{

    [ApiController]
    [Route("api/v1/Virtual")]
    public class VirtualController : BaseCRUDController<EmployeeEntity, EmployeeRequestModel, EmployeeResponseModel>
    {
        private EmployeeService _employeeService;
        private AuthPermissionService authPermissionService;
        private AuthGroupService authGroupService;
        private DepartmentService _departmentService;
        private JobTitleService _jobTitleService;
        private WorkingDayService _workingDayService;
        private WorkingDayUpdateService workingDayUpdateService;
        private WorkingTypeService _workingTypeService;
        private UserService _userService;
        public VirtualController(EmployeeService service,
          DepartmentService departmentService,
          JobTitleService jobTitleService,
          WorkingDayService workingDayService,
          WorkingDayUpdateService workingDayUpdateService,
          WorkingTypeService workingTypeService,
          AuthPermissionService authPermissionService,
          AuthGroupService authGroupService,
          UserService _userService,
        ILogger<EmployeeController> logger)
          : base(service, logger)
        {
            _departmentService = departmentService;
            _jobTitleService = jobTitleService;
            _workingDayService = workingDayService;
            _baseCRUDService = service;
            _employeeService = (EmployeeService)_baseCRUDService;
            _workingTypeService = workingTypeService;
            this.workingDayUpdateService = workingDayUpdateService;
            this.authPermissionService = authPermissionService;
            this.authGroupService = authGroupService;
            this._userService = _userService;
        }

        [HttpPost]
        [Route("virtual-initial-database")]
        public async Task<IActionResult> AddDataDatabase()
        {
            string funcName = nameof(AddDataDatabase);
            Messages.Message errMessage = null;
            List<EmployeeEntity> _employees = new List<EmployeeEntity>();
            List<DepartmentEntity> _department = new List<DepartmentEntity>();
            List<JobTitleEntity> _jobTitle = new List<JobTitleEntity>();
            List<WorkingDayEntity> _workingDay = new List<WorkingDayEntity>();

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);


                int lengDepartment = 10;
                for (int i = 0; i < lengDepartment; i++)
                {
                    DepartmentEntity departmentEntity = new DepartmentEntity();
                    departmentEntity.Order = i;
                    departmentEntity.Name = "Department" + i.ToString();
                    _department.Add(departmentEntity);
                    //_departmentService.UpsertIfNotExist(departmentEntity);
                }
                _departmentService.InsertMulti(_department);

                int lengJobTitle = 10;

                for (int i = 0; i < lengJobTitle; i++)
                {
                    JobTitleEntity jobTitleEntity = new JobTitleEntity();
                    jobTitleEntity.Id = 0;
                    jobTitleEntity.Order = i;
                    jobTitleEntity.Name = "Jobtitle" + i.ToString();
                    _jobTitle.Add(jobTitleEntity);
                    //_jobTitleService.UpsertIfNotExist(jobTitleEntity);
                }
                _jobTitleService.InsertMulti(_jobTitle);

                Random random = new Random();

                int lengEmployee = 500;

                for (long i = 0; i < lengEmployee; i++)
                {
                    EmployeeEntity employeeEntity = new EmployeeEntity();
                    employeeEntity.Id = 0;
                    employeeEntity.Order = i;
                    employeeEntity.Name = "Requester" + i.ToString();
                    employeeEntity.Address = "27 Xuan Quynh, Thu Duc";
                    employeeEntity.EmployeeCode = $"{i.ToString().PadLeft(4, '0')}";
                    employeeEntity.EmployeeMachineCode = $"{i.ToString().PadLeft(4, '0')}";
                    employeeEntity.PhoneNumber = $"{i.ToString().PadLeft(11, '0')}";
                    employeeEntity.Email = FakeData.GenerateRandomEmail();
                    employeeEntity.DepartmentId = random.Next(1, lengDepartment - 1);
                    employeeEntity.JobTitleId = random.Next(1, lengJobTitle - 1);
                    _employees.Add(employeeEntity);
                    //_employeeRepository.UpsertIfNotExist(employeeEntity);
                }
                _employeeService.InsertMulti(_employees);


                long lengWorkingDate = 30000;

                for (long i = 0; i < lengWorkingDate; i++)
                {
                    WorkingDayEntity workingDayEntity = new WorkingDayEntity();
                    workingDayEntity.Order = i;
                    workingDayEntity.Time_In = DateTime.Now.AddHours(6);
                    workingDayEntity.Time_Out = DateTime.Now.AddHours(16);
                    workingDayEntity.TimeDeviation = 10;
                    workingDayEntity.WorkingDate = DateTime.Now;
                    int a = random.Next(0, 3);
                    workingDayEntity.WorkingDayStatus = (EnumWorkingDayStatus)a;
                    int inout = random.Next(0, 3);
                    workingDayEntity.InOutState = (EnumInOutTypeStatus)inout;
                    workingDayEntity.EmployeeEntityId = random.Next(1, lengEmployee - 1);
                    //workingDayEntity.WorkingTypeEntityId = random.Next(0, 49);
                    //_workingDay.Add(workingDayEntity);
                    _workingDayService.Upsert(workingDayEntity);
                }
                //_workingDayService.InsertMulti(_workingDay);


                //Virtual User

                //var employees = _employeeService.GetList();
                //var departments = _departmentService.GetList();
                //for (int i = 0; i < employees.Count; i++)
                //{
                //	UserEntity userEntity = new UserEntity();
                //	userEntity.Role = "Admin";
                //	userEntity.EmployeeId = employees[i]?.Id;
                //	userEntity.Password = $"Admin{i}";
                //	userEntity.Username = $"Admin{i}";
                //	userEntity.Avatar = "/Images/Upload/20240430_004210_58456414_LOGOCTY_re.png";
                //	_userService.UpsertIfNotExist(userEntity);
                //}
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }


        [HttpPost]
        [Route("workingtype-defaut")]
        public async Task<IActionResult> AddWorkingDateType()
        {
            string funcName = nameof(AddDataDatabase);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // làm ca 1 hoặc ca 2 bthg
                WorkingTypeEntity workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "0";
                workingTypeEntity.Name = "0";
                workingTypeEntity.Description = "Ngày hôm đó chỉ làm bình thường theo ca 1 hoặc ca 2";
                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);
                //C3
                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "C3";
                workingTypeEntity.Name = "C3";
                workingTypeEntity.Description = "Ngày hôm đó chỉ đi làm theo ca 3";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 8;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 8;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 8;
                _workingTypeService.Upsert(workingTypeEntity);



                //1C3

                for (int i = 1; i < 9; i++)
                {
                    workingTypeEntity = new WorkingTypeEntity();
                    workingTypeEntity.Code = $"{i}C3";
                    workingTypeEntity.Name = $"{i}C3";
                    workingTypeEntity.Description = "\"Sau khi đi làm theo ca sáng bình thường thì tiếp tục làm thêm vào ca 3 theo một số giờ cụ thể";

                    workingTypeEntity.Normal_Meal = 0;
                    workingTypeEntity.OT_150 = 0;
                    workingTypeEntity.Normal_Night_30 = 0;
                    workingTypeEntity.OT_200 = i;

                    workingTypeEntity.Weekend_Meal = 0;
                    workingTypeEntity.Weekend_OT_200 = 8;
                    workingTypeEntity.Weekend_Night_OT_270 = i;

                    workingTypeEntity.Holiday_Meal = 0;
                    workingTypeEntity.Holiday_OT_300 = 8;
                    workingTypeEntity.Holiday_OT_Night_390 = i;
                    _workingTypeService.Upsert(workingTypeEntity);
                }

                //1+C3
                for (int i = 1; i < 17; i++)
                {
                    workingTypeEntity = new WorkingTypeEntity();
                    workingTypeEntity.Code = $"{i}+C3";
                    workingTypeEntity.Name = $"{i}+C3";
                    workingTypeEntity.Description = "Ca chính là ca 3, có làm thêm một số giờ trước khi vào ca 3. (Có thể giờ làm thêm rơi ca 1 hoặc ca 2, tuy nhiên cách tính là như nhau: tính tổng số giờ đã làm thêm từ 0g đến 22g của ngày hôm đó).\r\nVí dụ: Làm việc từ 10g tối chủ nhật đến 10 g sáng thứ hai. Sau đó tiếp tục làm từ 10g tối thứ hai đến 10g sáng thứ ba.\r\nNhư vậy, vào ngày chủ nhật chỉ ghi là C3. Đến ngày thứ hai sẽ ghi là 4+C3, và tương tự thứ ba cũng ghi nhận là 4+C3";

                    workingTypeEntity.Normal_Meal = 0;
                    workingTypeEntity.OT_150 = i;
                    workingTypeEntity.Normal_Night_30 = 8;
                    workingTypeEntity.OT_200 = 0;

                    workingTypeEntity.Weekend_Meal = 0;
                    workingTypeEntity.Weekend_OT_200 = i;
                    workingTypeEntity.Weekend_Night_OT_270 = 8;

                    workingTypeEntity.Holiday_Meal = 0;
                    workingTypeEntity.Holiday_OT_300 = i;
                    workingTypeEntity.Holiday_OT_Night_390 = 8;
                    _workingTypeService.Upsert(workingTypeEntity);
                }

                //1-16
                for (int i = 1; i < 17; i++)
                {
                    workingTypeEntity = new WorkingTypeEntity();
                    workingTypeEntity.Code = $"{i}";
                    workingTypeEntity.Name = $"{i}";
                    workingTypeEntity.Description = "Số giờ làm thêm (không rơi vào ca 3)";

                    workingTypeEntity.Normal_Meal = 0;
                    workingTypeEntity.OT_150 = i;
                    workingTypeEntity.Normal_Night_30 = 0;
                    workingTypeEntity.OT_200 = 0;

                    workingTypeEntity.Weekend_Meal = 0;
                    workingTypeEntity.Weekend_OT_200 = i;
                    workingTypeEntity.Weekend_Night_OT_270 = 0;

                    workingTypeEntity.Holiday_Meal = 0;
                    workingTypeEntity.Holiday_OT_300 = i;
                    workingTypeEntity.Holiday_OT_Night_390 = 0;
                    _workingTypeService.Upsert(workingTypeEntity);
                }

                //1M-16M

                for (int i = 1; i < 17; i++)
                {
                    workingTypeEntity = new WorkingTypeEntity();
                    workingTypeEntity.Code = $"{i}M";
                    workingTypeEntity.Name = $"{i}M";
                    workingTypeEntity.Description = "Số giờ làm thêm (không rơi vào ca 3) được trả thêm tiền cơm nếu nhà ăn không chuẩn bị cơm";

                    workingTypeEntity.Normal_Meal = 1;
                    workingTypeEntity.OT_150 = i;
                    workingTypeEntity.Normal_Night_30 = 0;
                    workingTypeEntity.OT_200 = 0;

                    workingTypeEntity.Weekend_Meal = 1;
                    workingTypeEntity.Weekend_OT_200 = i;
                    workingTypeEntity.Weekend_Night_OT_270 = 0;

                    workingTypeEntity.Holiday_Meal = 0;
                    workingTypeEntity.Holiday_OT_300 = i;
                    workingTypeEntity.Holiday_OT_Night_390 = 0;
                    if (i == 12)
                    {
                        workingTypeEntity.Weekend_Meal = 2;
                    }
                    _workingTypeService.Upsert(workingTypeEntity);

                }

                //C3+M

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "C3+M";
                workingTypeEntity.Name = "C3+M";
                workingTypeEntity.Description = "Làm ca 3 được trả thêm tiền cơm ( trường hợp đặc biệt nếu nhà ăn không chuẩn bị cơm ngày đó )";

                workingTypeEntity.Normal_Meal = 1;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 8;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 1;
                workingTypeEntity.Weekend_OT_200 = 8;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 1;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 8;
                _workingTypeService.Upsert(workingTypeEntity);

                //P

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "P";
                workingTypeEntity.Name = "P";
                workingTypeEntity.Description = "Nghỉ việc riêng được hưởng lương (nghỉ phép, kết hôn, tang chế)";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

                //P/2

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "P/2";
                workingTypeEntity.Name = "P/2";
                workingTypeEntity.Description = "Nghỉ việc riêng được hưởng lương nửa ngày (nghỉ phép, kết hôn, tang chế)";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

                //UP

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "UP";
                workingTypeEntity.Name = "UP";
                workingTypeEntity.Description = "Nghỉ không lương";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

                //S

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "S";
                workingTypeEntity.Name = "S";
                workingTypeEntity.Description = "Nghỉ bệnh 100%";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

                //S75

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "S75";
                workingTypeEntity.Name = "S75";
                workingTypeEntity.Description = "Nghỉ bệnh 75%";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);


                //MR

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "MR";
                workingTypeEntity.Name = "MR";
                workingTypeEntity.Description = "Nghỉ thai sản";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

                //SW

                workingTypeEntity = new WorkingTypeEntity();
                workingTypeEntity.Code = "SW";
                workingTypeEntity.Name = "SW";
                workingTypeEntity.Description = "Ngưng việc";

                workingTypeEntity.Normal_Meal = 0;
                workingTypeEntity.OT_150 = 0;
                workingTypeEntity.Normal_Night_30 = 0;
                workingTypeEntity.OT_200 = 0;

                workingTypeEntity.Weekend_Meal = 0;
                workingTypeEntity.Weekend_OT_200 = 0;
                workingTypeEntity.Weekend_Night_OT_270 = 0;

                workingTypeEntity.Holiday_Meal = 0;
                workingTypeEntity.Holiday_OT_300 = 0;
                workingTypeEntity.Holiday_OT_Night_390 = 0;
                _workingTypeService.Upsert(workingTypeEntity);

            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpPost]
        [Route("working-day")]
        public async Task<IActionResult> FakeDataWorkingDay([FromQuery] int year, int month)
        {
            string funcName = nameof(FakeDataWorkingDay);
            Messages.Message errMessage = null;
            for (int j = 1; j <= DateTime.DaysInMonth(year, month); j++)
            {
                try
                {
                    this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                    var wTypes = _workingTypeService.GetList().Select(x => x.Id).ToList();
                    await _workingDayService.CreateRandomWdIfNotExist(new DateTime(year, month, j));

                }
                catch (BaseException ex)
                {
                    errMessage = Messages.ErrBaseException.SetParameters(ex);
                }
                catch (Exception ex)
                {
                    errMessage = Messages.ErrException.SetParameters(ex);
                }
                if (errMessage != null)
                {
                    this._logger.LogMsg(errMessage);
                    return this.ResponseError(errMessage);
                }
            }
            return this.ResponseOk("Fake success");

        }

        [HttpPost]
        [Route("user-defaut")]
        public async Task<IActionResult> VirtualUser()
        {
            string funcName = nameof(VirtualUser);
            Messages.Message errMessage = null;
            List<UserEntity> users = new List<UserEntity>();
            // Authen for User Admin
            List<DepartmentAdminEntity> listDepartmentAdmin;
            DepartmentAdminEntity departmentAdminEntity;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var employees = _employeeService.GetAll();
                var departments = _departmentService.GetAll();

                listDepartmentAdmin = new List<DepartmentAdminEntity>();

                // Add 1 account Usser
                var user = new UserEntity();
                user.Username = "Admin";
                user.Password = EncodeUtil.MD5($"Vuletech@123");
                user.Role = EnumUserRole.Admin.ToString();

                //find UserId
                var userId = _userService.GetUserByName(user.Username);

                foreach (EnumDepartmentAdmin role in EnumDepartmentAdmin.GetValues(typeof(EnumDepartmentAdmin)))
                {
                    if (role == EnumDepartmentAdmin.None)
                    {
                        continue;
                    }
                    departmentAdminEntity = new DepartmentAdminEntity();
                    departmentAdminEntity.UserId = userId;
                    departmentAdminEntity.Role = role;
                    listDepartmentAdmin.Add(departmentAdminEntity);

                }

                //Add User && DepartmentAdmin
                _userService.UpsertTransaction(user, listDepartmentAdmin, null);


                if (employees == null || employees.Count() <= 0)
                {
                    throw new DBException("Null Employee");
                }
                else
                {
                    foreach (var employee in employees)
                    {
                        UserEntity userEntity = new UserEntity();
                        departmentAdminEntity = new DepartmentAdminEntity();
                        listDepartmentAdmin = new List<DepartmentAdminEntity>();

                        //Add User
                        userEntity.EmployeeId = employee.Id;
                        userEntity.Password = EncodeUtil.MD5($"Syngenta@123");
                        userEntity.Username = employee?.Name.RemoveUnicode().ToLower();
                        userEntity.Displayname = employee?.Name;
                        userEntity.Birthday = employee?.Birthday;
                        userEntity.Address = employee?.Address;
                        userEntity.Gender = employee?.Gender;
                        userEntity.PhoneNumber = employee?.PhoneNumber;
                        userEntity.Email = employee?.Email;
                        userEntity.Role = EnumUserRole.User.ToString();

                        //Add DepartmentAdmin
                        departmentAdminEntity.DepartmentId = employee?.DepartmentId;
                        departmentAdminEntity.UserId = userEntity?.Id;
                        departmentAdminEntity.Role = EnumDepartmentAdmin.User;
                        listDepartmentAdmin.Add(departmentAdminEntity);

                        long? departmentId = employee != null ? employee.DepartmentId : 0;

                        _userService.UpsertTransaction(userEntity, listDepartmentAdmin, employee?.DepartmentId);
                    }
                    return this.ResponseOk("Create User Success");
                }
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpPost]
        [Route("department-admin")]
        public async Task<IActionResult> VirtualDepartmentAdmin()
        {
            string funcName = nameof(VirtualDepartmentAdmin);
            Messages.Message errMessage = null;
            List<UserEntity> users = new List<UserEntity>();

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var employees = _employeeService.GetList();
                //var departments = _departmentService.GetList();
                for (int i = 0; i < employees.Count; i++)
                {
                    UserEntity userEntity = new UserEntity();
                    userEntity.Role = "Admin";
                    userEntity.EmployeeId = employees[i]?.Id;
                    userEntity.Password = $"Admin{i}";
                    userEntity.Username = $"Admin{i}";
                    userEntity.Avatar = "/Images/Upload/20240430_004210_58456414_LOGOCTY_re.png";
                    _userService.Upsert(userEntity);
                }
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        private async Task PostDataWDU(WorkingDayUpdateRequestModel request)
        {
            string funcName = nameof(PostDataWDU);
            if (request == null || request.EditerId == null || request.WorkingDayId == null) throw new Exception("Invalid parameter");
            try
            {
                var user = await _userService.GetByIdAsync(request.EditerId.GetValueOrDefault());
                var workday = await _workingDayService.GetByIdAsync(request.WorkingDayId.GetValueOrDefault());
                if (user == null) throw new Exception($"Requester not found");
                if (workday == null) throw new Exception($"Workday not found");
                var workingDayApprove = new WorkingDayApprovalEntity()
                {
                    ApproveStatus = Database.Enums.EnumApproveStatus.PENDING,
                    WorkingDayId = workday.Id,
                    ApproverId = request.EditerId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.EditerId,
                };
                var newUpdate = request.GetEntity(workday, request);
                newUpdate.WorkingDayApprovals = new List<WorkingDayApprovalEntity>();
                newUpdate.WorkingDayApprovals.Add(workingDayApprove);
                workingDayUpdateService.Upsert(newUpdate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("working-day-update")]
        public async Task<IActionResult> FakeDataWorkingDayUpdate()
        {
            string funcName = nameof(FakeDataWorkingDayUpdate);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var woDs = _workingDayService.GetList();
                var user = _userService.GetList().FirstOrDefault();
                var workingType = _workingTypeService.GetList();
                int firstWorkingTypeId = 0;
                int lastWorkingTypeId = 0;
                // Get the first and last elements
                if (workingType != null)
                {
                    var firstWorkingType = workingType[0];
                    var lastWorkingType = workingType[workingType.Count - 1];

                    // Extract the IDs
                    firstWorkingTypeId = (int)firstWorkingType.Id;
                    lastWorkingTypeId = (int)lastWorkingType.Id;
                }
                else
                {
                    throw new Exception($"WorkingType not found");
                }
                long userId = 0;
                if (user != null) userId = user.Id;
                else
                {
                    throw new Exception($"Requester not found");
                }
                for (int i = 0; i < woDs.Count; i++)
                {
                    await PostDataWDU(new WorkingDayUpdateRequestModel()
                    {
                        EditerId = userId,
                        WorkingTypeId = new Random().Next(firstWorkingTypeId, lastWorkingTypeId),
                        WorkingDayId = woDs[i].Id,
                    });
                }


            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpPost]
        [Route("create-wd-default")]
        public async Task<IActionResult> CreateWdDefault(int year, int month)
        {
            string funcName = nameof(CreateWdDefault);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                {
                    await _workingDayService.CreateWdIfNotExist(new DateTime(DateTime.Now.Year, DateTime.Now.Month, i));
                }
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpPost]
        [UserPermission(EnumUserRole.Root)]
        [Route("create-permissions")]
        public async Task<IActionResult> CreatePermissions()
        {
            string funcName = nameof(CreatePermissions);
            Messages.Message errMessage = null;
            List<UserEntity> users = new List<UserEntity>();

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var authGroups = authGroupService.GetList();
                if (authGroups == null || authGroups.Count <= 0)
                {
                    foreach (var type in Enum.GetValues<EnumDepartmentAdmin>())
                        authGroupService.Upsert(new AuthGroupEntity()
                        {
                            CreatedAt = DateTime.Now,
                            Name = type.ToString(),
                            UpdatedAt = DateTime.Now,
                        });
                }
                authGroups = authGroupService.GetList();
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
    }
}