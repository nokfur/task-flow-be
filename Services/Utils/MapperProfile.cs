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
            CreateMap<User, UserProfileResponse>();

            CreateMap<User, UserDetailResponse>();

            CreateMap<UserRegisterRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<UserProfileUpdateRequest, User>();

            // ============================================ Board Member ============================================
            CreateMap<MemberAddRequest, BoardMember>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom((src, _, _, context) => context.TryGetItems(out var items) ? (string)items["BoardId"] : ""));

            CreateMap<BoardMember, MemberResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MemberId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Member.Email));

            // ============================================ Board ============================================
            CreateMap<Board, Board>()
                .ForMember(dest => dest.Members, opt => opt.Ignore())
                .ForMember(dest => dest.Columns, opt => opt.Ignore())
                .ForMember(dest => dest.Labels, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMembers, opt => opt.Ignore());

            CreateMap<Board, BoardPreviewResponse>()
                .ForMember(dest => dest.ColumnCount, opt => opt.MapFrom(src => src.Columns.Count()))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Columns.SelectMany(c => c.Tasks).Count()))
                .ForMember(dest => dest.LabelCount, opt => opt.MapFrom(src => src.Labels.Count()))
                .ForMember(dest => dest.isOwn, opt => opt.MapFrom((src, _, _, context) =>
                {
                    var userId = context.TryGetItems(out var items) ? (string)items["UserId"] : "";
                    return src.OwnerId.Equals(userId);
                }));
            
            CreateMap<Board, BoardDetailResponse>()
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns.OrderBy(c => c.Position)))
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels.OrderBy(l => l.Name)));

            CreateMap<Board, BoardTemplateResponse>()
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns.OrderBy(c => c.Position).Select(c => c.Title)));

            CreateMap<BoardTemplateAddRequest, Board>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.IsTemplate, opt => opt.MapFrom(_ => true));

            CreateMap<BoardAddRequest, Board>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.BoardMembers, opt => opt.Ignore());

            CreateMap<BoardUpdateRequest, Board>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            // ============================================ Column ============================================
            CreateMap<Column, Column>()
                .ForMember(dest => dest.Board, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore());

            CreateMap<Column, ColumnDetailResponse>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.OrderBy(t => t.Position))); ;
            
            CreateMap<ColumnTemplateAddRequest, Column>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<ColumnAddRequest, Column>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            
            CreateMap<ColumnUpdateRequest, Column>();

            // ============================================ Task ============================================
            CreateMap<WorkTask, WorkTask>()
                .ForMember(dest => dest.Column, opt => opt.Ignore())
                .ForMember(dest => dest.TaskLabels, opt => opt.Ignore())
                .ForMember(dest => dest.Labels, opt => opt.Ignore());

            CreateMap<WorkTask, TaskDetailResponse>()
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskLabels.Select(tl => tl.Label).OrderBy(l => l.Name)));
            
            CreateMap<TaskTemplateAddRequest, WorkTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<TaskAddRequest, WorkTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
            
            CreateMap<TaskUpdateRequest, WorkTask>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            // ============================================ Label ============================================
            CreateMap<Label, Label>()
                .ForMember(dest => dest.TaskLabels, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore());

            CreateMap<Label, LabelDetailResponse>();

            CreateMap<LabelTemplateAddRequest, Label>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<LabelAddRequest, Label>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            
            CreateMap<LabelUpdateRequest, Label>();
        }
    }
}
