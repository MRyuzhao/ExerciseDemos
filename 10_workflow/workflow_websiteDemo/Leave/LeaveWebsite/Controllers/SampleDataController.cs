using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LeaveWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;

namespace LeaveWebsite.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IWorkflowHost _workflowHost;
        private readonly IMapper _mapper;

        public SampleDataController(IMapper mapper,
            IWorkflowHost workflowHost)
        {
            _mapper = mapper;
            _workflowHost = workflowHost;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Leave leave)
        {
            var leaveModel = new Leave
            {
                ApplyUser = leave.ApplyUser,
                ApplyContent = leave.ApplyContent
            };

            var workflowId = _workflowHost.StartWorkflow("LeaveWorkflow", leaveModel).Result;

            leaveModel.WorkflowId = workflowId;
            LeaveStore.Add(leaveModel);

            return Ok();
        }

        [HttpPut("exam/{id}/{level}")]
        public IActionResult Exam(string id, ExamState level)
        {
            if (level == ExamState.一级审批通过)
            {
                _workflowHost.PublishEvent("ExamedLevelOne", id, null);
            }
            else if (level == ExamState.二级审批通过)
            {
                _workflowHost.PublishEvent("ExamedLevelTwo", id, null);
            }
            else if (level == ExamState.三级审批通过)
            {
                _workflowHost.PublishEvent("ExamedLevelThree", id, null);
            }

            return Ok();
        }

        [HttpPut("reject/{id}")]
        public IActionResult Reject(Guid id)
        {
            LeaveStore.Leaves.ForEach(x =>
            {
                if (x.WorkflowId == id.ToString() && x.ExamState != ExamState.三级审批通过)
                {
                    x.ExamState = ExamState.已驳回;
                    _workflowHost.PublishEvent("Rejected", id.ToString(), null);
                }
            });

            return Ok();
        }

        [HttpGet]
        public IEnumerable<LeaveViewModel> GetAll()
        {
            var result = LeaveStore.Leaves.Select(x => _mapper.Map<LeaveViewModel>(x));
            return result;
        }
    }

    public enum ExamState
    {
        已驳回 = -1,
        未审批 = 0,
        一级审批通过 = 1,
        二级审批通过 = 2,
        三级审批通过 = 3
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Leave, LeaveViewModel>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Id.ToString()))
                .ForMember(d => d.CreateDate, s => s.MapFrom(x => x.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(d => d.ExamState, s => s.MapFrom(x => Enum.GetName(typeof(ExamState), x.ExamState)));
        }
    }

    public class LeaveStore
    {
        public static readonly List<Leave> Leaves = new List<Leave>();

        public static void Add(Leave leave)
        {
            Leaves.Add(leave);
        }

        public static void Edit(Leave leave)
        {
            Leaves.ForEach(x =>
            {
                if (x.Id == leave.Id)
                {
                    x = leave;
                }
            });
        }

        public static Leave Get(Guid id)
        {
            return Leaves.SingleOrDefault(x => x.Id == id);
        }
    }
}
