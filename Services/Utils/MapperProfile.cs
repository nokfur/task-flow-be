using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Board.Response;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.Column.Response;
using BusinessObjects.DTOs.Label.Request;
using BusinessObjects.DTOs.Label.Response;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.DTOs.WorkTask.Request;
using BusinessObjects.DTOs.WorkTask.Response;
using BusinessObjects.Models;

namespace Services.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            // ============================================ User ============================================
            CreateMap<User, UserProfileResponseModel>();

            CreateMap<User, UserDetailResponseModel>();

            CreateMap<UserRegisterRequestModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            // ============================================ Board Member ============================================
            CreateMap<MemberAddRequestModal, BoardMember>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom((src, _, _, context) => context.Items["BoardId"]));

            // ============================================ Board ============================================
            CreateMap<Board, BoardResponseModel>()
                .ForMember(dest => dest.ColumnCount, opt => opt.MapFrom(src => src.Columns.Count()))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Columns.SelectMany(c => c.Tasks).Count()))
                .ForMember(dest => dest.LabelCount, opt => opt.MapFrom(src => src.Labels.Count()))
                .ForMember(dest => dest.isOwn, opt => opt.MapFrom((src, _, _, context) => src.OwnerId.Equals(context.Items["UserId"])));
            
            CreateMap<Board, BoardDetailResponseModel>()
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns.OrderBy(c => c.Position)))
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels.OrderBy(l => l.Name)));

            CreateMap<BoardTemplateAddRequestModel, Board>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.IsTemplate, opt => opt.MapFrom(_ => true));

            CreateMap<BoardAddRequestModel, Board>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<BoardUpdateRequestModel, Board>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            // ============================================ Column ============================================
            CreateMap<Column, ColumnDetailResponseModel>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.OrderBy(t => t.Position))); ;
            
            CreateMap<ColumnTemplateAddRequestModel, Column>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<ColumnAddRequestModel, Column>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            
            CreateMap<ColumnUpdateRequestModel, Column>();

            // ============================================ Task ============================================
            CreateMap<WorkTask, TaskDetailResponseModel>()
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskLabels.Select(tl => tl.Label).OrderBy(l => l.Name)));
            
            CreateMap<TaskTemplateAddRequestModel, WorkTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<TaskAddRequestModel, WorkTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
            
            CreateMap<TaskUpdateRequestModel, WorkTask>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            // ============================================ Label ============================================
            CreateMap<Label, LabelDetailResponseModel>();
            
            CreateMap<LabelTemplateAddRequestModel, Label>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<LabelAddRequestModel, Label>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            
            CreateMap<LabelUpdateRequestModel, Label>();
        }
    }
}
